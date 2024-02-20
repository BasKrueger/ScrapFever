using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementGroupDetails : CustomUIElement
{
    [SerializeField, FoldoutGroup("References")]
    private Transform content;
    [SerializeField, FoldoutGroup("References")]
    private AchivementCard cardTemplate;
    [SerializeField, FoldoutGroup("References")]
    private Image icon;
    [SerializeField, FoldoutGroup("References")]
    private TextMeshProUGUI titleName;
    [SerializeField, FoldoutGroup("References")]
    private TextMeshProUGUI titleDescription;

    public void ShowGroup(List<AbstractAchivement> group, Sprite icon, string titleName, string titleDescription)
    {
        this.titleName.text = titleName;
        this.titleDescription.text = titleDescription;
        this.icon.sprite = icon;

        foreach(Transform t in content)
        {
            Destroy(t.gameObject);
        }

        foreach(var achievement in group)
        {
            var card = Instantiate(cardTemplate);
            card.transform.SetParent(content.transform, false);
            card.SetUp(achievement);
        }

        base.Show();
        StartCoroutine(delay());
    }

    IEnumerator delay()
    {
        yield return new WaitForSecondsRealtime(0.4f);
        foreach(var card in GetComponentsInChildren<AchivementCard>())
        {
            yield return new WaitForSecondsRealtime(0.15f);
            card.Show();
        }
    }
}
