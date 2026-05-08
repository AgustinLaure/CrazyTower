using System;
using UnityEngine;

public class FloorModule : MonoBehaviour
{
    [SerializeField] ConfigurableJoint joint;
    [SerializeField] Rigidbody rb;

    public event Action<FloorModule> OnFloorModuleCollision;
    public void SetSnap(Rigidbody pivot)
    {
        joint.connectedBody = pivot;

        joint.xMotion = ConfigurableJointMotion.Locked;
        joint.yMotion = ConfigurableJointMotion.Locked;
        joint.zMotion = ConfigurableJointMotion.Locked;

        joint.angularXMotion = ConfigurableJointMotion.Locked;
        joint.angularYMotion = ConfigurableJointMotion.Locked;
        joint.angularZMotion = ConfigurableJointMotion.Locked;
    }

    public void SetFalling()
    {
        //Sets connectedBody as the world itself
        joint.connectedBody = null;

        joint.xMotion = ConfigurableJointMotion.Free;
        joint.yMotion = ConfigurableJointMotion.Free;
        joint.zMotion = ConfigurableJointMotion.Free;

        joint.angularXMotion = ConfigurableJointMotion.Free;
        joint.angularYMotion = ConfigurableJointMotion.Free;
        joint.angularZMotion = ConfigurableJointMotion.Limited;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("FloorModule"))
        {
            if (collision.gameObject.TryGetComponent<FloorModule>(out var floorModule))
            {
                OnFloorModuleCollision?.Invoke(floorModule);
            }
        }
    }
    // private void RotateToIdentity()
    // {
    //     transform.rotation = Quaternion.identity;
    //
    //     float floorHeight = GetComponent<BoxCollider>().bounds.extents.y;
    //
    //     //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.identity, resetRotationSpeed * Time.deltaTime);
    //     //
    //     //Debug.Log(transform.rotation.eulerAngles);
    //     //
    //     //if (transform.rotation == Quaternion.identity)
    //     //{
    //     //    isRotating = false;
    //     //    OnReadyToSnap?.Invoke();
    //     //}
    // }
}
