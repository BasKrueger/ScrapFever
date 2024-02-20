using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI display;

    private float timer = 0;

    private static Timer instance;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        StartCoroutine(UpdateLoop());
    }

    public string GetSurvivedTimeFormated()
    {
        var ts = TimeSpan.FromSeconds(timer);
        return string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);
    }

    private IEnumerator UpdateLoop()
    {
        while (true)
        {
            timer += 1;

            var ts = TimeSpan.FromSeconds(timer);
            display.text = string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);

            AchivementManager.ProgressAchivement<TimeAchievement>(1);
            yield return new WaitForSeconds(1);
        }
    }
}
