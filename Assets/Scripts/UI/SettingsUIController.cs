using TMPro;
using UnityEngine;

public class SettingsUIController : MonoBehaviour
{
    [SerializeField] private TMP_InputField playerNameInput;
    [SerializeField] private MainMenuUIController mainMenuUI;
    [SerializeField] private TMP_Text feedbackLabel;

    private void OnEnable()
    {
        if (MetaGameManager.Instance == null || playerNameInput == null) return;

        playerNameInput.text = MetaGameManager.Instance.Profile.playerName;
    }

    public void SavePlayerName()
    {
        if (MetaGameManager.Instance == null || playerNameInput == null) return;

        string candidate = playerNameInput.text?.Trim();
        if (string.IsNullOrWhiteSpace(candidate))
        {
            SetFeedback("Nome inválido.");
            return;
        }

        MetaGameManager.Instance.SetPlayerName(candidate);
        mainMenuUI?.RefreshHeader();
        SetFeedback("Nome salvo com sucesso!");
    }

    private void SetFeedback(string text)
    {
        if (feedbackLabel != null)
            feedbackLabel.text = text;
    }
}
