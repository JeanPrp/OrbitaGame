using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundAutoFit2D : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;
    [SerializeField] private bool coverWidth = true;
    [SerializeField] private bool coverHeight = true;
    [SerializeField] private bool keepXCentered = true;

    private SpriteRenderer spriteRenderer;
    private float lastAspect;
    private float lastOrthoSize;

    private void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (targetCamera == null) targetCamera = Camera.main;
        Refit(true);
    }

    private void LateUpdate()
    {
        Refit(false);
    }

    public void Refit(bool force)
    {
        if (spriteRenderer == null || targetCamera == null || spriteRenderer.sprite == null) return;

        float aspect = targetCamera.aspect;
        float ortho = targetCamera.orthographicSize;

        if (!force && Mathf.Abs(aspect - lastAspect) < 0.001f && Mathf.Abs(ortho - lastOrthoSize) < 0.001f)
            return;

        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;
        if (spriteSize.x <= 0f || spriteSize.y <= 0f) return;

        float worldHeight = targetCamera.orthographicSize * 2f;
        float worldWidth = worldHeight * targetCamera.aspect;

        float scaleX = worldWidth / spriteSize.x;
        float scaleY = worldHeight / spriteSize.y;

        float chosenScale = 1f;
        if (coverWidth && coverHeight) chosenScale = Mathf.Max(scaleX, scaleY);
        else if (coverWidth) chosenScale = scaleX;
        else if (coverHeight) chosenScale = scaleY;

        transform.localScale = new Vector3(chosenScale, chosenScale, transform.localScale.z);

        if (keepXCentered)
        {
            Vector3 pos = transform.position;
            pos.x = 0f;
            transform.position = pos;
        }

        lastAspect = aspect;
        lastOrthoSize = ortho;
    }
}
