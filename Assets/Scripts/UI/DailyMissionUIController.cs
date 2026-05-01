using TMPro;
using UnityEngine;

public class DailyMissionUIController : MonoBehaviour
{
    [SerializeField] private MainMenuUIController mainMenuUI;
    [SerializeField] private TMP_Text missionText;
    [SerializeField] private TMP_Text claimResultText;

    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (MetaGameManager.Instance == null || missionText == null)
        {
            if (missionText != null) missionText.text = "Missão indisponível.";
            return;
        }

        PlayerProfileData profile = MetaGameManager.Instance.Profile;
        DailyMissionConfig cfg = DailyMissionService.GetConfig();

        if (cfg == null)
        {
            missionText.text = "Sem configuração de missão diária.";
            return;
        }

        DailyMissionService.EnsureDailyState(profile);

        string status = profile.missionClaimed ? "(Concluída)" : "";
        missionText.text = $"Missão diária: faça {cfg.TargetScore} pontos\nProgresso: {profile.missionProgressScore}/{cfg.TargetScore} {status}";
    }

    public void Claim()
    {
        if (MetaGameManager.Instance == null) return;

        bool success = MetaGameManager.Instance.TryClaimDailyMissionReward(out int reward);
        if (claimResultText != null)
            claimResultText.text = success ? $"Recompensa recebida: +{reward} moedas" : "Missão não concluída ou já resgatada.";

        mainMenuUI?.RefreshHeader();
        Refresh();
    }
}
