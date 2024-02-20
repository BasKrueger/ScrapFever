using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class PoolableVFX : MonoBehaviour, IPoolable
{

    private AudioSource cachedAudio = null;
    private AudioSource audioPlayer
    {
        get
        {
            if (cachedAudio == null)
            {
                cachedAudio = GetComponentInChildren<AudioSource>();
            }
            return cachedAudio;
        }
    }

    private ParticleSystem cachedParticlePlayer;
    private ParticleSystem particlePlayer
    {
        get
        {
            if(cachedParticlePlayer == null)
            {
                cachedParticlePlayer = GetComponentInChildren<ParticleSystem>();
            }
            return cachedParticlePlayer;
        }
    }

    public void OnReturnedToPool()
    {
        particlePlayer.Stop();
        audioPlayer.Stop();
    }

    public void OnTakenFromPool()
    {
        particlePlayer.Play();
        audioPlayer.Play();

        StartCoroutine(ReturnLoop());
    }

    private IEnumerator ReturnLoop()
    {
        while (particlePlayer != null && particlePlayer.isPlaying)
        {
            yield return new WaitForSeconds(0.05f);
        }

        Pool.Return<PoolableVFX>(this.gameObject);
    }
}
