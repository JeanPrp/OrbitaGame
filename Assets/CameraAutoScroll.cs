using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraAutoScroll : MonoBehaviour
{
    [Header("Modo normal")]
    [SerializeField] private Transform target;
    [SerializeField] private float normalFollowXStrength = 0.35f;
    [SerializeField] private float normalFollowXLerp = 4f;
    [SerializeField] private float normalMinX = -0.8f;
    [SerializeField] private float normalMaxX = 0.8f;
    [SerializeField] private Vector3 normalOffset = new Vector3(0f, 0f, -10f);

    [Header("Modo órbita")]
    [SerializeField] private float orbitFollowLerp = 8f;
    [SerializeField] private Vector3 orbitOffset = new Vector3(0f, 0f, -10f);

    [Header("Zoom")]
    [SerializeField] private float normalZoom = 5f;
    [SerializeField] private float orbitZoom = 6.4f;
    [SerializeField] private float zoomLerp = 5f;

    private bool isOrbitMode = false;
    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = normalZoom;
    }

    private void LateUpdate()
    {
        if (target == null) return;
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver()) return;

        if (isOrbitMode)
        {
            Vector3 desired = new Vector3(
                target.position.x + orbitOffset.x,
                target.position.y + orbitOffset.y,
                orbitOffset.z
            );

            transform.position = Vector3.Lerp(
                transform.position,
                desired,
                orbitFollowLerp * Time.deltaTime
            );

            cam.orthographicSize = Mathf.Lerp(
                cam.orthographicSize,
                orbitZoom,
                zoomLerp * Time.deltaTime
            );

            return;
        }

        Vector3 pos = transform.position;

        float scrollSpeed = 2.0f;
        if (GameManager.Instance != null)
        {
            scrollSpeed = GameManager.Instance.GetCurrentScrollSpeed();
        }

        pos.y += scrollSpeed * Time.deltaTime;

        float desiredX = target.position.x * normalFollowXStrength;
        desiredX = Mathf.Clamp(desiredX, normalMinX, normalMaxX);
        pos.x = Mathf.Lerp(pos.x, desiredX, normalFollowXLerp * Time.deltaTime);

        pos.z = normalOffset.z;
        transform.position = pos;

        cam.orthographicSize = Mathf.Lerp(
            cam.orthographicSize,
            normalZoom,
            zoomLerp * Time.deltaTime
        );
    }

    public void EnterOrbitMode()
    {
        isOrbitMode = true;
    }

    public void ExitOrbitMode()
    {
        isOrbitMode = false;
    }

    public bool IsOrbitMode()
    {
        return isOrbitMode;
    }
}