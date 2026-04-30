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

    private float elapsedTime = 0f;
    private bool isGameOver = false;

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
            {
                gameOverPanel = fallbackPanel;
            }
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
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

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        Time.timeScale = 0f;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
