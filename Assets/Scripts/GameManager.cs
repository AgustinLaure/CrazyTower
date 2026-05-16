using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource buttonPressedSound;

    private const float initialMasterVolume = 0f;
    private const float initialSfxVolume = -30f;
    private const float initialMusicVolume = 0f;

    private string[] levels = { "Level01" };
    private int currentLevel = 0;
    private bool isPaused = false;

    private string wasGameOpenedBefore = "WasGameOpenedBefore";
    private string masterVolumeKey = "MasterVolume";
    private string sfxVolumeKey = "SfxVolume";
    private string muiscVolumeKey = "MusicVolume";

    private float masterVolume;
    private float sfxVolume;
    private float musicVolume;

    public float MasterVolume { get { return masterVolume; } set { masterVolume = value; audioMixer.SetFloat(masterVolumeKey, masterVolume); } }

    public float SfxVolume { get { return sfxVolume; } set { sfxVolume = value; audioMixer.SetFloat(sfxVolumeKey, sfxVolume); } }

    public float MusicVolume { get { return musicVolume; } set { musicVolume = value; audioMixer.SetFloat(muiscVolumeKey, musicVolume); } }

    public bool IsPaused { get { return isPaused; } private set { } }

    public AudioSource ButtonPressedSound { get { return buttonPressedSound; } private set { } }

    protected override void OnAwaken()
    {
        if (PlayerPrefs.HasKey(wasGameOpenedBefore))
        {
            masterVolume = PlayerPrefs.GetFloat(masterVolumeKey);
            sfxVolume = PlayerPrefs.GetFloat(sfxVolumeKey);
            musicVolume = PlayerPrefs.GetFloat(muiscVolumeKey);
        }
        else
        {
            masterVolume = initialMasterVolume;
            sfxVolume = initialSfxVolume;
            musicVolume = initialMusicVolume;

            SaveConfig();

            PlayerPrefs.SetInt(wasGameOpenedBefore, 1);
        }

        UpdateAudioMixerValues();
    }

    private void SetPrefValue(string key, float value)
    {

    }
    public void Play()
    {
        SceneManager.LoadScene(levels[currentLevel]);
    }

    public void QuitGame()
    {
        SaveConfig();
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void UpdateAudioMixerValues()
    {
        PlayerPrefs.SetFloat(masterVolumeKey, masterVolume);
        PlayerPrefs.SetFloat(sfxVolumeKey, sfxVolume);
        PlayerPrefs.SetFloat(muiscVolumeKey, musicVolume);
    }

    private void SaveConfig()
    {
        UpdateAudioMixerValues();
        PlayerPrefs.Save();
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void ChangeNextLevel()
    {
        if (currentLevel + 1 < levels.Length)
        {
            SceneManager.LoadScene(levels[currentLevel + 1]);
            currentLevel++;
        }
        else
        {
            Debug.LogError("Tried changing to next level at last level");
        }
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(levels[currentLevel]);
    }

    public void MoveToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    protected override void OnDestroyed()
    {
        SaveConfig();
    }
}
