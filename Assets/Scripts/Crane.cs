using UnityEngine;
public class Crane : MonoBehaviour
{
    [SerializeField] private Level level;
    [SerializeField] private float swingSpeed;
    [SerializeField] private ForceMode forceMode;

    [SerializeField] private Transform floorSpawnPoint;
    [SerializeField] private Transform pivotTransform;

    [SerializeField] private GameObject wire;

    private Transform wireTransform;
    private Rigidbody wireRb;

    private FloorModule currentFloor;

    //In degrees
    [SerializeField] private float pendulumAngle;
    private float distanceToLoopingPoint;

    private bool isSwinging;
    private bool push;

    private const float maxPendulumAngle = 90f;

    private void Awake()
    {
        wireTransform = wire.GetComponent<Transform>();
        wireRb = wire.GetComponent<Rigidbody>();

        push = false;
        float pivotWireDistY = Mathf.Abs(pivotTransform.transform.position.y - wireTransform.transform.position.y);

        distanceToLoopingPoint = pendulumAngle * pivotWireDistY / maxPendulumAngle;
    }

    private void Start()
    {
        level.OnCreateFloor += HandleCreateFloor;
        level.OnTryDropFloor += HandleTryDropFloor;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            push = true;
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

    private void PushPendulum()
    {
        wireRb.AddRelativeForce(Vector3.right * swingSpeed, forceMode);
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
                wireRb.linearVelocity = Vector3.zero;
                wireRb.AddRelativeForce(-Vector3.right * swingSpeed, forceMode);
            }
            else if (pivotWireRightDistance < -distanceToLoopingPoint)
            {
                wireRb.linearVelocity = Vector3.zero;
                wireRb.AddRelativeForce(Vector3.right * swingSpeed, forceMode);
            }
        }
    }

    private void AddFloor(FloorModule floor)
    {
        currentFloor = floor;

        currentFloor.transform.SetParent(transform);
        currentFloor.transform.position = floorSpawnPoint.position;
        currentFloor.transform.rotation = wireTransform.rotation;
        currentFloor.SetSnap(wireRb);
    }
    private void DropFloor()
    {
        currentFloor.transform.SetParent(null);
        currentFloor.SetLanding();
        currentFloor = null;
    }
    private void HandleCreateFloor(FloorModule floor)
    {
        AddFloor(floor);
    }

    private void HandleTryDropFloor()
    {
        if (currentFloor)
        {
            DropFloor();
        }
    }
    private void OnDestroy()
    {
        level.OnCreateFloor -= HandleCreateFloor;
        level.OnTryDropFloor -= HandleTryDropFloor;
    }
}
