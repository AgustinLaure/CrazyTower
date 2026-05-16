using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public event Action OnDropFloorRequest;
    public event Action OnPauseRequest;

    [SerializeField] KeyCode dropFloorKey = KeyCode.Space;
    [SerializeField] KeyCode pauseKey = KeyCode.Escape;

    private void Update()
    {
        if (!GameManager.Instance.IsPaused)
        {
            if (Input.GetKeyDown(dropFloorKey))
            {
                OnDropFloorRequest?.Invoke();
            }
        }

        if (Input.GetKeyUp(pauseKey))
        {
            OnPauseRequest?.Invoke();
        }
    }
}
