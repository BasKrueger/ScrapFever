using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonForwarder : MonoBehaviour
{
    [SerializeField]
    private Button other;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClicked);
    }

    void OnClicked()
    {
        other.onClick.Invoke();
    }
}
