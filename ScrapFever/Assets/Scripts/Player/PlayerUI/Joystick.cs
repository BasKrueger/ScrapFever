using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Joystick : MonoBehaviour
{
    [SerializeField]
    private Image inner;

    private RectTransform rect;
    private CanvasGroup group;

    float maxDistance = 0;

    private PlayerInputActions input;

    private void Awake()
    {
        input = new PlayerInputActions();
        input.Enable();
        input.Player.Enable();
        input.Player.MousePosition.Enable();

        rect = GetComponent<RectTransform>();
        group = GetComponent<CanvasGroup>(); 
        maxDistance = rect.rect.width / 2;
    }

    private void Start()
    {
        StartCoroutine(UpdateLoop());
    }

    public void Show()
    {
        group.alpha = 1;
    }

    public void Hide()
    {
        group.alpha = 0;
        inner.transform.position = transform.position;
    }

    public Vector2 GetDirection()
    {
        var normal = (inner.transform.position - transform.position).normalized;
        var percent = Mathf.Clamp(Vector2.Distance(inner.transform.position, rect.position) / maxDistance,0,1);

        return normal * percent;
    }

    private IEnumerator UpdateLoop()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();

            if (Time.timeScale == 0 || Input.touchCount == 0)
            {
                Hide();
                continue;
            }

            TryShow();
            TryUpdateInput();
            TryHide();
        }
    }

    private bool TryShow()
    {
        if (Input.GetTouch(0).phase != TouchPhase.Began) return false;

        Show();
        transform.position = Input.GetTouch(0).position;
        return true;
    }

    private bool TryUpdateInput()
    {
        var pos = input.Player.MousePosition.ReadValue<Vector2>();
        if (pos == new Vector2()) return false;

        if (Input.GetTouch(0).phase != TouchPhase.Moved) return false;

        var dist = Vector2.Distance(pos, rect.position);

        if(dist > maxDistance)
        {
            Vector3 dir = (pos - (Vector2)transform.position).normalized;
            pos = rect.position + dir * maxDistance;
        }

        inner.transform.position = pos;

        return true;
    }

    private bool TryHide()
    {
        if (Input.GetTouch(0).phase != TouchPhase.Ended) return false;

        Hide();
        return true;
    }
}
