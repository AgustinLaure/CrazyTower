using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private MainMenu ui;
    void Start()
    {
        ui.OnPlay += OnPlay;
        ui.OnExit += OnExit;
    }

    private void OnPlay()
    {
        SceneManager.LoadScene("Gameplay");
    }

    private void OnExit()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    private void OnDisable()
    {
        ui.OnPlay -= OnPlay;
        ui.OnExit -= OnExit;
    }
}
