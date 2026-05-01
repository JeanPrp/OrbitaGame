using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LeaderboardService : MonoBehaviour
{
    public static LeaderboardService Instance { get; private set; }

    [Header("Remote leaderboard")]
    [SerializeField] private bool useRemoteLeaderboard = false;
    [SerializeField] private string fetchUrl = "";
    [SerializeField] private float timeoutSeconds = 5f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void FetchTopScores(int maxRows, Action<List<RankEntryData>, bool> onComplete)
    {
        if (useRemoteLeaderboard && !string.IsNullOrWhiteSpace(fetchUrl))
        {
            StartCoroutine(FetchRemote(maxRows, onComplete));
            return;
        }

        onComplete?.Invoke(GetLocalScores(maxRows), false);
    }

    private List<RankEntryData> GetLocalScores(int maxRows)
    {
        List<RankEntryData> rows = new List<RankEntryData>();

        if (MetaGameManager.Instance != null && MetaGameManager.Instance.Profile.localRanking != null)
        {
            rows.AddRange(MetaGameManager.Instance.Profile.localRanking);
        }

        if (rows.Count > maxRows)
            rows = rows.GetRange(0, maxRows);

        return rows;
    }

    private IEnumerator FetchRemote(int maxRows, Action<List<RankEntryData>, bool> onComplete)
    {
        string url = fetchUrl.Contains("?")
            ? $"{fetchUrl}&limit={maxRows}"
            : $"{fetchUrl}?limit={maxRows}";

        using UnityWebRequest request = UnityWebRequest.Get(url);
        request.timeout = Mathf.CeilToInt(timeoutSeconds);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            onComplete?.Invoke(GetLocalScores(maxRows), false);
            yield break;
        }

        try
        {
            LeaderboardResponse parsed = JsonUtility.FromJson<LeaderboardResponse>(request.downloadHandler.text);
            if (parsed == null || parsed.entries == null || parsed.entries.Length == 0)
            {
                onComplete?.Invoke(GetLocalScores(maxRows), false);
                yield break;
            }

            List<RankEntryData> rows = new List<RankEntryData>(parsed.entries.Length);
            foreach (LeaderboardEntryDto dto in parsed.entries)
            {
                rows.Add(new RankEntryData
                {
                    playerName = dto.playerName,
                    score = dto.score,
                    createdAtIsoUtc = dto.createdAtIsoUtc
                });
            }

            onComplete?.Invoke(rows, true);
        }
        catch
        {
            onComplete?.Invoke(GetLocalScores(maxRows), false);
        }
    }

    [Serializable]
    private class LeaderboardResponse
    {
        public LeaderboardEntryDto[] entries;
    }

    [Serializable]
    private class LeaderboardEntryDto
    {
        public string playerName;
        public int score;
        public string createdAtIsoUtc;
    }
}
