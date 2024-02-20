using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUnlockCanvas : MonoBehaviour
{
    [SerializeField, FoldoutGroup("References")]
    private Image icon;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Show(AbstractWeapon recentUnlock)
    {
        anim?.Play("LevelUpCanvas");
        icon.sprite = recentUnlock.sprite;
    }
}
