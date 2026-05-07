using System;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private FloorModule floorModulePrefab;
    private FloorModule currentFloor;

    public event Action<FloorModule> OnCreateFloor;

    [SerializeField] private Crane crane;
    [SerializeField] private Tower tower;

    private void Update()
    {
        if (Input.GetKey(KeyCode.C))
        {
            CreateFloor();
        }
    }

    private void CreateFloor()
    {
        if (currentFloor == null)
        {
            currentFloor = Instantiate(floorModulePrefab);
            OnCreateFloor?.Invoke(currentFloor);
        }
    }
}
