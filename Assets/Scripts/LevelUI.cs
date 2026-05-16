using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    private float maxTowerHeight = 0f;

    [SerializeField] private TMPro.TextMeshProUGUI hudTotalLandedText;
    [SerializeField] private TMPro.TextMeshProUGUI perfectRowText;
    [SerializeField] private TMPro.TextMeshProUGUI hudScoreText;

    [SerializeField] private TMPro.TextMeshProUGUI endGameTitle;
    [SerializeField] private TMPro.TextMeshProUGUI endGameScoreText;
    [SerializeField] private TMPro.TextMeshProUGUI endGameTotalLandedText;

    [SerializeField] private CanvasGroup pauseCanvasGroup;
    [SerializeField] private CanvasGroup endGameCanvasGroup;

    [SerializeField] private Button resumeButton;
    [SerializeField] private Button menuButton;

    [SerializeField] private Button endGameMenuButton;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button retryButton;

    public float MaxTowerHeight { get { return maxTowerHeight; } set { maxTowerHeight = value; } }

    private void Awake()
    {
        perfectRowText.alpha = 0f;

        resumeButton.onClick.AddListener(HandleResumeButtonPressed);
        menuButton.onClick.AddListener(HandleMenuButtonPressed);
        endGameMenuButton.onClick.AddListener(HandleMenuButtonPressed);
        nextLevelButton.onClick.AddListener(HandleNextLevelPressed);
        retryButton.onClick.AddListener(HandleRetryButtonPressed);
    }

    private void Start()
    {
        hudTotalLandedText.text = $"Tower height: 2 / {maxTowerHeight.ToString("F0")}";
        hudScoreText.text = $"Score: 0";
    }

    public void UpdateHUDData(float newTowerHeight, int newPerfectRow, int newScore)
    {
        string newTowerHeightString = newTowerHeight.ToString("F0");
        string maxTowerHeightString = maxTowerHeight.ToString("F0");
        string newScoreString = maxTowerHeight.ToString("F0");

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
        endGameTitle.text = hasWon ? "YOU WON" : "YOU LOST";
        GameManager.Instance.TogglePause();
        SetCanvasState(endGameCanvasGroup, true);
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

    private void HandleNextLevelPressed()
    {
        GameManager.Instance.TogglePause();
        GameManager.Instance.ButtonPressedSound.Play();
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
        nextLevelButton.onClick.RemoveListener(HandleNextLevelPressed);
        retryButton.onClick.RemoveListener(HandleRetryButtonPressed);
    }
}
