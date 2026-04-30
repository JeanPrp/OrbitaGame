using UnityEngine;

public class OrbitTriggerZone : MonoBehaviour
{
    private CameraAutoScroll cameraController;

    private void Start()
    {
        cameraController = Camera.main.GetComponent<CameraAutoScroll>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        cameraController?.EnterOrbitMode();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        cameraController?.ExitOrbitMode();
    }
}