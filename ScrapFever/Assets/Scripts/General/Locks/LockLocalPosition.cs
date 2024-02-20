using UnityEngine;

public class LockLocalPosition : MonoBehaviour
{
    private Vector3 pos;

    private void Awake()
    {
        pos = transform.localPosition;
    }

    private void LateUpdate()
    {
        transform.localPosition = pos;
    }
}
