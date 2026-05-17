using System;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public event Action<bool, float, float> OnAddedFloor;

    [SerializeField] private List<FloorModule> floors = new List<FloorModule>();
    [SerializeField] private float perfectMarginX = 0.5f;
    [SerializeField] private float isAddableMarginX = 0.2f;
    [SerializeField] private AudioSource floorMissSound;
    [SerializeField] private AudioSource floorSnapSound;
    [SerializeField] private AudioSource perfectSnapSound;

    [SerializeField] private AudioSource beforeBreakingSound;
    [SerializeField] private AudioSource breakSound;
    public float FloorsCount { get { return floors.Count; } }

    private FloorModule lastFloor { get { return floors[^1]; } }
    private float relativeBobSpeed { get { return Math.Abs(bobStartPos.x - bobEndPos.x) * bobSpeed / objectiveDistance; } }

    private bool isBobbing = false;
    private bool shouldCrumble = false;

    //Proportional to ObjectiveDistance
    [SerializeField] private float bobSpeed = 1f;
    private float objectiveDistance = 5f;

    [SerializeField] private float minFloorsToBob = 1f;

    [SerializeField] private float maxBobintensityToCrumble = 0.6f;

    private Vector3 bobStartPos;
    private Vector3 bobEndPos;
    private float bobMultiplier = 0.5f;
    private float bobIntensity = 0f;
    private int bobbingDir = 1;
    private bool isBobbingSet = false;

    private void Awake()
    {
        bobStartPos = transform.position;
        foreach (FloorModule floor in floors)
        {
            floor.OnFloorModuleCollision += HandleFloorCollision;
        }
    }

    private void Update()
    {
        if (!isBobbing)
        {
            CheckShouldBob();
        }
        else
        {
            Bob();
        }
    }

    private void CheckShouldBob()
    {
        if (floors.Count >= minFloorsToBob)
        {
            isBobbing = true;
            bobStartPos = lastFloor.transform.position;
        }
    }
    private void Bob()
    {
        if (!isBobbingSet)
        {
            bobEndPos = new Vector3(bobStartPos.x + bobIntensity * bobbingDir, transform.position.y, transform.position.z);
        }

        transform.position = Vector3.MoveTowards(transform.position, bobEndPos, relativeBobSpeed * Time.deltaTime);

        if (transform.position == bobEndPos)
        {
            if (shouldCrumble && ((bobbingDir < 0f && bobIntensity < 0f) || (bobbingDir > 0f && bobIntensity > 0f)))
            {
                Crumble();
            }

            bobbingDir *= -1;
            isBobbingSet = false;
        }
    }

    private void Crumble()
    {
        foreach (var floor in floors)
        {
            floor.SetFalling();
            floor.GetComponent<Rigidbody>().isKinematic = false;
            floor.OnFloorModuleCollision -= HandleFloorCollision;
        }

        floors.Clear();

        breakSound.Play();
        beforeBreakingSound.Stop();
    }

    private void CheckShouldCrumble()
    {
        Debug.Log(bobIntensity);

        shouldCrumble = Math.Abs(bobIntensity) >= maxBobintensityToCrumble;

        if (shouldCrumble)
        {
            beforeBreakingSound.Play();
        }
    }

    private float UpdateBobIntensity(Vector3 floorPos)
    {
        Vector3 lastFloorPos = lastFloor.transform.position;

        float distX = Math.Abs(floorPos.x - lastFloor.transform.position.x);
        int dir = 0;

        if (floorPos.x < lastFloorPos.x)
        {
            dir = -1;
        }
        else if (floorPos.x > lastFloorPos.x)
        {
            dir = 1;
        }

        bobIntensity += distX * dir * bobMultiplier;

        CheckShouldCrumble();

        return distX;
    }

    private void UpdateBobDir(Vector3 floorPos)
    {
        if (floorPos.x < lastFloor.transform.position.x)
        {
            bobbingDir = -1;
        }
        else if (floorPos.x > lastFloor.transform.position.x)
        {
            bobbingDir = 1;
        }

        isBobbingSet = false;
    }

    public void AddFloor(FloorModule floor)
    {
        floor.transform.SetParent(transform.Find("Floors"));
        Rigidbody floorRb = floor.GetComponent<Rigidbody>();
        bool isPerfect = IsPerfect(floor);

        FixFloorPos(floor);
        AdjustPerfect(floor, isPerfect);

        floor.SetSnap(floors[^1].GetComponent<Rigidbody>());
        floorRb.isKinematic = true;

        float maxPossibleOffsetX = floor.ColliderGlobalExtents.x + lastFloor.ColliderGlobalExtents.x - isAddableMarginX;
        float floorOffsetX = UpdateBobIntensity(floor.transform.position);

        UpdateBobDir(floor.transform.position);

        floors.Add(floor);
        floor.OnFloorModuleCollision += HandleFloorCollision;

        if (!shouldCrumble)
        {
            OnAddedFloor?.Invoke(isPerfect, maxPossibleOffsetX, floorOffsetX);
        }

        if (!isPerfect)
        {
            floorSnapSound.Play();
        }
    }

    private void FixFloorPos(FloorModule floor)
    {
        floor.transform.rotation = Quaternion.identity;

        float newFloorHeight = (floor.GetComponent<BoxCollider>().size.y * floor.transform.localScale.y) / 2f;
        float lastFloorHeight = (lastFloor.GetComponent<BoxCollider>().size.y * floor.transform.localScale.y) / 2f;

        Vector3 newFloorPos = floor.transform.position;
        Vector3 lastFloorPos = lastFloor.transform.position;

        floor.transform.position = new Vector3(newFloorPos.x, lastFloorPos.y + newFloorHeight + lastFloorHeight, newFloorPos.z);
    }

    private bool IsAddable(FloorModule floor)
    {
        float maxPossibleDistance = floor.ColliderGlobalExtents.x + lastFloor.ColliderGlobalExtents.x - isAddableMarginX;

        float distX = Math.Abs(lastFloor.transform.position.x - floor.transform.position.x);
        bool isAddable = floor.transform.position.y > lastFloor.transform.position.y + lastFloor.ColliderGlobalExtents.y
            && distX <= maxPossibleDistance;

        return isAddable;
    }

    private void PushUnaddableFloor(FloorModule floor)
    {
        floor.SetFalling();
    }

    private void HandleFloorCollision(FloorModule floor)
    {
        if (IsAddable(floor))
        {
            if (!floor.IsJointConnected())
            {
                AddFloor(floor);
            }
        }
        else
        {
            PushUnaddableFloor(floor);
            floorMissSound.Play();
        }
    }

    private bool IsPerfect(FloorModule floor)
    {
        return MathF.Abs(floor.transform.position.x - lastFloor.transform.position.x) < perfectMarginX;
    }

    private void AdjustPerfect(FloorModule floor, bool isPerfect)
    {
        if (isPerfect)
        {
            Vector3 floorPos = floor.transform.position;
            Vector3 lastFloorPos = lastFloor.transform.position;

            floor.transform.position = new Vector3(lastFloorPos.x, floorPos.y, lastFloorPos.z);

            perfectSnapSound.Play();
        }
    }

    private void OnDestroy()
    {
        foreach (FloorModule floor in floors)
        {
            floor.OnFloorModuleCollision -= HandleFloorCollision;
        }
    }
}
