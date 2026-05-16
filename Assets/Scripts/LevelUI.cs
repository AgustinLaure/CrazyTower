using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI totalLandedText;
    [SerializeField] private TMPro.TextMeshProUGUI perfectRowText;
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;
    [SerializeField] private CanvasGroup pauseCanvasGroup;

    [SerializeField] private Button resumeButton;
    [SerializeField] private Button menuButton;

    private void Awake()
    {
        perfectRowText.alpha = 0f;

        resumeButton.onClick.AddListener(HandleResumeButtonPressed);
        menuButton.onClick.AddListener(HandleMenuButtonPressed);
    }

    public void UpdateHUDData(float newTowerHeight, int newPerfectRow, int newScore)
    {
        totalLandedText.text = $"Tower height: {newTowerHeight.ToString("F0")}";
        scoreText.text = $"Score: {newScore}";

        if (newPerfectRow > 0f)
        {
            perfectRowText.alpha = 1f;
            perfectRowText.text = $"Perfect row: {newPerfectRow}";
        }
        else
        {
            perfectRowText.alpha = 0f;
        }
    }

    public void TogglePause()
    {
        GameManager.Instance.TogglePause();
        SetCanvasState(pauseCanvasGroup, GameManager.Instance.IsPaused);
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
        GameManager.Instance.MoveToMainMenu();
    }

    private void OnDestroy()
    {
        resumeButton.onClick.RemoveListener(HandleResumeButtonPressed);
        resumeButton.onClick.RemoveListener(HandleMenuButtonPressed);
    }
}
