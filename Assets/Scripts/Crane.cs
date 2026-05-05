using UnityEngine;
using UnityEngine.InputSystem;

public class Crane : MonoBehaviour
{
    private const float epsilon = 1e-5f;
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody rb;
    private bool isSwinging;
    private int pendulumPushDir = 1;
    private void Update()
    {

    }

    private void FixedUpdate()
    {
        Swing();
        if (Input.GetKey(KeyCode.K))
        {
            PushPendulum();
        }

    }

    private void PushPendulum()
    {
        rb.AddRelativeForce(Vector3.right * speed, ForceMode.Acceleration);
        Debug.Log("lineal" + rb.linearVelocity);
        Debug.Log(" angular" + rb.angularVelocity);
        isSwinging = true;
    }
    private void Swing()
    {
        if (isSwinging)
        {
           //if (!(rb.linearVelocity.x < 0 + epsilon && pendulumPushDir < 0 + epsilon))
           //{
           //    rb.AddRelativeForce(Vector3.right * (speed * pendulumPushDir), ForceMode.Acceleration);
           //    pendulumPushDir *= -1;
           //}
        }
    }
}
