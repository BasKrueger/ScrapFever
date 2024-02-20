using Sirenix.OdinInspector;
using UnityEngine;

public class LavaWave : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField, SuffixLabel("per second", true)]
    public float bonusSpeed;

    private void FixedUpdate()
    {
        transform.Translate(transform.right * speed * Time.deltaTime);
        speed += bonusSpeed * Time.deltaTime;
    }
}
