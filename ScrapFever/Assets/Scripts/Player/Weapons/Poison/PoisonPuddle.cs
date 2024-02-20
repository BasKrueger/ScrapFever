using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonPuddle : DamageZone, IPoolable
{
    [FoldoutGroup("References"), SerializeField]
    private List<Transform> scaleWithSize = new List<Transform>();

    public void OnReturnedToPool()
    {
    }

    public void OnTakenFromPool()
    {
    }

    public void SetUp(float puddleDuration, float size, IDamageSource source)
    {
        GetComponent<CapsuleCollider>().radius = size / 2;
        foreach(var t in scaleWithSize)
        {
            t.transform.localScale += new Vector3(size, size, size);
        }

        base.overrideSource = source;
        StartCoroutine(delay(puddleDuration));
    }

    private IEnumerator delay(float duration)
    {
        yield return new WaitForSeconds(duration);
        Pool.Return<PoisonPuddle>(this.gameObject);
    }
}
