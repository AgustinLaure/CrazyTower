using System;
using UnityEngine;

public class FloorModule : MonoBehaviour
{
    [SerializeField] ConfigurableJoint joint;
    [SerializeField] Rigidbody rb;

    public event Action<FloorModule> OnFloorModuleCollision;
    public event Action OnBoundarieCollision;

    public void SetSnap(Rigidbody pivot)
    {
        joint.connectedBody = pivot;

        joint.xMotion = ConfigurableJointMotion.Locked;
        joint.yMotion = ConfigurableJointMotion.Locked;
        joint.zMotion = ConfigurableJointMotion.Locked;

        joint.angularXMotion = ConfigurableJointMotion.Locked;
        joint.angularYMotion = ConfigurableJointMotion.Locked;
        joint.angularZMotion = ConfigurableJointMotion.Locked;

        rb.freezeRotation = false;
    }

    public void SetLanding()
    {
        //Sets connectedBody as the world itself
        joint.connectedBody = null;

        joint.xMotion = ConfigurableJointMotion.Free;
        joint.yMotion = ConfigurableJointMotion.Free;
        joint.zMotion = ConfigurableJointMotion.Free;

        joint.angularXMotion = ConfigurableJointMotion.Free;
        joint.angularYMotion = ConfigurableJointMotion.Free;
        joint.angularZMotion = ConfigurableJointMotion.Free;

        rb.freezeRotation = true;
    }

    public void SetFalling()
    {
        joint.connectedBody = null;

        joint.xMotion = ConfigurableJointMotion.Free;
        joint.yMotion = ConfigurableJointMotion.Free;
        joint.zMotion = ConfigurableJointMotion.Free;

        joint.angularXMotion = ConfigurableJointMotion.Free;
        joint.angularYMotion = ConfigurableJointMotion.Free;
        joint.angularZMotion = ConfigurableJointMotion.Free;

        rb.freezeRotation = false;
    }

    public bool IsJointConnected()
    {
        return joint.connectedBody != null;
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
        else if (collision.gameObject.CompareTag("Boundarie"))
        {
            OnBoundarieCollision?.Invoke();
        }
    }

    public Vector3 ColliderGlobalSize { get { return GetComponent<BoxCollider>().size * transform.localScale.y; } }

    public Vector3 ColliderGlobalExtents { get { return (GetComponent<BoxCollider>().size * transform.localScale.y) / 2; } }
}
