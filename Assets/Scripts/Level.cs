using UnityEngine;
using System;

public class Level : MonoBehaviour
{
    public event Action<FloorModule> OnCreateFloor;
    public event Action OnTryDropFloor;

    [SerializeField] private LevelScriptable levelData;

    private float craneAngle = 60f;
    private float craneSwingSpeed = 2f;
    private int maxFloors = 10;
    //Not regarding perfect score multipliers
    private float maxScorePerLand = 100f;

    //In meters
    [SerializeField] private float floorHeightUIMult = 2f;
    private float levelMaxHeight = 1f;

    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject[] floorModules;
    [SerializeField] private FloorModule floorModulePrefab;
    [SerializeField] private LevelUI levelUI;


    private GameObject currentFloor;
    private FloorModule currentFloorScript;

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
    bool hasLevelEnded = false;
    Vector3 movingScenaryTowards;
    private void Awake()
    {
        scenaryMoveDistance = (floorModulePrefab.GetComponent<BoxCollider>().size.y * floorModulePrefab.transform.localScale.y);

        tower.OnAddedFloor += HandleFloorSnap;
        playerController.OnDropFloorRequest += HandleDropFloorRequest;
        playerController.OnPauseRequest += HandlePauseRequest;

        craneAngle = levelData.CraneAngle;
        craneSwingSpeed = levelData.CraneSpeed;
        maxFloors = levelData.MaxFloors;
        maxScorePerLand = levelData.MaxScorePerLand;

        levelMaxHeight = maxFloors * floorHeightUIMult;
        crane.PendulumAngle = craneAngle;
        crane.SwingSpeed = craneSwingSpeed;
        levelUI.MaxTowerHeight = levelMaxHeight;
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
            currentFloorScript.OnBoundarieCollision -= HandleFloorBoundarieCollision;
        }

        int floorModulePrefabIndex;

        if (isLastFloor())
        {
            floorModulePrefabIndex = floorModules.Length - 1;
        }
        else
        {
            floorModulePrefabIndex = UnityEngine.Random.Range(0, floorModules.Length - 1);
        }

        currentFloor = Instantiate(floorModules[floorModulePrefabIndex]);
        currentFloorScript = currentFloor.GetComponent<FloorModule>();
        currentFloorScript.OnBoundarieCollision += HandleFloorBoundarieCollision;
        canCreateFloor = false;
        OnCreateFloor?.Invoke(currentFloorScript);
    }

    private bool isLastFloor()
    {
        return tower.FloorsCount + 1 >= maxFloors;
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
        UpdateScore(maxPossibleOffsetX, offsetX);

        perfectLandRow = isPerfect ? perfectLandRow + 1 : 0;

        levelUI.UpdateHUDData(tower.FloorsCount * floorHeightUIMult, perfectLandRow, score);

        hasLevelEnded = tower.FloorsCount >= maxFloors;

        if (hasLevelEnded)
        {
            EndLevel(true);
        }
        else
        {
            isScenaryMoving = true;
        }
    }

    private void EndLevel(bool hasWon)
    {
        levelUI.ShowGameEndedScreen(hasWon);

        if (hasWon && GameManager.Instance.GetBestScore() < score)
        {
            GameManager.Instance.SaveScore(score);
        }
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

    private void HandlePauseRequest()
    {
        if (!hasLevelEnded)
        {
            levelUI.TogglePause();
        }
    }

    private void HandleFloorBoundarieCollision()
    {
        hasLevelEnded = true;
        EndLevel(false);
    }

    private void OnDestroy()
    {
        tower.OnAddedFloor -= HandleFloorSnap;
        playerController.OnDropFloorRequest -= HandleDropFloorRequest;
        playerController.OnPauseRequest -= HandlePauseRequest;
    }
}
