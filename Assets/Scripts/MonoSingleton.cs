using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : class
{
    public static T Instance { get; private set; }
    [SerializeField] bool dontDestroyOnLoad = false;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;

            if (dontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }

            OnAwaken();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
            OnDestroyed();
        }
    }

    protected virtual void OnAwaken()
    {

    }

    protected virtual void OnDestroyed()
    {

    }
}
