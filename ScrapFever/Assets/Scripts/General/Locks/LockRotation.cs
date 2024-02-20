using UnityEngine;

public class LockRotation : MonoBehaviour
{
    private Quaternion rotation;
    [SerializeField]
    private bool fixedUpdate = false;
    [SerializeField]
    private bool update = false;

    private void Awake()
    {
        rotation = transform.rotation;
    }

    private void LateUpdate()
    {
        transform.rotation = rotation;
    }

    private void FixedUpdate()
    {
        if (fixedUpdate)
        {
            transform.rotation = rotation;
        }
    }

    private void Update()
    {
        if (update)
        {
            transform.rotation = rotation;
        }
    }
}
