using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DamageZone : MonoBehaviour, IDamageSource
{
    [Title("DamageZone")]

    public event Action<int, IDamageAble> damageDealt;

    [SerializeField]
    public int damage = 1;

    [SerializeField]
    private bool collisionEnter;
    [SerializeField]
    private bool collisionExit;

    [SerializeField]
    private bool stay = true;
    [SerializeField, SuffixLabel("seconds", true), ShowIf("stay")]
    public float cooldown = 0.1f;

    private List<IDamageAble> inRange = new List<IDamageAble>();
    public IDamageSource overrideSource = null;
    string IDamageSource.glueSourceName => "GlueSourceName";

    public void ClearTargets() => inRange = new List<IDamageAble>();

    private void OnTriggerEnter(Collider other)
    {
        EnterTarget(other.transform.GetComponent<IDamageAble>());
    }

    private void OnTriggerExit(Collider other)
    {
        ExitTarget(other.transform.GetComponent<IDamageAble>());
    }

    private void OnCollisionEnter(Collision collision)
    {
        EnterTarget(collision.transform.GetComponent<IDamageAble>());
    }

    private void OnCollisionExit(Collision collision)
    {
        ExitTarget(collision.transform.GetComponent<IDamageAble>());
    }

    private void ExitTarget(IDamageAble target)
    {
        if (target == null || target.gameObject.layer == this.gameObject.layer) return;

        inRange.Remove(target);

        if (!collisionExit) return;
        Hit(target);
    }

    private void EnterTarget(IDamageAble target)
    {
        if (target == null || target.gameObject.layer == this.gameObject.layer) return;
        
        if (stay)
        {
            StartCoroutine(DamageWhileInRange(target));
        }

        if (!collisionEnter) return;
        Hit(target);
    }

    protected virtual void Hit(IDamageAble target)
    {
        if (target != null)
        {
            if(overrideSource != null)
            {
                target.TakeDamage(damage, overrideSource);
            }
            else
            {
                target.TakeDamage(damage, this);
            }

            damageDealt?.Invoke(damage, target);
        }
    }

    private IEnumerator DamageWhileInRange(IDamageAble target)
    {
        inRange.Add(target);

        while (inRange.Contains(target) && this.enabled && this.gameObject.activeInHierarchy)
        {
            yield return new WaitForSeconds(cooldown);

            if (target == null || target.gameObject == null || !target.gameObject.activeInHierarchy)
            {
                inRange.Remove(target);
                yield break;
            }

            Hit(target);
        }
    }
}
