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
        Vector3 linearVel = floorRb.linearVelocity;
        Vector3 angularVel = floorRb.angularVelocity;

        currentFloor.transform.SetParent(null);
        currentFloor.GetComponent<ConfigurableJoint>().connectedBody = null;
        currentFloor.GetComponent<ConfigurableJoint>().xMotion = ConfigurableJointMotion.Free;
        currentFloor.GetComponent<ConfigurableJoint>().yMotion = ConfigurableJointMotion.Free;
        currentFloor.GetComponent<ConfigurableJoint>().zMotion = ConfigurableJointMotion.Free;

        currentFloor.GetComponent<ConfigurableJoint>().angularXMotion = ConfigurableJointMotion.Free;
        currentFloor.GetComponent<ConfigurableJoint>().angularYMotion = ConfigurableJointMotion.Free;
        currentFloor.GetComponent<ConfigurableJoint>().angularZMotion = ConfigurableJointMotion.Limited;

        

        //floorRb.linearVelocity = linearVel;
        //floorRb.angularVelocity = angularVel;
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
