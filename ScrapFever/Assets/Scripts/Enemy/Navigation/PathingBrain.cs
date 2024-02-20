using System.Collections.Generic;
using UnityEngine;

public class MoveInformations
{
    public Line toCross;
    public Vector2 direction;
    public bool startedLeft;
}

public static class PathingBrain
{
    private static List<EnemyPath> paths = new List<EnemyPath>();

    public static void Add(EnemyPath path) => paths.Add(path);
    public static void Remove(EnemyPath path) => paths.Remove(path);

    private static WayPoint GetStartingWayPoint(Vector3 from)
    {
        WayPoint wayPoint = null;

        foreach (var path in paths)
        {
            var point = path.GetClosestWayPointTo(from);

            if (point.nextPoint == null) continue;
            if (wayPoint == null ||
               Vector3.Distance(from, point.transform.position) <
               Vector3.Distance(from, wayPoint.transform.position))
            {
                wayPoint = point;
            }
        }

        return wayPoint;
    }

    public static List<MoveInformations> GetNextPath(Vector3 position)
    {
        var wayPoint = GetStartingWayPoint(position);

        if (wayPoint == null || wayPoint.nextPoint == null) return new List<MoveInformations>();

        var result = new List<MoveInformations>();

        var distance = wayPoint.lineToNextPoint.GetSignedDistance(new Vector2(position.x, position.z));
        while (wayPoint.nextPoint != null && wayPoint.nextPoint.nextPoint != null)
        {
            //get paralel to next point
            var nextParalel = wayPoint.lineToNextPoint.GetParalel(distance);

            //get paralel to next next point
            var nextNextParalel = wayPoint.nextPoint.lineToNextPoint.GetParalel(distance);

            //get point where the paralels intersect 
            var intersection = nextParalel.GetIntersectionWith(nextNextParalel);

            var info = new MoveInformations();
            info.toCross = new Line(intersection, wayPoint.lineToNextPoint.end);
            info.direction = (wayPoint.lineToNextPoint.end - wayPoint.lineToNextPoint.start).normalized;

            Debug.DrawLine(info.toCross.start.ToVector3(), info.toCross.end.ToVector3(), Color.yellow, 1000);

            result.Add(info);

            wayPoint = wayPoint.nextPoint;
        }

        var lastInfo = new MoveInformations();
        lastInfo.toCross = new Line(wayPoint.lineToNextPoint.GetPerpendicular());
        lastInfo.direction = (wayPoint.lineToNextPoint.end - wayPoint.lineToNextPoint.start).normalized;
        Debug.DrawLine(lastInfo.toCross.start.ToVector3(), lastInfo.toCross.end.ToVector3(), Color.yellow, 1000);

        result.Add(lastInfo);

        return result;
    }
}
