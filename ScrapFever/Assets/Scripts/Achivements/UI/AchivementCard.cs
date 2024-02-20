using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchivementCard : CustomUIElement
{
    [SerializeField, FoldoutGroup("References")]
    private TextMeshProUGUI title;
    [SerializeField, FoldoutGroup("References")]
    private TextMeshProUGUI description;
    [SerializeField, FoldoutGroup("References")]
    private Image fill;
    [SerializeField, FoldoutGroup("References")]
    private TextMeshProUGUI percent;
    [SerializeField, FoldoutGroup("References")]
    private Sprite completeBackground;
    [SerializeField, FoldoutGroup("References")]
    private Image background;

    public void SetUp(AbstractAchivement achivement)
    {
        this.title.text = achivement.GetName();
        this.description.text = achivement.GetDescription();

        percent.text = achivement.GetProgressString();
        fill.fillAmount = achivement.GetProgressPercent();

        if(achivement.GetProgressPercent() == 1)
        {
            background.sprite = completeBackground;
        }
    }
}
