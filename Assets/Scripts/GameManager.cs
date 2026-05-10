using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager
{
    private static GameManager instance = null;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager();
            }

            return instance;
        }
        private set { } 
    }

    private string[] levels = { "Level01" };
    private int currentLevel = 0;

    public void Play()
    {
        SceneManager.LoadScene(levels[currentLevel]);
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
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
}
