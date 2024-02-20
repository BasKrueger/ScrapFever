using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class RollingStone : AbstractEnemy
{
    [SerializeField, TabGroup("Specifics")]
    private float splinterCount = 10;
    [SerializeField, TabGroup("Specifics")]
    private List<StoneSplinter> splinterTemplate;

    [SerializeField, TabGroup("Specifics")]
    private Vector2 range;
    [SerializeField, TabGroup("Specifics")]
    private Vector2 offset;

    protected override void Die()
    {
        for (int i = 0; i < splinterCount; i++)
        {
            var pos = GetRandomSplitterPosition();
            var splinter = Instantiate(splinterTemplate.GetRandom());
            splinter.transform.position = transform.position;

            splinter.Launch(transform.position, pos);
        }

        base.Die();
    }

    private Vector3 GetRandomSplitterPosition()
    {
        return new Vector3(Random.Range(-range.x / 2, range.x / 2), -0.918f, Random.Range(-range.y / 2, range.y / 2)) + new Vector3(offset.x,0,offset.y);
    }
}
