using System.Collections.Generic;
using UnityEngine;

public class EnemyPath : MonoBehaviour
{
    private List<WayPoint> wayPoints;

    private Color gizmosColor = Color.red;

    private void Awake()
    {
        wayPoints = new List<WayPoint>();
        foreach(var point in GetComponentsInChildren<WayPoint>())
        {
            wayPoints.Add(point);
        }

        ConnectWayPoints(wayPoints);
    }

    public void Activate()
    {
        PathingBrain.Add(this);
        gizmosColor = Color.green;
    }

    public void DeActivate()
    {
        PathingBrain.Remove(this);
        gizmosColor = Color.red;
    }

    public WayPoint GetClosestWayPointTo(Vector3 position)
    {
        WayPoint result = null;

        foreach(var point in wayPoints)
        {
            if(result == null || Vector3.Distance(point.transform.position, position) < Vector3.Distance(result.transform.position, position))
            {
                result = point;
            }
        }

        return result;
    }

    private void ConnectWayPoints(List<WayPoint> points)
    {
        WayPoint lastPoint = null;
        foreach (var point in points)
        {
            if (lastPoint != null)
            {
                lastPoint.nextPoint = point;
            }

            lastPoint = point;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmosColor;

        var lastPoint = new Vector3();
        foreach(var point in GetComponentsInChildren<WayPoint>())
        {
            var nextPoint = point.transform.position;

            if(lastPoint != new Vector3())
            {
                Gizmos.DrawLine(lastPoint, nextPoint);
            }

            lastPoint = nextPoint;
        }
    }
}
