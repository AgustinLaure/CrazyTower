using System;
using UnityEngine;

public class Level : MonoBehaviour
{
    public event Action<FloorModule> OnCreateFloor;
    public event Action OnTryDropFloor;

    [SerializeField] private PlayerController playerController;
    [SerializeField] private FloorModule floorModulePrefab;
    [SerializeField] private LevelUI levelUI;

    private FloorModule currentFloor;
    private bool isPlaying = true;
    private bool canCreateFloor = true;
    private float totalFloorsLanded = 0f;
    private float totalPerfectLanded = 0f;
    private float perfectLandRow = 0f;

    [SerializeField] private Crane crane;
    [SerializeField] private Tower tower;

    [SerializeField] private GameObject movableScenary;

    //How much the scenary moves when adding a new floor to tower
    private float scenaryMoveDistance;
    [SerializeField] private float scenaryMoveSpeed;

    bool isScenaryMoving = false;
    bool isScenaryTrayectoryDefined = false;
    Vector3 movingScenaryTowards;
    private void Awake()
    {
        scenaryMoveDistance = (floorModulePrefab.GetComponent<BoxCollider>().size.y * floorModulePrefab.transform.localScale.y);

        tower.OnAddedFloor += HandleFloorSnap;
        playerController.OnDropFloorRequest += HandleDropFloorRequest;
    }

    private void Update()
    {
        if (isPlaying)
        {
            if (canCreateFloor)
            {
                CreateFloor();
            }

            if (isScenaryMoving)
            {
                MoveScenary();
            }
        }
    }

    public void ResetLevel()
    {
        GameManager.Instance.ResetLevel();
    }
    private void CreateFloor()
    {
        if (currentFloor)
        {
            currentFloor.OnBoundarieCollision -= HandleFloorBoundarieCollision;
        }

        currentFloor = Instantiate(floorModulePrefab);
        currentFloor.OnBoundarieCollision += HandleFloorBoundarieCollision;
        canCreateFloor = false;
        OnCreateFloor?.Invoke(currentFloor);
    }

    private void MoveScenary()
    {
        if (!isScenaryTrayectoryDefined)
        {
            movingScenaryTowards = new Vector3(movableScenary.transform.position.x, movableScenary.transform.position.y - scenaryMoveDistance, movableScenary.transform.position.z);
            isScenaryTrayectoryDefined = true;
        }

        movableScenary.transform.position = Vector3.MoveTowards(movableScenary.transform.position, movingScenaryTowards, scenaryMoveSpeed * Time.deltaTime);

        if (movableScenary.transform.position == movingScenaryTowards)
        {
            isScenaryMoving = false;
            isScenaryTrayectoryDefined = false;
            canCreateFloor = true;
        }
    }

    private void HandleFloorSnap(bool isPerfect)
    {
        isScenaryMoving = true;
        totalFloorsLanded += 1f;

        if (isPerfect)
        {
            totalPerfectLanded += 1f;
            perfectLandRow += 1f;
        }
        else
        {
            perfectLandRow = 0f;
        }

        levelUI.UpdateHUDData(totalFloorsLanded, perfectLandRow);
    }

    private void HandleDropFloorRequest()
    {
        OnTryDropFloor?.Invoke();
    }

    private void HandleFloorBoundarieCollision()
    {
        ResetLevel();
    }

    private void OnDestroy()
    {
        tower.OnAddedFloor -= HandleFloorSnap;
        playerController.OnDropFloorRequest -= HandleDropFloorRequest;
    }
}
