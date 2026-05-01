using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI")]
    [SerializeField] private GameObject gameOverPanel;

    [Header("Velocidade do jogo")]
    [SerializeField] private float startScrollSpeed = 1.8f;
    [SerializeField] private float maxScrollSpeed = 6.0f;
    [SerializeField] private float speedIncreasePerSecond = 0.08f;

    [Header("Progressão")]
    [SerializeField] private string defaultPlayerName = "Player";
    [SerializeField] private float scorePerSecondFallback = 10f;
    [SerializeField] private float currencyPerSecondFallback = 2.5f;

    private float elapsedTime = 0f;
    private bool isGameOver = false;
    public int LastRunScore { get; private set; }
    public int LastRunCurrencyReward { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void EnsureInstanceExists()
    {
        if (Instance != null) return;

        GameManager existing = FindFirstObjectByType<GameManager>();
        if (existing != null)
        {
            Instance = existing;
            return;
        }

        GameObject bootstrap = new GameObject("GameManager_Auto");
        Instance = bootstrap.AddComponent<GameManager>();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void EnsureInstanceExists()
    {
        if (Instance != null) return;

        GameManager existing = FindFirstObjectByType<GameManager>();
        if (existing != null)
        {
            Instance = existing;
            return;
        }

        GameObject bootstrap = new GameObject("GameManager_Auto");
        Instance = bootstrap.AddComponent<GameManager>();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (gameOverPanel == null)
        {
            GameObject fallbackPanel = GameObject.Find("GameOverPanel");
            if (fallbackPanel != null)
                gameOverPanel = fallbackPanel;
        }

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    private void Update()
    {
        if (isGameOver) return;
        elapsedTime += Time.deltaTime;
    }

    public float GetCurrentScrollSpeed()
    {
        float speed = startScrollSpeed + (elapsedTime * speedIncreasePerSecond);
        return Mathf.Min(speed, maxScrollSpeed);
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    public int GetCurrentScore()
    {
        EconomyConfig cfg = EconomyConfigService.Get();
        float scorePerSecond = cfg != null ? cfg.ScorePerSecond : scorePerSecondFallback;
        return Mathf.Max(0, Mathf.RoundToInt(elapsedTime * scorePerSecond));
    }

    public int GetRunCurrencyReward()
    {
        EconomyConfig cfg = EconomyConfigService.Get();
        float currencyPerSecond = cfg != null ? cfg.CurrencyPerSecond : currencyPerSecondFallback;
        return Mathf.Max(0, Mathf.RoundToInt(elapsedTime * currencyPerSecond));
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;

        int runScore = GetCurrentScore();
        int reward = GetRunCurrencyReward();

        LastRunScore = runScore;
        LastRunCurrencyReward = reward;

        if (MetaGameManager.Instance != null)
        {
            PlayerProfileData profile = MetaGameManager.Instance.Profile;
            string playerName = profile != null && !string.IsNullOrWhiteSpace(profile.playerName)
                ? profile.playerName
                : defaultPlayerName;

            MetaGameManager.Instance.RegisterRunScore(playerName, runScore);
            MetaGameManager.Instance.RegisterRunForDailyMission(runScore);
            MetaGameManager.Instance.AddCurrency(reward);
        }

        Time.timeScale = 0f;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
