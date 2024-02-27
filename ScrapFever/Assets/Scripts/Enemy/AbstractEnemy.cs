using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(DamageZone))]
public abstract class AbstractEnemy : MonoBehaviour, IPoolable, IDamageAble, IDamageSource
{
    public event Action<AbstractEnemy> died;
    public event Action<AbstractEnemy> outOfScreen;

    const float ROTATIONSPEED = 10;

    [SerializeField, FoldoutGroup("Settings")]
    protected FloatStat hp;

    [SerializeField, FoldoutGroup("Settings")]
    private float healthDropPercentChance = 0.01f;

    #region Component references
    [SerializeField]
    private EnemyNavigation navigation;

    [SerializeField]
    private EnemyVFX visuals;

    private Rigidbody rb;
    private EnemyMesh mesh;
    private Collider collider_;
    private Animator anim;
    private DamageZone damageZone;
    #endregion

    public string Name => visuals.Name;
    public string Description => visuals.Description;
    public string glueSourceName => visuals.glueSourceName;

    public Sprite sprite => visuals.sprite;
    public string glueName => visuals.glueName;

    private void Awake()
    {
        var damage = GetComponent<DamageZone>();
        damage.overrideSource = this;
        damage.damageDealt += (int damage, IDamageAble target) => this.TakeDamage(Mathf.RoundToInt(hp.value * 0.1f), this);

        GetComponentInChildren<Dissolve>().dissolved += () => Pool.Return<AbstractEnemy>(this.gameObject);
    }

    private void SetUp()
    {
        this.enabled = true;

        #region GetReferences
        if(rb == null) rb = GetComponent<Rigidbody>();
        if(collider_ == null) collider_ = GetComponent<Collider>();
        if(mesh == null) mesh = GetComponentInChildren<EnemyMesh>();
        if(anim == null) anim = GetComponentInChildren<Animator>();
        if(damageZone == null) damageZone = GetComponent<DamageZone>();
        #endregion

        collider_.enabled = true;
        damageZone.enabled = true;
        damageZone.overrideSource = this;
        rb.isKinematic = false;

        navigation.SetUp(transform.position);
        visuals.TryPlaySpawnAnimation(transform);

        StartCoroutine(ReturnIfOutOfScreen());
        InternalSetUp();
    }

    private void Update()
    {
        if (rb == null || rb.isKinematic) return;

        var dir = navigation.GetMovementDirection();
        if (dir == new Vector3()) return;

        dir.Normalize();
        dir.y = 0;
        var rot = Quaternion.LookRotation(dir);

        transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * ROTATIONSPEED);
    }

    private void FixedUpdate()
    {
        if (rb == null || rb.isKinematic) return;

        navigation.FixedUpdate(transform.position);
        navigation.Move(rb);
    }

    public void TakeDamage(int damage, IDamageSource source)
    {
        if (damage <= 0 || !this.enabled) return;

        hp -= damage;
        AchivementManager.ProgressAchivement<DamageAchivement>(damage, new[] {source});
        AchivementManager.ProgressAchivement<TotalDamageAchivement>(damage);

        mesh.Blink();
        visuals.TryPlayDamageEffects(damage, transform);

        if (hp <= 0) 
        {
            if (!TryDieOnScreen())
            {
                Die();
            }
        }
    }

    public void SetTargetTransform(Transform t) => navigation.SetTarget(t);

    private bool TryDieOnScreen()
    {
        if (!this.gameObject.IsOnScreen(1)) return false;

        DropItem("XP Item");

        if (UnityEngine.Random.Range(0, 1f) < healthDropPercentChance)
        {
            DropItem("HealthItem");
        }

        Die();

        return true;
    }

    protected virtual void Die()
    {
        AchivementManager.ProgressAchivement<KillAchivement>(1, new[] { this });

        StopAllCoroutines();

        visuals.TryPlayDeathEffects(transform);

        collider_.enabled = false;
        this.enabled = false;

        damageZone.enabled = false;
        damageZone.ClearTargets();

        died?.Invoke(this);
        outOfScreen = null;
        died = null;

        visuals.PlayDeathVisuals(transform, anim, rb);
    }

    private void DropItem(string Name)
    {
        var item = Pool.Get<AbstractItem>(Name);
        if (item == null) return;

        var dropPos = transform.position;
        dropPos.y = -0.95f;
        item.transform.position = dropPos;
    }

    private IEnumerator ReturnIfOutOfScreen()
    {
        while (true)
        {
            if (!this.gameObject.IsOnScreen(1))
            {
                var timer = 0f;
                while (!this.gameObject.IsOnScreen(1))
                {
                    timer += Time.deltaTime;
                    if (timer > 1.5)
                    {
                        outOfScreen?.Invoke(this);
                    }
                    yield return new WaitForEndOfFrame();
                }
            }

            yield return new WaitForSeconds(1);
        }
    }
    
    protected virtual void InternalSetUp()
    {

    }

    #region IPoolable
    public void OnReturnedToPool()
    {
        if (this.gameObject.activeInHierarchy)
        {
            anim?.Rebind();
            anim?.Update(0f);
        }

        StopAllCoroutines();

        hp?.Reset();
        mesh?.Reset();

        this.enabled = false;
    }

    public void OnTakenFromPool()
    {
        SetUp();
    }
    #endregion
}