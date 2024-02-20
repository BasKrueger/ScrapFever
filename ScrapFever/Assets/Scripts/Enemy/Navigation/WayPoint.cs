using UnityEngine;

public class WayPoint : MonoBehaviour
{
    [HideInInspector]
    public WayPoint nextPoint = null;

    public Line lineToNextPoint
    {
        get
        {
            if (nextPoint == null) return new Line();

            var start = new Vector2(transform.position.x, transform.position.z);
            var end = new Vector2(nextPoint.transform.position.x, nextPoint.transform.position.z);
            return new Line(start, end);
        }
    }
}
