using UnityEngine;
using UnityEngine.InputSystem;

public class Crane : MonoBehaviour
{
    private const float epsilon = 1e-5f;

    [SerializeField] private FloorModule currentFloor;

    //As the force applied when linearVelocity.x reaches 0 with a margin of minLinearToAccel
    [SerializeField] private float pendulumAngle;
    [SerializeField] private Rigidbody rb;
    private bool isSwinging;
    private int pendulumPushDir = 1;

    private const float minLinearToAccel = 0.9f;
    private void Update()
    {
        if (Input.GetKey(KeyCode.L))
        {
            DropFloor();
        }
    }

    private void FixedUpdate()
    {
        Swing();
        if (Input.GetKey(KeyCode.K))
        {
            PushPendulum();
        }
    }

    private void DropFloor()
    {
        Rigidbody floorRb = currentFloor.GetComponent<Rigidbody>();
        ConfigurableJoint floorJoint = currentFloor.GetComponent<ConfigurableJoint>();

        currentFloor.transform.SetParent(null);

        //Sets connectedBody as the world itself
        floorJoint.connectedBody = null;

        UnlockFloorMotion(floorJoint);
    }

    private void UnlockFloorMotion(ConfigurableJoint floorJoint)
    {
        floorJoint.xMotion = ConfigurableJointMotion.Free;
        floorJoint.yMotion = ConfigurableJointMotion.Free;
        floorJoint.zMotion = ConfigurableJointMotion.Free;

        floorJoint.angularXMotion = ConfigurableJointMotion.Free;
        floorJoint.angularYMotion = ConfigurableJointMotion.Free;
        floorJoint.angularZMotion = ConfigurableJointMotion.Limited;
    }
    private void PushPendulum()
    {
        rb.AddRelativeForce(Vector3.right * pendulumAngle, ForceMode.Acceleration);
        Debug.Log("lineal" + rb.linearVelocity);
        Debug.Log(" angular" + rb.angularVelocity);
        isSwinging = true;
    }
    private void Swing()
    {
        if (isSwinging)
        {
           if (rb.linearVelocity.x < -minLinearToAccel && pendulumPushDir > 0 || rb.linearVelocity.x > minLinearToAccel && pendulumPushDir < 0)
           {
               rb.AddForce(Vector3.left * (pendulumAngle * pendulumPushDir), ForceMode.Acceleration);
               pendulumPushDir *= -1;
           }
        }
    }
    
}
