using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour, IDamageAble, IDamageSource
{
    private PlayerInputActions input;

    public string glueSourceName => "GluePlayer";

    #region Player Components
    [SerializeField, PropertyOrder(-3)]
    private PlayerMovement movement;

    [SerializeField, PropertyOrder(-2)]
    private PlayerWeapons weapons;

    private Rigidbody rb;
    #endregion

    #region Settings
    [SerializeField, FoldoutGroup("Settings")]
    private IntStat hp;

    [SerializeField, FoldoutGroup("Settings")]
    public int LevelUpHP;

    [SerializeField, FoldoutGroup("Settings")]
    private float debugStartLevel;

    [SerializeField, FoldoutGroup("Settings")]
    public List<int> xpUntilLevelUpHistory;
    #endregion

    #region Visuals
    #region Particles
    [SerializeField, FoldoutGroup("References")]
    private ParticleSystem damageParticles;

    [SerializeField, FoldoutGroup("References")]
    private ParticleSystem healParticles;

    [SerializeField, FoldoutGroup("References")]
    private ParticleSystem levelUpParticles;

    [SerializeField, FoldoutGroup("References")]
    private ParticleSystem deathVFX;
    #endregion

    [SerializeField, FoldoutGroup("References")]
    private Animator anim;

    private PlayerVignette vignette;
    private PlayerUI ui;
    private Healthbar healthbar;
    private PlayerMesh mesh;
    #endregion

    private float currentXP = 0;
    private int currentLevel = 0;

    private void Awake()
    {
        Time.timeScale = 1;
        weapons.AwakeSetUp(StartCoroutine);

        #region Get References
        input = new PlayerInputActions();
        input.Enable();

        ui = GetComponentInChildren<PlayerUI>();
        healthbar = GetComponentInChildren<Healthbar>();
        mesh = GetComponentInChildren<PlayerMesh>();
        vignette = GetComponentInChildren<PlayerVignette>();
        rb = GetComponent<Rigidbody>();
        #endregion
    }

    private void Start()
    {
        ui?.SetLevel(currentLevel);
        weapons.SetUp();

        StartCoroutine(delay());

        IEnumerator delay()
        {
            for (int i = 0; i < debugStartLevel - 1; i++)
            {
                yield return new WaitForSeconds(0.5f);
                LevelUp();
            }
        }
    }

    private void Update()
    {
        if (Time.timeScale == 0.0f) return;

        movement.MovementUpdate(input);
    }

    private void FixedUpdate()
    {
        movement.FixedMovementUpdate(transform, rb, anim);
    }

    public void EarnXP(float amount)
    {
        if (!this.enabled) return;

        var XPUntilLevelUp = 1;

        if (currentLevel < xpUntilLevelUpHistory.Count)
        {
            XPUntilLevelUp = xpUntilLevelUpHistory[currentLevel];
        }
        else
        {
            XPUntilLevelUp = xpUntilLevelUpHistory.Last();
        }

        currentXP += amount;

        ui?.SetXPBar(currentXP, XPUntilLevelUp);

        while (currentXP >= XPUntilLevelUp)
        {
            LevelUp();
            currentXP -= XPUntilLevelUp;

            ui?.SetXPBar(currentXP, XPUntilLevelUp);
        }
    }

    private void LevelUp()
    {
        if (!this.enabled) return;

        RestoreHealth(LevelUpHP);

        levelUpParticles.Play();

        var audio = Pool.Get<PoolableSound>("LevelUpSound");
        if (audio != null) audio.transform.position = transform.position;

        AchivementManager.ProgressAchivement<LevelAchivement>(1);

        if (weapons.IsMaxed())
        {
            ui?.SetLevel("Max");
            return;
        }

        weapons.LevelUp();
        currentLevel++;
        ui?.SetLevel(currentLevel + 1);
    }

    public void TakeDamage(int damage, IDamageSource source)
    {
        if (!this.enabled) return;
        if (damage == 0) return;

        hp -= damage;

        healthbar.SetTo(hp.totalValue / (float)hp.value);
        mesh.Blink();
        vignette.Blink();
        damageParticles.Play();

        var damageAudio = Pool.Get("PlayerDamageSound");
        if(damageAudio = null) damageAudio.transform.position = transform.position;

        if (hp <= 0)
        {
            StartCoroutine(Die());
            return;
        }

        AchivementManager.ProgressAchivement<DamageSurvivedAchievement>(damage);
    }

    private IEnumerator Die()
    {
        this.enabled = false;
        input.Disable();

        healthbar.SetTo(0);
        anim.SetTrigger("Die");
        deathVFX.Play();

        rb.velocity = new Vector3();
        rb.isKinematic = true;

        yield return new WaitForSeconds(2);
        GameUI.ShowGameOverScreen();
    }

    public void RestoreHealth(int healing)
    {
        if (!this.enabled) return;

        hp += healing;
        healthbar?.SetTo(hp.totalValue / (float)hp.value);
        healParticles.Play();
    }
}
