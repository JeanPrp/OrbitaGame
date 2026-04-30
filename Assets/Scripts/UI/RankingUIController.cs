using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RankingUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text rankingText;
    [SerializeField] private int maxRows = 10;

    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (rankingText == null) return;

        if (MetaGameManager.Instance == null)
        {
            rankingText.text = "Ranking indisponível.";
            return;
        }

        List<RankEntryData> rows = MetaGameManager.Instance.Profile.localRanking;

        if (rows == null || rows.Count == 0)
        {
            rankingText.text = "Sem pontuações ainda. Jogue para entrar no ranking!";
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
    }
}
