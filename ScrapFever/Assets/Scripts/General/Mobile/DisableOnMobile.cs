using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class DisableOnMobile : MonoBehaviour
{
    [SerializeField]
    private bool completely;

    private void Awake()
    {
        if (completely)
        {
            gameObject.SetActive(!Application.isMobilePlatform);
        }

        var group = GetComponent<CanvasGroup>();

        if (!Application.isMobilePlatform)
        {
            group.interactable = true;
            group.blocksRaycasts = true;
            group.alpha = 1;
        }
        else
        {
            group.interactable = false;
            group.blocksRaycasts = false;
            group.alpha = 0;
        }
    }
}
