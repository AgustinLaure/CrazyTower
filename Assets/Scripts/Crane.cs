using UnityEngine;
using UnityEngine.InputSystem;

public class Crane : MonoBehaviour
{
    [SerializeField] private FloorModule currentFloor;
    [SerializeField] private float swingSpeed;
    [SerializeField] private ForceMode forceMode;

    //In degrees
    [SerializeField] private float pendulumAngle;
    private float distanceToLoopingPoint;

    [SerializeField] private Transform pivotTransform;
    [SerializeField] private Transform wireTransform;

    [SerializeField] private Rigidbody rb;
    private bool isSwinging;
    private bool push;

    private const float maxPendulumAngle = 90f;

    private void Awake()
    {
        push = false;
        float pivotWireDistY = Mathf.Abs(pivotTransform.transform.position.y - wireTransform.transform.position.y);

        distanceToLoopingPoint = pendulumAngle * pivotWireDistY / maxPendulumAngle;
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.L))
        {
            DropFloor();
        }

        if (Input.GetKey(KeyCode.K))
        {
            push = true;
        }
        else
        {
            push = false;
        }
       
    }
    private void FixedUpdate()
    {
        if (push)
        {
            PushPendulum();
            push = false;
        }

        Swing();
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
        rb.AddRelativeForce(Vector3.right * swingSpeed, forceMode);
        isSwinging = true;
    }
    private void Swing()
    {
        if (isSwinging)
        {
            float wireRightProjMagnitude = Vector3.Dot(transform.right, wireTransform.position);
            float pivotRightProjMagnitude = Vector3.Dot(transform.right, pivotTransform.position);

            float pivotWireRightDistance = wireRightProjMagnitude - pivotRightProjMagnitude;

            if (pivotWireRightDistance > distanceToLoopingPoint)
            {
                rb.linearVelocity = Vector3.zero;
                rb.AddRelativeForce(-Vector3.right * swingSpeed, forceMode);
                Debug.Log("lineal" + rb.linearVelocity);
            }
            else if (pivotWireRightDistance < -distanceToLoopingPoint)
            {
                rb.linearVelocity = Vector3.zero;
                rb.AddRelativeForce(Vector3.right * swingSpeed, forceMode);
                Debug.Log("lineal" + rb.linearVelocity);
            }
        }
    }
}
