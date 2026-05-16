using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup mainMenu;
    [SerializeField] private CanvasGroup howToPlay;
    [SerializeField] private CanvasGroup credits;

    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button settingsBackButton;
    [SerializeField] private Button creditsBackButton;

    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;

    private void Awake()
    {
        SetCanvasState(mainMenu, true);
        SetCanvasState(howToPlay, false);
        SetCanvasState(credits, false);
    }

    private void Start()
    {
        masterVolumeSlider.value = GameManager.Instance.MasterVolume;
        sfxVolumeSlider.value = GameManager.Instance.SfxVolume;
        musicVolumeSlider.value = GameManager.Instance.MusicVolume;

        playButton.onClick.AddListener(HandlePlayPressed);
        settingsButton.onClick.AddListener(HandleSettingsPressed);
        creditsButton.onClick.AddListener(HandleCreditsPressed);
        exitButton.onClick.AddListener(HandleExitPressed);

        settingsBackButton.onClick.AddListener(HandleSettingsBackPressed);
        creditsBackButton.onClick.AddListener(HandleCreditsBackPressed);

        masterVolumeSlider.onValueChanged.AddListener(HandleMasterVolumeChanged);
        sfxVolumeSlider.onValueChanged.AddListener(HandleSfxVolumeChanged);
        musicVolumeSlider.onValueChanged.AddListener(HandleMusicVolumeChanged);
    }

    private void OnDestroy()
    {
        playButton.onClick.RemoveListener(HandlePlayPressed);
        settingsButton.onClick.RemoveListener(HandleSettingsPressed);
        creditsButton.onClick.RemoveListener(HandleCreditsPressed);
        exitButton.onClick.RemoveListener(HandleExitPressed);

        settingsBackButton.onClick.RemoveListener(HandleSettingsBackPressed);
        creditsBackButton.onClick.RemoveListener(HandleCreditsBackPressed);

        masterVolumeSlider.onValueChanged.RemoveListener(HandleMasterVolumeChanged);
        sfxVolumeSlider.onValueChanged.RemoveListener(HandleSfxVolumeChanged);
        musicVolumeSlider.onValueChanged.RemoveListener(HandleMusicVolumeChanged);
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
        GameManager.Instance.ButtonPressedSound.Play();
    }

    private void HandleSettingsPressed()
    {
        SetCanvasState(mainMenu, false);
        SetCanvasState(howToPlay, true);
        GameManager.Instance.ButtonPressedSound.Play();
    }

    private void HandleCreditsPressed()
    {
        SetCanvasState(mainMenu, false);
        SetCanvasState(credits, true);
        GameManager.Instance.ButtonPressedSound.Play();
    }
    private void HandleExitPressed()
    {
        GameManager.Instance.QuitGame();
        GameManager.Instance.ButtonPressedSound.Play();
    }

    private void HandleSettingsBackPressed()
    {
        SetCanvasState(howToPlay, false);
        SetCanvasState(mainMenu, true);
        GameManager.Instance.ButtonPressedSound.Play();
    }

    private void HandleCreditsBackPressed()
    {
        SetCanvasState(credits, false);
        SetCanvasState(mainMenu, true);
        GameManager.Instance.ButtonPressedSound.Play();
    }

    private void HandleMasterVolumeChanged(float value)
    {
        GameManager.Instance.MasterVolume = value;
    }

    private void HandleSfxVolumeChanged(float value)
    {
        GameManager.Instance.SfxVolume = value;
    }

    private void HandleMusicVolumeChanged(float value)
    {
        GameManager.Instance.MusicVolume = value;
    }
}
