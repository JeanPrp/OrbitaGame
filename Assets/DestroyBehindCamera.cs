using UnityEngine;

public class DestroyBehindCamera : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float destroyMargin = 2f;

    private Camera mainCam;
    private Renderer[] childRenderers;
    private Collider2D[] childColliders;

    private void Awake()
    {
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }

        mainCam = Camera.main;
        childRenderers = GetComponentsInChildren<Renderer>();
        childColliders = GetComponentsInChildren<Collider2D>();
    }

    private void Update()
    {
        if (cameraTransform == null || mainCam == null) return;
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver()) return;

        Bounds totalBounds = GetCombinedBounds();
        float cameraBottom = cameraTransform.position.y - mainCam.orthographicSize;

        if (totalBounds.max.y < cameraBottom - destroyMargin)
        {
            Destroy(gameObject);
        }
    }

    private Bounds GetCombinedBounds()
    {
        bool hasBounds = false;
        Bounds combined = new Bounds(transform.position, Vector3.zero);

        foreach (Renderer r in childRenderers)
        {
            if (r == null || !r.enabled) continue;

            if (!hasBounds)
            {
                combined = r.bounds;
                hasBounds = true;
            }
            else
            {
                combined.Encapsulate(r.bounds);
            }
        }

        foreach (Collider2D c in childColliders)
        {
            if (c == null) continue;

            if (!hasBounds)
            {
                combined = c.bounds;
                hasBounds = true;
            }
            else
            {
                combined.Encapsulate(c.bounds);
            }
        }

        if (!hasBounds)
        {
            combined = new Bounds(transform.position, Vector3.zero);
        }

        return combined;
    }
}