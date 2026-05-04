using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public event Action OnPlay;
    public event Action OnExit;

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
        playButton.onClick.AddListener(OnPlayPressed);
        htpButton.onClick.AddListener(OnHtpPressed);
        creditsButton.onClick.AddListener(OnCreditsPressed);
        exitButton.onClick.AddListener(OnExitPressed);
        htpBackButton.onClick.AddListener(OnHtpBackPressed);
        creditsBackButton.onClick.AddListener(OnCreditsBackPressed);
    }

    private void OnDestroy()
    {
        playButton.onClick.RemoveListener(OnPlayPressed);
        htpButton.onClick.RemoveListener(OnHtpPressed);
        creditsButton.onClick.RemoveListener(OnCreditsPressed);
        exitButton.onClick.RemoveListener(OnExitPressed);
        htpBackButton.onClick.RemoveListener(OnHtpBackPressed);
        creditsBackButton.onClick.RemoveListener(OnCreditsBackPressed);
    }

    private void SetCanvasState(CanvasGroup canvas, bool state)
    {
        canvas.alpha = state ? 1 : 0;
        canvas.interactable = state;
        canvas.blocksRaycasts = state;
    }

    private void OnPlayPressed()
    {
        OnPlay?.Invoke();
    }

    private void OnHtpPressed()
    {
        SetCanvasState(mainMenu, false);
        SetCanvasState(howToPlay, true);
    }

    private void OnCreditsPressed()
    {
        SetCanvasState(mainMenu, false);
        SetCanvasState(credits, true);
    }
    private void OnExitPressed()
    {
        OnExit?.Invoke();
    }

    private void OnHtpBackPressed()
    {
        SetCanvasState(howToPlay, false);
        SetCanvasState(mainMenu, true);
    }

    private void OnCreditsBackPressed()
    {
        SetCanvasState(credits, false);
        SetCanvasState(mainMenu, true);
    }
}
