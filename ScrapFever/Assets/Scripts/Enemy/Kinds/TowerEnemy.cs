using Sirenix.OdinInspector;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class TowerEnemy : AbstractEnemy
{
    [SerializeField, TabGroup("Specific"), SuffixLabel("seconds",true)]
    private float delay;
    [SerializeField, TabGroup("Specific")]
    private float bonusHealth;
    [SerializeField, TabGroup("Specific")]
    private Animator animator;
    [SerializeField, TabGroup("Specific")]
    private Rigidbody body;
    [SerializeField, TabGroup("Specific")]
    private AudioSource activateSound;
    [SerializeField, TabGroup("Specific")]
    private AudioSource walkSound;

    protected override void InternalSetUp()
    {
        StartCoroutine(delayedMovement());
    }

    private IEnumerator delayedMovement()
    {
        body.isKinematic = true;

        yield return new WaitForSeconds(delay);
        
        if (Application.isPlaying)
        {
            base.hp += bonusHealth / 2;
            animator.SetTrigger("Walk");
            body.isKinematic = false;
            activateSound.Play();
            walkSound.Play();
        }
    }
}
