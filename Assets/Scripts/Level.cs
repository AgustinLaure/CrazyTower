using System;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private FloorModule floorModulePrefab;
    private FloorModule currentFloor;
    private bool canCreateFloor;

    public event Action<FloorModule> OnCreateFloor;

    [SerializeField] private Crane crane;
    [SerializeField] private Tower tower;
    [SerializeField] private GameObject movableScenary;

    //How much the scenary moves when adding a new floor to tower
    private float scenaryMoveDistance;
    [SerializeField] private float scenaryMoveSpeed;

    private void Awake()
    {
        canCreateFloor = true;
        scenaryMoveDistance = (floorModulePrefab.GetComponent<BoxCollider>().size.y * floorModulePrefab.transform.localScale.y);

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

    private void MoveScenary()
    {
        Vector3 movingScenaryFrom = movableScenary.transform.position;
        Vector3 movingScenaryTo = new Vector3(movingScenaryFrom.x, movingScenaryFrom.y - scenaryMoveDistance, movingScenaryFrom.z);

        movableScenary.transform.position = Vector3.MoveTowards(movingScenaryFrom, movingScenaryTo, scenaryMoveSpeed);
    }
    private void HandleFloorSnap(bool isPerfect)
    {
        MoveScenary();
        canCreateFloor = true;
    }
    private void OnDestroy()
    {
        tower.OnAddedFloor -= HandleFloorSnap;
    }
}
