using System;
using Unity.Content;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private FloorModule floorModulePrefab;
    private FloorModule currentFloor;
    private bool canCreateFloor;

    public event Action<FloorModule> OnCreateFloor;

    [SerializeField] private Crane crane;
    [SerializeField] private Tower tower;

    private void Awake()
    {
        canCreateFloor = true;
        tower.OnAddedFloor += HandleFloorSnap;
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.C))
        {
            CreateFloor();
        }
    }

    private void CreateFloor()
    {
        if (canCreateFloor)
        {
            currentFloor = Instantiate(floorModulePrefab);
            canCreateFloor = false;
            OnCreateFloor?.Invoke(currentFloor);
        }
    }

    private void HandleFloorSnap()
    {
        canCreateFloor = true;
    }
    private void OnDestroy()
    {
        tower.OnAddedFloor -= HandleFloorSnap;
    }
}
