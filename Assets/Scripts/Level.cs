using System;
using UnityEngine;

public class Level : MonoBehaviour
{
    public event Action<FloorModule> OnCreateFloor;
    public event Action OnTryDropFloor;

    [SerializeField] private PlayerController playerController;
    [SerializeField] private FloorModule floorModulePrefab;
    [SerializeField] private LevelUI levelUI;

    //Not regarding perfect score multipliers
    [SerializeField] private float maxScorePerLand = 100f;

    private FloorModule currentFloor;
    private bool isPlaying = true;
    private bool canCreateFloor = true;
    private int perfectLandRow = 0;
    private int score = 0;

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

    private void HandleFloorSnap(bool isPerfect, float maxPossibleOffsetX, float offsetX)
    {
        isScenaryMoving = true;

        perfectLandRow = isPerfect ? perfectLandRow + 1 : 0;

        UpdateScore(maxPossibleOffsetX, offsetX);

        levelUI.UpdateHUDData(tower.Height, perfectLandRow, score);
    }

    private void UpdateScore(float maxPossibleOffsetX, float offsetX)
    {
        int possibleScore = (int)(maxScorePerLand - maxScorePerLand * (offsetX / maxPossibleOffsetX));
        int possibleScoreRest = possibleScore % 5;
        possibleScore -= possibleScoreRest;

        if (perfectLandRow > 0)
        {
            possibleScore *= perfectLandRow;
        }

        score += possibleScore;
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
