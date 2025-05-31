using UnityEngine;

public class BallPhysicsController : MonoBehaviour
{
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Launch(Vector3 direction, float speed)
    {
        rb.linearVelocity = direction.normalized * speed;
        Debug.Log("Ball launched with velocity: " + rb.linearVelocity);
    }
}
