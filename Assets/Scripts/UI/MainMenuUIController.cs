using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject homePanel;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject customizationPanel;
    [SerializeField] private GameObject rankingPanel;

    [Header("Labels")]
    [SerializeField] private TMP_Text currencyLabel;
    [SerializeField] private TMP_Text bestScoreLabel;
    [SerializeField] private TMP_Text playerNameLabel;

    [Header("Scenes")]
    [SerializeField] private string gameplaySceneName = "SceneGame";

    private void Start()
    {
        ShowHome();
        RefreshHeader();
    }

    public void ShowHome()
    {
        SetPanels(homePanel);
    }

    public void ShowShop()
    {
        SetPanels(shopPanel);
    }

    public void ShowCustomization()
    {
        SetPanels(customizationPanel);
    }

    public void ShowRanking()
    {
        SetPanels(rankingPanel);
    }

    public void Play()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void RefreshHeader()
    {
        if (MetaGameManager.Instance == null) return;

        PlayerProfileData profile = MetaGameManager.Instance.Profile;

        if (currencyLabel != null)
            currencyLabel.text = $"Moedas: {profile.softCurrency}";

        if (bestScoreLabel != null)
            bestScoreLabel.text = $"Recorde: {profile.bestScore}";

        if (playerNameLabel != null)
            playerNameLabel.text = profile.playerName;
    }

    private void SetPanels(GameObject active)
    {
        if (homePanel != null) homePanel.SetActive(active == homePanel);
        if (shopPanel != null) shopPanel.SetActive(active == shopPanel);
        if (customizationPanel != null) customizationPanel.SetActive(active == customizationPanel);
        if (rankingPanel != null) rankingPanel.SetActive(active == rankingPanel);
    }
}
