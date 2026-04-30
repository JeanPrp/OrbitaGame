using UnityEngine;
using UnityEngine.EventSystems;

public class FixedJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform handle;
    [SerializeField] private float handleRange = 45f;
    [SerializeField] private float deadZone = 0.08f;

    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 inputVector;

    public float Horizontal => inputVector.x;
    public float Vertical => inputVector.y;
    public Vector2 Direction => inputVector;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            eventData.position,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out localPoint
        );

        localPoint.x /= rectTransform.sizeDelta.x * 0.5f;
        localPoint.y /= rectTransform.sizeDelta.y * 0.5f;

        inputVector = new Vector2(localPoint.x, localPoint.y);
        inputVector = Vector2.ClampMagnitude(inputVector, 1f);

        if (inputVector.magnitude < deadZone)
        {
            inputVector = Vector2.zero;
        }

        if (handle != null)
            handle.anchoredPosition = inputVector * handleRange;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;

        if (handle != null)
            handle.anchoredPosition = Vector2.zero;
    }
}