using System;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private List<FloorModule> floors = new List<FloorModule>();

    private const float perfectMarginX = 10f;

    public event Action OnAddedFloor;

    private void Awake()
    {
        foreach (FloorModule floor in floors)
        {
            floor.OnFloorModuleCollision += HandleFloorCollision;
        }
    }

    private void Update()
    {
        Debug.Log(floors[^1].transform.position);
    }
    public void AddFloor(FloorModule floor)
    {
        floor.transform.SetParent(transform);

        Rigidbody floorRb = floor.GetComponent<Rigidbody>();

        floorRb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;

        FixFloorPos(floor);

        floor.SetSnap(floors[^1].GetComponent<Rigidbody>());

        floorRb.constraints = RigidbodyConstraints.None;

        floors.Add(floor);
        floor.OnFloorModuleCollision += HandleFloorCollision;

        OnAddedFloor?.Invoke();
    }

    private void FixFloorPos(FloorModule floor)
    {
        floor.transform.rotation = Quaternion.identity;

        float newFloorHeight = (floor.GetComponent<BoxCollider>().size.y * floor.transform.localScale.y) / 2f;
        float lastFloorHeight = (floors[^1].GetComponent<BoxCollider>().size.y * floor.transform.localScale.y) / 2f;

        Vector3 newFloorPos = floor.transform.position;
        Vector3 lastFloorPos = floors[^1].transform.position;

        floor.transform.position = new Vector3(newFloorPos.x, lastFloorPos.y + newFloorHeight + lastFloorHeight, newFloorPos.z);
    }

    private bool IsAddable(FloorModule floor)
    {
        return true;
    }
    private void HandleFloorCollision(FloorModule floor)
    {
        if (IsAddable(floor))
        {
            AddFloor(floor);
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
