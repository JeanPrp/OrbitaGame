using UnityEngine;

public static class MetaGameBootstrap
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void EnsureMetaGameManager()
    {
        if (MetaGameManager.Instance != null) return;

        MetaGameManager existing = Object.FindFirstObjectByType<MetaGameManager>();
        if (existing != null) return;

        GameObject go = new GameObject("MetaGameManager");
        go.AddComponent<MetaGameManager>();
    }
}
