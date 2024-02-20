using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AchivementDetailButton : CustomUIElement, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public event Action<List<AbstractAchivement>, Sprite, string, string> clicked;

    private Animator anim;

    [SerializeField, FoldoutGroup("References")]
    private Image fill;
    [SerializeField, FoldoutGroup("References")]
    private TextMeshProUGUI fillProgress;
    [SerializeField, FoldoutGroup("References")]
    private Image icon;
    [SerializeField, FoldoutGroup("References")]
    private Sprite finishedSprite;
    [SerializeField, FoldoutGroup("References")]
    private Image background;

    private string groupName;
    private string groupDescription;
    private List<AbstractAchivement> achivementGroup;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void SetUp(List<AbstractAchivement> achivementGroup, Sprite groupIcon, string groupName, string groupDescription)
    {
        this.achivementGroup = achivementGroup;
        this.groupName = groupName;
        this.groupDescription = groupDescription;
        icon.sprite = groupIcon;

        float total = 0;
        foreach(var achivement in achivementGroup)
        {
            total += achivement.GetProgressPercent();
        }

        float totalPercent = total / (float)achivementGroup.Count;
        if (totalPercent == 1) background.sprite = finishedSprite;

        fillProgress.text = $"{Mathf.RoundToInt(totalPercent * 100)}%";
        fill.fillAmount = totalPercent;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        anim.SetBool("Highlighted", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        anim.SetBool("Highlighted", false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        clicked?.Invoke(achivementGroup, icon.sprite, groupName, groupDescription);
    }
}
