using UnityEngine;

public class SceneGameAutoSetup : MonoBehaviour
{
    [Header("Auto-setup")]
    [SerializeField] private bool setupCameraAdapter = true;
    [SerializeField] private bool setupBackgroundFitters = true;
    [SerializeField] private bool setupPlayerVisualApplier = true;

    private void Awake()
    {
        if (setupCameraAdapter) EnsureCameraAdapter();
        if (setupBackgroundFitters) EnsureBackgroundFitters();
        if (setupPlayerVisualApplier) EnsurePlayerVisualApplier();
    }

    private void EnsureCameraAdapter()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        if (cam.GetComponent<AspectRatioCameraAdapter>() == null)
            cam.gameObject.AddComponent<AspectRatioCameraAdapter>();
    }

    private void EnsureBackgroundFitters()
    {
        GameObject backgroundRoot = GameObject.Find("BackgroundRoot");
        if (backgroundRoot == null) return;

        SpriteRenderer[] renderers = backgroundRoot.GetComponentsInChildren<SpriteRenderer>(true);
        for (int i = 0; i < renderers.Length; i++)
        {
            SpriteRenderer sr = renderers[i];
            if (sr == null || sr.sprite == null) continue;

            if (sr.GetComponent<BackgroundAutoFit2D>() == null)
                sr.gameObject.AddComponent<BackgroundAutoFit2D>();
        }
    }

    private void EnsurePlayerVisualApplier()
    {
        GameObject shipVisual = GameObject.Find("ShipVisual");
        if (shipVisual == null) return;

        if (shipVisual.GetComponent<PlayerShipVisualApplier>() == null)
            shipVisual.AddComponent<PlayerShipVisualApplier>();
    }
}
