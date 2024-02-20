using TMPro;
using UnityEngine;

public class KillCountDisplay : MonoBehaviour
{
    private TextMeshProUGUI text;
    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        text.text = Pool.GetReturnedToPoolCount<AbstractEnemy>().ToString();
    }
}
