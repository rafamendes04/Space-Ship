using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Central game manager: score, win/lose conditions, slow-motion unlock.
/// Attach to a persistent GameObject called "GameManager".
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // ─── Inspector References ────────────────────────────────────────────────
    [Header("UI – HUD")]
    public TextMeshProUGUI scoreText;

    [Header("UI – Game Over Screen")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;
    public Button restartButtonLose;

    [Header("UI – Victory Screen")]
    public GameObject victoryPanel;
    public TextMeshProUGUI victoryText;
    public Button restartButtonWin;

    [Header("Game Rules")]
    public int slowMotionUnlockScore = 10;
    public int victoryScore          = 50;

    // ─── State ───────────────────────────────────────────────────────────────
    private int  _score           = 0;
    private bool _isPlaying       = true;
    private bool _slowUnlocked    = false;

    public bool IsPlaying              => _isPlaying;
    public bool IsSlowMotionUnlocked   => _slowUnlocked;

    // ─── Lifecycle ────────────────────────────────────────────────────────────
    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        Time.timeScale      = 1f;
        Time.fixedDeltaTime = 0.02f;

        if (gameOverPanel  != null) gameOverPanel.SetActive(false);
        if (victoryPanel   != null) victoryPanel.SetActive(false);

        if (restartButtonLose != null) restartButtonLose.onClick.AddListener(RestartGame);
        if (restartButtonWin  != null) restartButtonWin.onClick.AddListener(RestartGame);

        UpdateScoreUI();
    }

    // ─── Public API ───────────────────────────────────────────────────────────
    public void AddScore(int amount)
    {
        if (!_isPlaying) return;

        _score += amount;
        UpdateScoreUI();

        if (!_slowUnlocked && _score >= slowMotionUnlockScore)
        {
            _slowUnlocked = true;
            Debug.Log("[GameManager] Slow Motion unlocked!");
        }

        if (_score >= victoryScore)
        {
            TriggerVictory();
        }
    }

    public void TriggerGameOver()
    {
        if (!_isPlaying) return;
        _isPlaying = false;

        // Ensure normal time returns
        Time.timeScale      = 1f;
        Time.fixedDeltaTime = 0.02f;

        if (gameOverPanel != null) gameOverPanel.SetActive(true);
    }

    public void TriggerVictory()
    {
        if (!_isPlaying) return;
        _isPlaying = false;

        Time.timeScale      = 1f;
        Time.fixedDeltaTime = 0.02f;

        if (victoryPanel != null) victoryPanel.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale      = 1f;
        Time.fixedDeltaTime = 0.02f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // ─── Private Helpers ─────────────────────────────────────────────────────
    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = $"Score: {_score}";
    }
}
