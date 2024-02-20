using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public abstract class AbstractWeapon : MonoBehaviour, IDamageSource
{
    public enum WeaponTargets
    {
        closest,
        furthest
    }

    public event Action fired;
    public event Action<float> cooldownUpdated;
    public event Action<float> upgraded;

    #region Settings
    [SerializeField, EnumToggleButtons, TabGroup("General")]
    private WeaponTargets targetMode;
    [SerializeField, TabGroup("General")]
    protected int damage = 1;
    [SerializeField, SuffixLabel("shots per Second", true), TabGroup("General")]
    protected float fireRate = 0.25f;
    [SerializeField, TabGroup("General"), LabelText("Line Of Sight")]
    private bool requiresLineOfSight = false;

    [SerializeField, ListDrawerSettings(AddCopiesLastElement = true, CustomAddFunction = "GetNewUpgrade")]
    private List<WeaponUpgrade> upgrades;
    #endregion

    #region Visuals
    [field: SerializeField, Required, PreviewField(ObjectFieldAlignment.Left), TabGroup("VFX & SFX"), PropertyOrder(1)]
    public Sprite sprite { get; private set; }
    [field: SerializeField, Required, PreviewField(ObjectFieldAlignment.Left), TabGroup("VFX & SFX"), PropertyOrder(1)]
    public VideoClip clip { get; private set; }
    
    [SerializeField, TabGroup("VFX & SFX"), PropertyOrder(1)]
    private AudioSource fireSFX;
    [SerializeField, TabGroup("VFX & SFX"), PropertyOrder(1)]
    private ParticleSystem particle;
    [SerializeField, TabGroup("VFX & SFX"), PropertyOrder(1)]
    private string weaponName;
    [SerializeField, TabGroup("VFX & SFX"), PropertyOrder(1), FilePath(ParentFolder = "Assets/Resources", Extensions = ".csv", IncludeFileExtension = false, RequireExistingPath = true)]
    private string localizationPath;
    #endregion

    public string Name { get { return CSVLanguageFileParser.GetLangDictionary(localizationPath, SelectedLanguage.value)[$"{weaponName}_Name"]; ; } }
    public string Description { get { return CSVLanguageFileParser.GetLangDictionary(localizationPath, SelectedLanguage.value)[$"{weaponName}_Description"]; ; } }
    public string glueSourceName => weaponName;

    private WeaponUpgrade lastUpgrade;
    protected float activeCooldown;

    private void Awake()
    {
        InternalAwake();
    }

    protected abstract void Fire(Transform target);

    protected abstract List<string> GetSpecificUpgradeNames();

    protected abstract void SpecificUpgrade(WeaponUpgrade upgrade);

    public void Upgrade()
    {
        var nextUpgrade = GetNextUpgrade();
        if(nextUpgrade == null)
        {
            Debug.LogError("Error: tried to upgrade a maxed out Weapon");
            return;
        }

        targetMode = nextUpgrade.newTarget;
        damage += nextUpgrade.bonusDamage;
        fireRate += nextUpgrade.bonusFireRate;

        SpecificUpgrade(nextUpgrade);

        lastUpgrade = nextUpgrade;
        upgraded?.Invoke((float)(upgrades.IndexOf(lastUpgrade) + 1) / (float)(upgrades.Count));
    }

    private WeaponUpgrade GetNextUpgrade()
    {
        if (upgrades.Count <= 0) return null;
        if (lastUpgrade == null) return upgrades[0];

        var lastIndex = upgrades.IndexOf(lastUpgrade);
        lastIndex++;
        if (lastIndex > upgrades.Count - 1) return null;

        return upgrades[lastIndex];
    }

    public string GetNextUpgradeDescription()
    {
        var nextUpgrade = GetNextUpgrade();
        if (nextUpgrade == null) return "maxed";

        return nextUpgrade.GetDescription();
    }

    public bool IsMaxed() => GetNextUpgrade() == null;

    private void Update()
    {
        if(TickDown())
        {
            var target = GetTarget();
            if (target == null) return;
           
            #region Reset Visuals
            foreach (var anim in GetComponentsInChildren<Animator>())
            {
                anim.Rebind();
                anim.Update(0f);
            }
            if (fireSFX != null)
            {
                fireSFX.Stop();
                fireSFX.Play();
            }
            if (particle != null)
            {
                particle.Stop();
                particle.Play();
            }
            #endregion

            var pos = target.transform.position;
            pos.y = transform.position.y;
            transform.LookAt(pos);

            Fire(target);
            fired?.Invoke();
        }

        InternalUpdate();
    }

    private bool TickDown()
    {
        activeCooldown -= Time.deltaTime;
        cooldownUpdated?.Invoke(activeCooldown / (1 / fireRate));

        if (activeCooldown <= 0)
        {
            activeCooldown = 1 / fireRate;
            return true;
        }
        return false;
    }

    protected T GetProjectile<T>(string name) where T : AbstractProjectile
    {
        var projectile = Pool.Get<T>(name);
        projectile.transform.position = transform.position;
        projectile.SetUp(damage, this);

        return projectile;
    }

    #region Internal Voids
    protected virtual void InternalUpdate() { }

    protected virtual void InternalAwake() { }

    protected virtual void InternalDrawGizmosSelected() { }
    #endregion

    #region target Selection
    protected Transform GetTarget()
    {
        switch (targetMode)
        {
            case WeaponTargets.closest:
                return GetClosestTarget();
            case WeaponTargets.furthest:
                return GetFurthestTarget();
        }

        return null;
    }

    private Transform GetClosestTarget()
    {
        GameObject closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (var enemy in Pool.GetOutOfPools<AbstractEnemy>())
        {
            var distance = Vector3.Distance(enemy.transform.position, transform.position);
            if (distance < closestDistance)
            {
                if (requiresLineOfSight && !LineOfSightTo(enemy)) continue;

                closest = enemy;
                closestDistance = distance;
            }
        }

        if (closest == null) return null;

        return closest.transform;
    }

    private bool LineOfSightTo(GameObject target)
    {
        var ray = new Ray(transform.position, (target.transform.position - transform.position));
        var hit = new RaycastHit();
        int layer = 1 << LayerMask.NameToLayer("Obstacle") | 1 << LayerMask.NameToLayer("PlayerOnlyWall");

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
        {
            if (Vector3.Distance(transform.position, hit.point) > Vector3.Distance(transform.position, target.transform.position)) return true;

            return false;
        }

        return true;
    }

    private Transform GetFurthestTarget()
    {
        GameObject closest = null;
        float highestDistance = 0;

        foreach (var enemy in Pool.GetOutOfPools<AbstractEnemy>())
        {
            var distance = Vector3.Distance(enemy.transform.position, transform.position);
            if (distance > highestDistance)
            {
                closest = enemy;
                highestDistance = distance;
            }
        }

        return closest.transform;
    }
    #endregion

    //Used by Odin
    private WeaponUpgrade GetNewUpgrade()
    {
        var upgrade = new WeaponUpgrade();
        upgrade.SetUp(GetSpecificUpgradeNames());
        return upgrade;
    }

    private void OnDrawGizmosSelected()
    {
        InternalDrawGizmosSelected();
    }
}
