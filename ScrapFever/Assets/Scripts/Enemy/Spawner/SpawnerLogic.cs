using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnerLogic
{
    [SerializeField,ProgressBar(0, "GetCooldown"), HideLabel]
    protected float timer = 0;

    [SerializeField, Required, AssetsOnly]
    protected AbstractEnemy EnemyTemplate;
    [SerializeField]
    private bool stopAutoDestroy = false;

    [SerializeField]
    private SpawnLocation spawnLocation;

    [SerializeField]
    protected SpawnCooldown spawnRate;

    private Color gizmosColor = Color.red;
    private List<AbstractEnemy> localSpawns = new List<AbstractEnemy>();

    public void FixedUpdate(Transform transform)
    {
        TickDown(localSpawns, transform);
    }

    private void TickDown(List<AbstractEnemy> localSpawns, Transform transform)
    {
        timer += Time.fixedDeltaTime;
        if (timer >= spawnRate.GetUpdatedCooldown(localSpawns))
        {
            Spawn(EnemyTemplate, transform);
        }
    }

    private void Spawn(AbstractEnemy template, Transform transform)
    {
        if (template == null) return;

        var inst = Pool.Get(template.gameObject);
        if (inst == null) return;

        var enemy = inst.GetComponent<AbstractEnemy>();

        enemy.transform.position = spawnLocation.GetSpawnPoint(transform);
        enemy.transform.SetParent(transform);

        timer = 0;
        spawnRate.Refresh();

        localSpawns.Add(enemy);
        enemy.died += (enemy_) => { localSpawns.Remove(enemy_); };

        if (!stopAutoDestroy)
        {
            enemy.outOfScreen += (enemy_) => { OnEnemyOutOfScreen(enemy_, transform); };
        }
    }

    private void OnEnemyOutOfScreen(AbstractEnemy enemy, Transform transform)
    {
        enemy.transform.position = spawnLocation.GetSpawnPoint(transform);
    }

    //Used by Odin
    private float GetCooldown() => spawnRate.GetUpdatedCooldown(localSpawns);

    public void OnDrawGizmos(Transform transform)
    {
        spawnLocation.OnDrawGizmos(transform);
    }
}
