using System.Collections;
using UnityEngine;

public class StoneSplinter : MonoBehaviour
{
    private const float launchHeight = 5f;

    [SerializeField]
    private GameObject puddleTemplate;
    [SerializeField]
    private float speed;

    public void Launch(Vector3 from, Vector3 target) => StartCoroutine(Launching(from, target));

    private IEnumerator Launching(Vector3 from,Vector3 targetPosition)
    {
        Vector3 p0 = from;
        Vector3 p3 = targetPosition;

        transform.LookAt(p3);

        float time = Vector3.Distance(p3, p0) * (1 / speed);
        float timer = 0;
        float percent = timer / time;

        while (timer < time)
        {
            timer += Time.deltaTime;
            percent = timer / time;

            Vector3 p1 = p0 + (p3 - p0) * 0.333f + Vector3.up * launchHeight;
            Vector3 p2 = p0 + (p3 - p0) * 0.666f + Vector3.up * launchHeight;

            transform.position = AdvancedMath.CubeBezier3(p0, p1, p2, p3, timer / time);

            yield return new WaitForEndOfFrame();
        }

        var puddle = Instantiate(puddleTemplate);
        puddle.transform.position = targetPosition + new Vector3(0,Random.Range(0, 0.1f),0);

        Destroy(this.gameObject);
    }
}
