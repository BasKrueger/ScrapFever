using UnityEngine;

public class BasicSpawner : MonoBehaviour
{
    [SerializeField]
    private SpawnerLogic logic;

    private void FixedUpdate()
    {
        logic.FixedUpdate(transform);
    }

    private void OnDrawGizmos()
    {
        logic.OnDrawGizmos(transform);
    }
}
