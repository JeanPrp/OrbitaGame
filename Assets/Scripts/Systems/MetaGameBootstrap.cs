using UnityEngine;

public static class MetaGameBootstrap
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void EnsureMetaSystems()
    {
        if (MetaGameManager.Instance == null && Object.FindFirstObjectByType<MetaGameManager>() == null)
        {
            GameObject go = new GameObject("MetaGameManager");
            go.AddComponent<MetaGameManager>();
        }

        if (LeaderboardService.Instance == null && Object.FindFirstObjectByType<LeaderboardService>() == null)
        {
            GameObject leaderboardGo = new GameObject("LeaderboardService");
            leaderboardGo.AddComponent<LeaderboardService>();
        }
    }
}
