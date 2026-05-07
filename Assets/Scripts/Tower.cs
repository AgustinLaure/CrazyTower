using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Tower : MonoBehaviour
{
    [SerializeField] private Rigidbody latestFloorRb;

    private const float perfectMarginX = 10f;
    public void AddFloor(FloorModule floor)
    {
        floor.transform.SetParent(transform);
        floor.transform.rotation = Quaternion.identity;
        floor.SetSnap(latestFloorRb);
    }
}
