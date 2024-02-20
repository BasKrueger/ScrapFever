using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class SequenceSpawner : MonoBehaviour
{
    [SerializeField, ProgressBar(0, "GetDuration"), HideLabel]
    protected float timer = 0;

    [SerializeField, OnValueChanged("OnDurationChanged")]
    private float duration;
    [SerializeField]
    private bool loop;
    [SerializeField, ListDrawerSettings(AddCopiesLastElement = true, CustomAddFunction = "AddNewSequence")]
    private List<SpawnSequence> sequences;

    private void FixedUpdate()
    {
        if(timer > duration)
        {
            if (!loop) return;
            timer = 0;
        }

        timer += Time.fixedDeltaTime;
        foreach(var sequence in sequences)
        {
            sequence.TryFixedUpdate(timer, transform);
        }
    }

    private void OnDrawGizmos()
    {
        if (sequences == null) return;
        foreach (var sequence in sequences)
        {
            sequence.OnDrwaGizmos(transform);
        }
    }

    //Called by Odin
    private void OnDurationChanged()
    {
#if UNITY_EDITOR
        foreach(var sequence in sequences)
        {
            sequence.SetMaxDuration(duration);
        }
#endif
    }

    //Called by Odin
    private float GetDuration() => duration;

    //called by Odin
    private SpawnSequence AddNewSequence()
    {
        var sequence = new SpawnSequence();
        sequence.SetMaxDuration(duration);
        return sequence;
    }
}

[System.Serializable]
public class SpawnSequence
{
    [SerializeField, MinMaxSlider(0, "GetMaxValue", ShowFields = true)]
    private Vector2 activeDuration;
    [SerializeField, LabelText("Spawner")]
    private SpawnerLogic logic;
    [SerializeField, HideInInspector]
    private float maxDuration = 0;
    public void SetMaxDuration(float value) => maxDuration = value;

    public bool TryFixedUpdate(float time, Transform transform)
    {
        if(time > activeDuration.x && time < activeDuration.y)
        {
            logic.FixedUpdate(transform);
            return true;
        }
        return false;
    }

    public void OnDrwaGizmos(Transform transform)
    {
        logic.OnDrawGizmos(transform);
    }

    //Used by Odin
    private float GetMaxValue() => maxDuration;
}
