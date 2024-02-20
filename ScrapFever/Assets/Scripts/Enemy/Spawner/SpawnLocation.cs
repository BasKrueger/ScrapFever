using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class SpawnLocation
{
    private enum LocationPlace
    {
        World,
        Screen
    }

    private enum ScreenEdge
    {
        top,
        right, 
        bottom, 
        left,
        random
    }

    [SerializeField]
    private float bonusSpawnHeight = 0;
    [SerializeField]
    private LocationPlace place = LocationPlace.Screen;
    [SerializeField, ShowIf("ShowWorldPosition")]
    private Vector2 spawnArea = new Vector2(5, 5);
    [SerializeField, ShowIf("ShowWorldPosition")]
    private Vector2 spawnOffset;
    [SerializeField, ShowIf("ShowWorldPosition")]
    private bool areaEdges = true;

    public Vector3 GetSpawnPoint(Transform transform)
    {
        switch (place)
        {
            case LocationPlace.World:
                if (areaEdges)
                {
                    return GetRandomWorldAreaLocation(transform) + new Vector3(0,bonusSpawnHeight);
                }
                return GetWorldLocation(transform) + new Vector3(0, bonusSpawnHeight);
            case LocationPlace.Screen:
                return GetScreenLocation(transform) + new Vector3(0, bonusSpawnHeight);
        }

        return new Vector3();
    }

    private Vector3 GetWorldLocation(Transform transform)
    {
        var result = new Vector3();

        result += transform.position + spawnOffset.ToVector3();
        result.x += Random.Range(spawnArea.x / 2 * -1, spawnArea.x / 2);
        result.z += Random.Range(spawnArea.y / 2 * -1, spawnArea.y / 2);

        return result;
    }

    private Vector3 GetRandomWorldAreaLocation(Transform transform)
    {
        var topLeft = transform.position + spawnOffset.ToVector3() + new Vector3(-spawnArea.x / 2,0,spawnArea.y / 2);
        var bottomRight = transform.position + spawnOffset.ToVector3() + new Vector3(spawnArea.x / 2, 0, -spawnArea.y / 2);

        switch ((ScreenEdge)Random.Range(0,4))
        {
            case ScreenEdge.top:
                return new Vector3(Random.Range(topLeft.x, bottomRight.x),
                    transform.position.y,
                    topLeft.z);

            case ScreenEdge.bottom:
                return new Vector3(Random.Range(topLeft.x, bottomRight.x),
                    transform.position.y,
                    bottomRight.z);
            case ScreenEdge.right:
                return new Vector3(bottomRight.x,
                    transform.position.y,
                    Random.Range(bottomRight.z, topLeft.z));
            case ScreenEdge.left:
                return new Vector3(topLeft.x,
                    transform.position.y,
                    Random.Range(bottomRight.z, topLeft.z));
        }

        return new Vector3();
    }

    private Vector3 GetScreenLocation(Transform transform)
    {
        var plane = new Plane(Vector3.down, transform.position.y);

        var randomEdge = (ScreenEdge)Random.Range(0, 4);
        var ray = Camera.main.ScreenPointToRay(GetScreenPosition(randomEdge));

        if (plane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }

        return new Vector3();
    }

    private Vector3 GetScreenPosition(ScreenEdge edge)
    {
        var topLeft = new Vector3(0, 0, 0);
        var topRight = new Vector3(Screen.width, 0, 0);
        var bottomLeft = new Vector3(0, Screen.height, 0);
        var bottomRight = new Vector3(Screen.width, Screen.height, 0);

        switch (edge)
        {
            case ScreenEdge.top:
                return new Vector3(Random.Range(0, Screen.width),
                    0, 
                    0);

            case ScreenEdge.bottom:
                return new Vector3(Random.Range(0, Screen.width),
                    Screen.height,
                    0);
            case ScreenEdge.right:
                return new Vector3(Screen.width,
                    Random.Range(0, Screen.height),
                    0);
            case ScreenEdge.left:
                return new Vector3(0,
                    Random.Range(0, Screen.height),
                    0);
        }

        return new Vector3();
    }

    private Vector3 GetRandomPointBetween(Vector3 a, Vector3 b)
    {
        var dist = b - a;
        dist *= Random.Range(0f, 1f);

        return dist;
    }

    public void OnDrawGizmos(Transform transform)
    {
        if (place != LocationPlace.World) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + spawnOffset.ToVector3(), new Vector3(spawnArea.x, 0, spawnArea.y));
    }

    //used by odin
    private bool ShowWorldPosition => place == LocationPlace.World;
}
