using UnityEngine;

[RequireComponent(typeof(Camera))]
public class AspectRatioCameraAdapter : MonoBehaviour
{
    [Header("Base framing")]
    [SerializeField] private float referenceAspect = 9f / 16f;
    [SerializeField] private float referenceOrthoSize = 5f;

    [Header("Limits")]
    [SerializeField] private float minOrthoSize = 4.5f;
    [SerializeField] private float maxOrthoSize = 8.5f;

    [Header("Runtime")]
    [SerializeField] private bool updateContinuously = true;

    private Camera cam;
    private float lastAspect;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        ApplyNow(true);
    }

    private void LateUpdate()
    {
        if (!updateContinuously) return;

        float aspect = (float)Screen.width / Screen.height;
        if (Mathf.Abs(aspect - lastAspect) > 0.001f)
            ApplyNow(false);
    }

    public void ApplyNow(bool force)
    {
        if (cam == null) cam = GetComponent<Camera>();

        float aspect = (float)Screen.width / Screen.height;
        if (!force && Mathf.Abs(aspect - lastAspect) <= 0.001f) return;

        // Mantém a largura de referência para não "abrir" demais em tablets.
        float targetSize = referenceOrthoSize * (referenceAspect / aspect);
        targetSize = Mathf.Clamp(targetSize, minOrthoSize, maxOrthoSize);

        cam.orthographicSize = targetSize;
        lastAspect = aspect;
    }
}
