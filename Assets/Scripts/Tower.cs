using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Tower : MonoBehaviour
{
    public event Action<bool> OnAddedFloor;

    [SerializeField] private List<FloorModule> floors = new List<FloorModule>();

    [SerializeField] private float perfectMarginX = 0.5f;
    [SerializeField] private float isAddableMarginX = 0.2f;
    private FloorModule LastFloor { get { return floors[^1]; } }

    private void Awake()
    {
        foreach (FloorModule floor in floors)
        {
            floor.OnFloorModuleCollision += HandleFloorCollision;
        }
    }

    public void AddFloor(FloorModule floor)
    {
        floor.transform.SetParent(transform.Find("Floors"));

        Rigidbody floorRb = floor.GetComponent<Rigidbody>();
        floorRb.isKinematic = true;

        FixFloorPos(floor);

        AdjustPerfect(floor);

        floor.SetSnap(floors[^1].GetComponent<Rigidbody>());

        floorRb.isKinematic = true;
        
        floors.Add(floor);
        floor.OnFloorModuleCollision += HandleFloorCollision;

        OnAddedFloor?.Invoke(true);
    }

    private void FixFloorPos(FloorModule floor)
    {
        floor.transform.rotation = Quaternion.identity;

        float newFloorHeight = (floor.GetComponent<BoxCollider>().size.y * floor.transform.localScale.y) / 2f;
        float lastFloorHeight = (LastFloor.GetComponent<BoxCollider>().size.y * floor.transform.localScale.y) / 2f;

        Vector3 newFloorPos = floor.transform.position;
        Vector3 lastFloorPos = LastFloor.transform.position;

        floor.transform.position = new Vector3(newFloorPos.x, lastFloorPos.y + newFloorHeight + lastFloorHeight, newFloorPos.z);
    }

    private bool IsAddable(FloorModule floor)
    {
        float distX = Math.Abs(LastFloor.transform.position.x - floor.transform.position.x);

        bool isAddable = floor.transform.position.y > LastFloor.transform.position.y + LastFloor.ColliderGlobalExtents.y
            && distX < floor.ColliderGlobalExtents.x + LastFloor.ColliderGlobalExtents.x - isAddableMarginX;

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
        }
    }

    private bool IsPerfect(FloorModule floor)
    {
        return MathF.Abs(floor.transform.position.x - LastFloor.transform.position.x) < perfectMarginX;
    }

    private void AdjustPerfect(FloorModule floor)
    {
        if (IsPerfect(floor))
        {
            Vector3 floorPos = floor.transform.position;
            Vector3 lastFloorPos = LastFloor.transform.position;

            floor.transform.position = new Vector3(lastFloorPos.x, floorPos.y, lastFloorPos.z);
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
