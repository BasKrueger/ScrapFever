using UnityEngine;

public class GameTime : MonoBehaviour
{
    private float pastTime;
    private static GameTime instance;

    public static float time
    {
        get
        {
            if(instance != null)
            {
                return instance.pastTime;
            }
            return -1;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        pastTime += Time.deltaTime;
    }
}
