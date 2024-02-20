using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyVFX
{
    public enum DeathVFXType
    {
        smallExplosion,
        mediumExplosion
    }

    private readonly Dictionary<DeathVFXType, string> vfxNames = new Dictionary<DeathVFXType, string>()
    {
        [DeathVFXType.smallExplosion] = "Smoll_Explosion_VFX",
        [DeathVFXType.mediumExplosion] = "Explosion_VFX"
    };

    [SerializeField, TabGroup("VFX Settings")]
    private bool ignoreDeathAnimation;
    [SerializeField, TabGroup("VFX Settings")]
    private bool manifestOnSpawn;
    [SerializeField, TabGroup("VFX Settings")]
    private DeathVFXType explosionType = DeathVFXType.mediumExplosion;
    [SerializeField, TabGroup("VFX Settings")]
    public string glueName;
    [SerializeField, TabGroup("VFX Settings"), FilePath(ParentFolder = "Assets/Resources", Extensions = ".csv", IncludeFileExtension = false, RequireExistingPath = true)]
    private string filePath;
    [field: SerializeField, Required, PreviewField(ObjectFieldAlignment.Left), TabGroup("VFX Settings"), PropertyOrder(1)]
    public Sprite sprite { get; private set; }

    public string Name { get { return CSVLanguageFileParser.GetLangDictionary(filePath, SelectedLanguage.value)[$"{glueName}_Name"]; } }
    public string Description { get { return CSVLanguageFileParser.GetLangDictionary(filePath, SelectedLanguage.value)[$"{glueName}_Description"]; } }
    public string glueSourceName => glueName;

    public void TryPlayDamageEffects(int damage, Transform transform)
    {
        var damageNumber = Pool.Get<DamageNumber>("DamageNumber");
        if (damageNumber != null)
        {
            damageNumber.transform.position = transform.position;
            damageNumber.SetUp(damage);
        }

        var damageAudio = Pool.Get("DamageSound");
        if (damageAudio != null)
        {
            damageAudio.transform.position = transform.position;
        }
    }

    public void TryPlayDeathEffects(Transform transform)
    {
        var vfx = Pool.Get(vfxNames[explosionType]);
        if (vfx != null)
        {
            vfx.transform.position = transform.position;
        }

        var killAudio = Pool.Get("EnemyDeathSound");
        if (killAudio != null)
        {
            killAudio.transform.position = transform.position;
        }
    }

    public bool TryPlaySpawnAnimation(Transform transform)
    {
        if (!manifestOnSpawn) return false;

        transform.GetComponentInChildren<Dissolve>().Begin(dissolveMode.manifest);

        return true;
    }

    public void PlayDeathVisuals(Transform transform, Animator anim, Rigidbody rb)
    {
        if (ignoreDeathAnimation || anim == null)
        {
            transform.GetComponentInChildren<Dissolve>().Begin(dissolveMode.dissolve);
        }
        else
        {
            anim.SetTrigger("Die");
            rb.isKinematic = true;
        }
    }
}
