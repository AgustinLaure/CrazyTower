using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup mainMenu;
    [SerializeField] private CanvasGroup howToPlay;
    [SerializeField] private CanvasGroup credits;

    [SerializeField] private Button playButton;
    [SerializeField] private Button htpButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button htpBackButton;
    [SerializeField] private Button creditsBackButton;

    private void Awake()
    {
        SetCanvasState(mainMenu, true);
        SetCanvasState(howToPlay, false);
        SetCanvasState(credits, false);
    }

    private void Start()
    {
        playButton.onClick.AddListener(HandlePlayPressed);
        htpButton.onClick.AddListener(HandleHtpPressed);
        creditsButton.onClick.AddListener(HandleCreditsPressed);
        exitButton.onClick.AddListener(HandleExitPressed);
        htpBackButton.onClick.AddListener(HandleHtpBackPressed);
        creditsBackButton.onClick.AddListener(HandleCreditsBackPressed);
    }

    private void OnDestroy()
    {
        playButton.onClick.RemoveListener(HandlePlayPressed);
        htpButton.onClick.RemoveListener(HandleHtpPressed);
        creditsButton.onClick.RemoveListener(HandleCreditsPressed);
        exitButton.onClick.RemoveListener(HandleExitPressed);
        htpBackButton.onClick.RemoveListener(HandleHtpBackPressed);
        creditsBackButton.onClick.RemoveListener(HandleCreditsBackPressed);
    }

    private void SetCanvasState(CanvasGroup canvas, bool state)
    {
        canvas.alpha = state ? 1 : 0;
        canvas.interactable = state;
        canvas.blocksRaycasts = state;
    }

    private void HandlePlayPressed()
    {
        GameManager.Instance.Play();
    }

    private void HandleHtpPressed()
    {
        SetCanvasState(mainMenu, false);
        SetCanvasState(howToPlay, true);
    }

    private void HandleCreditsPressed()
    {
        SetCanvasState(mainMenu, false);
        SetCanvasState(credits, true);
    }
    private void HandleExitPressed()
    {
        GameManager.Instance.QuitGame();
    }

    private void HandleHtpBackPressed()
    {
        SetCanvasState(howToPlay, false);
        SetCanvasState(mainMenu, true);
    }

    private void HandleCreditsBackPressed()
    {
        SetCanvasState(credits, false);
        SetCanvasState(mainMenu, true);
    }
}
