using UnityEngine;

public class LockLocalRotation : MonoBehaviour
{
    private Quaternion rotation;

    private void Awake()
    {
        rotation = transform.localRotation;
    }

    private void LateUpdate()
    {
        transform.localRotation = rotation;
    }
}
