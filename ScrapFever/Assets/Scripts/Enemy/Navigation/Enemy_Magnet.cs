using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class EnemyMagnet : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<SphereCollider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponent<AbstractEnemy>();
        enemy?.SetTargetTransform(transform);
    }
}
