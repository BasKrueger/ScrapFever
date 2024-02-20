using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PoolableSound : MonoBehaviour, IPoolable
{
    private IEnumerator play()
    {
        transform.parent = null;

        var audio = GetComponent<AudioSource>();
        audio.Play();
        while (audio.isPlaying)
        {
            yield return null;
        }

        Pool.Return<PoolableSound>(this.gameObject);
    }

    #region Ipoolable
    public void OnReturnedToPool()
    {
    }

    public void OnTakenFromPool()
    {
        StopAllCoroutines();
        StartCoroutine(play());
    }
    #endregion

}
