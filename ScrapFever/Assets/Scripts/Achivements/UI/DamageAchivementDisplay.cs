using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class DamageAchivementDisplay : MonoBehaviour
{
    [SerializeField, FoldoutGroup("References")]
    private Transform content;
    [SerializeField, FoldoutGroup("References")]
    private AchivementDetailButton buttonTemplate;
    [SerializeField, FoldoutGroup("References")]
    private AchievementGroupDetails detailWindow;

    [SerializeField, FoldoutGroup("References")]
    private List<AbstractWeapon> referenceWeapons;

    private void Awake()
    {
        foreach (var button in GetComponentsInChildren<AchivementDetailButton>())
        {
            button.clicked += detailWindow.ShowGroup;
        }
    }

    private void OnEnable()
    {
        var buttons = GetComponentsInChildren<AchivementDetailButton>();
        var groups = GetAchievementGroups();

        for (int i = 0; i < buttons.Length; i++)
        {
            if (i >= groups.Count)
            {
                Destroy(buttons[i].gameObject);
                continue;
            }

            var group = groups[i];

            var description = "";
            var name = "";
            Sprite icon = null;

            foreach (var weapon in referenceWeapons)
            {
                if (group[0].saveId.Split(AchivementManager.SEPERATOR)[1] == weapon.glueSourceName)
                {
                    description = weapon.Description;
                    name = weapon.Name;
                    icon = weapon.sprite;
                }
            }

            buttons[i].transform.SetParent(content, false);
            buttons[i].SetUp(group, icon, name, description);
        }
    }

    private List<List<AbstractAchivement>> GetAchievementGroups()
    {
        Dictionary<string, List<AbstractAchivement>> groups = new Dictionary<string, List<AbstractAchivement>>();

        foreach (var achivement in AchivementManager.GetAchivements())
        {
            string[] components = achivement.saveId.Split(AchivementManager.SEPERATOR);
            if (components.Length == 0) continue;
            if (components[0] != DamageAchivement.DAMAGEACHIVEMENTNAME) continue;

            var targetName = components[1];
            if (groups.Keys.Contains(targetName))
            {
                groups[targetName].Add(achivement);
            }
            else
            {
                var list = new List<AbstractAchivement>();
                list.Add(achivement);
                groups.Add(targetName, list);
            }
        }

        List<List<AbstractAchivement>> result = new List<List<AbstractAchivement>>();
        foreach (var key in groups.Keys)
        {
            result.Add(groups[key]);
        }

        return result;
    }
}
