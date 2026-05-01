using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text rewardText;
    [SerializeField] private string menuSceneName = "SceneMenu";

    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (GameManager.Instance == null) return;

        if (scoreText != null)
            scoreText.text = $"Score: {GameManager.Instance.LastRunScore}";

        if (rewardText != null)
            rewardText.text = $"Moedas ganhas: +{GameManager.Instance.LastRunCurrencyReward}";
    }

    public void Restart()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.RestartGame();
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }
}
