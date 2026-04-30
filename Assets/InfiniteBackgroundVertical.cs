using UnityEngine;

public class BackgroundLooper2D : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;
    [SerializeField] private SpriteRenderer tileA;
    [SerializeField] private SpriteRenderer tileB;
    [SerializeField] private float parallaxFactor = 0.2f;
    [SerializeField] private float extraOverlap = 0.15f;

    private float spriteHeight;
    private float lastCameraY;

    private void Start()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;

        if (tileA == null || tileB == null || targetCamera == null)
        {
            enabled = false;
            return;
        }

        // altura real do sprite no mundo
        spriteHeight = tileA.bounds.size.y;

        // força os dois a começarem alinhados
        Vector3 posA = tileA.transform.position;
        Vector3 posB = tileB.transform.position;

        posA.x = 0f;
        posB.x = 0f;

        posA.y = 0f;
        posB.y = posA.y + spriteHeight - extraOverlap;

        tileA.transform.position = posA;
        tileB.transform.position = posB;

        lastCameraY = targetCamera.transform.position.y;
    }

    private void LateUpdate()
    {
        if (targetCamera == null || tileA == null || tileB == null)
            return;

        float deltaY = targetCamera.transform.position.y - lastCameraY;
        lastCameraY = targetCamera.transform.position.y;

        float moveY = deltaY * parallaxFactor;

        tileA.transform.position += new Vector3(0f, moveY, 0f);
        tileB.transform.position += new Vector3(0f, moveY, 0f);

        float cameraBottom = targetCamera.transform.position.y - targetCamera.orthographicSize;

        // Se A saiu por baixo, reposiciona acima de B
        if (tileA.bounds.max.y < cameraBottom)
        {
            Vector3 pos = tileA.transform.position;
            pos.x = 0f;
            pos.y = tileB.transform.position.y + spriteHeight - extraOverlap;
            tileA.transform.position = pos;
        }

        // Se B saiu por baixo, reposiciona acima de A
        if (tileB.bounds.max.y < cameraBottom)
        {
            Vector3 pos = tileB.transform.position;
            pos.x = 0f;
            pos.y = tileA.transform.position.y + spriteHeight - extraOverlap;
            tileB.transform.position = pos;
        }
    }
}