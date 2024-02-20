using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OtherAchievmentDisplay : MonoBehaviour
{
    [SerializeField, FoldoutGroup("References")]
    private Transform content;
    [SerializeField, FoldoutGroup("References")]
    private AchivementDetailButton buttonTemplate;
    [SerializeField, FoldoutGroup("References")]
    private AchievementGroupDetails detailWindow;

    [SerializeField, FoldoutGroup("References")]
    private Sprite timeIcon;
    [SerializeField, FoldoutGroup("References")]
    private Sprite damageTakenIcon;
    [SerializeField, FoldoutGroup("References")]
    private Sprite damageDealtIcon;
    [SerializeField, FoldoutGroup("References")]
    private Sprite maxLevelIcon;

    private void OnEnable()
    {
        var buttons = GetComponentsInChildren<AchivementDetailButton>();
        var groups = GetAchievementGroups();

        for(int i = 0; i < buttons.Length; i++)
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

            if (group[0] is TimeAchievement)
            {
                icon = timeIcon;
            }
            if (group[0] is TotalDamageAchivement)
            {
                icon = damageDealtIcon;
            }
            if (group[0] is DamageSurvivedAchievement)
            {
                icon = damageTakenIcon;
            }
            if (group[0] is LevelAchivement)
            {
                icon = maxLevelIcon;
            }

            buttons[i].SetUp(group, icon, name, description);
            buttons[i].clicked += detailWindow.ShowGroup;
        }

    }

    private List<List<AbstractAchivement>> GetAchievementGroups()
    {
        Dictionary<string, List<AbstractAchivement>> groups = new Dictionary<string, List<AbstractAchivement>>();

        foreach (var achivement in AchivementManager.GetAchivements())
        {
            string[] components = achivement.saveId.Split(AchivementManager.SEPERATOR);
            if (components.Length == 0) continue;
            if (components[0] == DamageAchivement.DAMAGEACHIVEMENTNAME) continue;
            if (components[0] == KillAchivement.KILLACHIVEMENTNAME) continue;

            var achievmentType = components[0];
            if (groups.Keys.Contains(achievmentType))
            {
                groups[achievmentType].Add(achivement);
            }
            else
            {
                var list = new List<AbstractAchivement>();
                list.Add(achivement);
                groups.Add(achievmentType, list);
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
