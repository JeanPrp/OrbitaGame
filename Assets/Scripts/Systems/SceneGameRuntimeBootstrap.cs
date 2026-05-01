using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneGameRuntimeBootstrap
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void BootstrapSceneGame()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name != "SceneGame") return;

        if (Object.FindFirstObjectByType<SceneGameAutoSetup>() != null) return;

        GameObject go = new GameObject("SceneGameAutoSetup");
        go.AddComponent<SceneGameAutoSetup>();
    }
}
