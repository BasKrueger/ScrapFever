using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class EscapeSequence : MonoBehaviour
{
    [SerializeField]
    private float duration = 20;
    [SerializeField]
    private AudioMixer mixer;

    private void Start()
    {
        if (Application.isMobilePlatform)
        {
            GetComponentInChildren<VideoPlayer>().enabled = false;
        }

        mixer.SetFloat("SFXVol", 0.001f);

        StartCoroutine(delay());
    }

    private IEnumerator delay()
    {
        float cooldown = duration;
        while(cooldown > 0)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButton(0) ||
                Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Stationary || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                Time.timeScale = 5f;
            }
            else
            {
                Time.timeScale = 1;
            }

            cooldown -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        FadeScreen.FadeIn();
        FadeScreen.FadeInCompleted += () => SceneManager.LoadScene(0);
    }
}
