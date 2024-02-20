using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class EnemyNavigation
{
    [SerializeField]
    private FloatStat speed;

    private List<MoveInformations> path = null;
    private Transform overrideTarget;

    private Vector3 movementDirection = new Vector3();

    public void SetTarget(Transform t)
    {
        overrideTarget = t;
    }

    public void SetUp(Vector3 from)
    {
        overrideTarget = null;
        movementDirection = new Vector3();
        speed.Reset();

        path = PathingBrain.GetNextPath(from);

        if (path.Count > 0)
        {
            path[0].startedLeft = path.First().toCross.IsLeftFrom(from);
        }
    }

    public Vector3 GetMovementDirection()
    {
        return movementDirection;
    }

    public void Move(Rigidbody rb)
    {
        rb.velocity = movementDirection.normalized * speed;
    }

    public void FixedUpdate(Vector3 from)
    {
        UpdateMovementDirection(from);

        if (path == null) return;
        if (path.Count == 0) return;

        var info = path.First();
        if (info.startedLeft && info.toCross.IsRightFrom(from) ||
            !info.startedLeft && info.toCross.IsLeftFrom(from))
        {
            path.Remove(path.First());

            if (path.Count > 0)
            {
                path[0].startedLeft = path.First().toCross.IsLeftFrom(from);
            }
        }
    }

    private void UpdateMovementDirection(Vector3 from)
    {
        if (overrideTarget != null)
        {
            movementDirection = (overrideTarget.position - from);
            return;
        }
        else if (path != null && path.Count > 0)
        {
            var dir = path.First().direction;
            movementDirection = path.First().direction.ToVector3();
            return;
        }

        movementDirection = (new Vector3(movementDirection.x, 0, movementDirection.z)).normalized;
    }
}
