using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RankingUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text rankingText;
    [SerializeField] private TMP_Text sourceText;
    [SerializeField] private int maxRows = 10;

    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (rankingText == null) return;

        if (LeaderboardService.Instance == null)
        {
            rankingText.text = "Ranking indisponível.";
            return;
        }

        rankingText.text = "Carregando ranking...";

        LeaderboardService.Instance.FetchTopScores(maxRows, OnRankingLoaded);
    }

    private void OnRankingLoaded(List<RankEntryData> rows, bool fromRemote)
    {
        if (rows == null || rows.Count == 0)
        {
            rankingText.text = "Sem pontuações ainda. Jogue para entrar no ranking!";
            if (sourceText != null) sourceText.text = "Fonte: local";
            return;
        }

        int limit = Mathf.Min(maxRows, rows.Count);
        System.Text.StringBuilder builder = new System.Text.StringBuilder();

        for (int i = 0; i < limit; i++)
        {
            RankEntryData row = rows[i];
            builder.AppendLine($"{i + 1}. {row.playerName} - {row.score}");
        }

        rankingText.text = builder.ToString();

        if (sourceText != null)
            sourceText.text = fromRemote ? "Fonte: online" : "Fonte: local";
    }
}
