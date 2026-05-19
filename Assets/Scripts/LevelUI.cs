using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    private float maxTowerHeight = 0f;
    private const float inactiveButtonAlpha = 0.75f;

    [SerializeField] private TMPro.TextMeshProUGUI hudTotalLandedText;
    [SerializeField] private TMPro.TextMeshProUGUI perfectRowText;
    [SerializeField] private TMPro.TextMeshProUGUI hudScoreText;

    [SerializeField] private TMPro.TextMeshProUGUI levelTitleText;
    [SerializeField] private TMPro.TextMeshProUGUI endGameTitleText;
    [SerializeField] private TMPro.TextMeshProUGUI endGameScoreText;
    [SerializeField] private TMPro.TextMeshProUGUI endGameTotalLandedText;
    [SerializeField] private TMPro.TextMeshProUGUI endGameBestScoreText;

    [SerializeField] private CanvasGroup hudCanvasGroup;
    [SerializeField] private CanvasGroup pauseCanvasGroup;
    [SerializeField] private CanvasGroup endGameCanvasGroup;
    [SerializeField] private CanvasGroup nextLevelCanvasGroup;
    [SerializeField] private CanvasGroup prevLevelCanvasGroup;

    [SerializeField] private Button resumeButton;
    [SerializeField] private Button menuButton;

    [SerializeField] private Button endGameMenuButton;
    [SerializeField] private Button prevLevelButton;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button retryButton;

    public float MaxTowerHeight { get { return maxTowerHeight; } set { maxTowerHeight = value; } }

    private void Awake()
    {
        perfectRowText.alpha = 0f;

        resumeButton.onClick.AddListener(HandleResumeButtonPressed);
        menuButton.onClick.AddListener(HandleMenuButtonPressed);
        endGameMenuButton.onClick.AddListener(HandleMenuButtonPressed);
        prevLevelButton.onClick.AddListener(HandlePrevLevelPressed);
        nextLevelButton.onClick.AddListener(HandleNextLevelPressed);
        retryButton.onClick.AddListener(HandleRetryButtonPressed);
    }

    private void Start()
    {
        levelTitleText.text = $"Level {GameManager.Instance.CurrentLevelIndex+1}";
        hudTotalLandedText.text = $"Tower height: 2 / {maxTowerHeight.ToString("F0")}";
        hudScoreText.text = $"Score: 0";

        endGameScoreText.text = "Score: 0";
        endGameTotalLandedText.text = $"Tower height: 0 / {maxTowerHeight}";
        endGameBestScoreText.text = $"Best Score: {GameManager.Instance.GetBestScore()}";
    }

    public void UpdateHUDData(float newTowerHeight, int newPerfectRow, int newScore)
    {
        string newTowerHeightString = newTowerHeight.ToString("F0");
        string maxTowerHeightString = maxTowerHeight.ToString("F0");
        string newScoreString = newScore.ToString("F0");

        hudTotalLandedText.text = $"Tower height: {newTowerHeightString} / {maxTowerHeightString}";
        hudScoreText.text = $"Score: {newScoreString}";

        if (newPerfectRow > 0f)
        {
            perfectRowText.alpha = 1f;
            perfectRowText.text = $"Perfect row: {newPerfectRow}";
        }
        else
        {
            perfectRowText.alpha = 0f;
        }

        endGameScoreText.text = $"Score: {newScoreString}";
        endGameTotalLandedText.text = $"Tower height: {newTowerHeightString} / {maxTowerHeightString}";
    }

    public void TogglePause()
    {
        GameManager.Instance.TogglePause();
        SetCanvasState(pauseCanvasGroup, GameManager.Instance.IsPaused);
    }

    public void ShowGameEndedScreen(bool hasWon)
    {
        endGameTitleText.text = hasWon ? "YOU WON" : "YOU LOST";

        if (GameManager.Instance.IsAtLastLevel())
        {
            SetButtonActive(nextLevelCanvasGroup, false);
        }

        if (GameManager.Instance.IsAtFirstLevel())
        {
            SetButtonActive(prevLevelCanvasGroup, false);
        }

        GameManager.Instance.TogglePause();
        SetCanvasState(hudCanvasGroup, false);
        SetCanvasState(endGameCanvasGroup, true);
    }

    private void SetButtonActive(CanvasGroup canvas, bool isActive)
    {
        canvas.alpha = isActive ? 1f : inactiveButtonAlpha;
        canvas.interactable = isActive;
    }
    private void SetCanvasState(CanvasGroup canvas, bool state)
    {
        canvas.alpha = state ? 1f : 0f;
        canvas.interactable = state;
        canvas.blocksRaycasts = state;
    }

    private void HandleResumeButtonPressed()
    {
        TogglePause();
    }

    private void HandleMenuButtonPressed()
    {
        GameManager.Instance.TogglePause();
        GameManager.Instance.MoveToMainMenu();
        GameManager.Instance.ButtonPressedSound.Play();
    }

    private void HandlePrevLevelPressed()
    {
        GameManager.Instance.TogglePause();
        GameManager.Instance.ButtonPressedSound.Play();
        GameManager.Instance.ChangePrevLevel();
    }

    private void HandleNextLevelPressed()
    {
        GameManager.Instance.TogglePause();
        GameManager.Instance.ButtonPressedSound.Play();
        GameManager.Instance.ChangeNextLevel();
    }

    private void HandleRetryButtonPressed()
    {
        GameManager.Instance.TogglePause();
        GameManager.Instance.ButtonPressedSound.Play();
        GameManager.Instance.ResetLevel();
    }

    private void OnDestroy()
    {
        resumeButton.onClick.RemoveListener(HandleResumeButtonPressed);
        resumeButton.onClick.RemoveListener(HandleMenuButtonPressed);
        endGameMenuButton.onClick.RemoveListener(HandleMenuButtonPressed);
        prevLevelButton.onClick.RemoveListener(HandlePrevLevelPressed);
        nextLevelButton.onClick.RemoveListener(HandleNextLevelPressed);
        retryButton.onClick.RemoveListener(HandleRetryButtonPressed);
    }
}
