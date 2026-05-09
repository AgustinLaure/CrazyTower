using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public event Action OnDropFloorRequest;

    [SerializeField] KeyCode dropFloorKey;

    private void Update()
    {
        if (Input.GetKeyDown(dropFloorKey))
        {
            OnDropFloorRequest?.Invoke();
        }
    }
}
