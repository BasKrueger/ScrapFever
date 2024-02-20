using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

public class InstantSpawner : MonoBehaviour
{
    [SerializeField, AssetsOnly]
    private AbstractEnemy enemyTemplate;
    [SerializeField]
    private SpawnLocation spawnLocation;

    [Header("Settings")]
    [SerializeField]
    private int count;
    [SerializeField, SuffixLabel("seconds", true)]
    private float delay;
    [SerializeField]
    private bool preventAutoDestroy;

    private SpawnerLogic logic;

    private void Awake()
    {
        logic = new SpawnerLogic();
    }

    private void Start()
    {
        StartCoroutine(delayedSpawn());
    }

    private IEnumerator delayedSpawn()
    {
        yield return new WaitForSeconds(delay);

        for (int i = 0;i < count; i++)
        {
            if(i % 5 == 0)
            {
                yield return new WaitForEndOfFrame();
            }

            Spawn();
        }
    }

    private void Spawn()
    {
        if (enemyTemplate == null) return;

        var enemy = Pool.Get<AbstractEnemy>(enemyTemplate.gameObject.name);
        if (enemy == null) return;

        enemy.transform.position = spawnLocation.GetSpawnPoint(transform);
        enemy.transform.SetParent(transform);

        if (preventAutoDestroy)
        {
            enemy.StopAllCoroutines();
        }
    }

    private void OnDrawGizmos()
    {
        spawnLocation.OnDrawGizmos(transform);
    }
}
