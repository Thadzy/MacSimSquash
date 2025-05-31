using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class PaddleHitter : MonoBehaviour
{
    public float hitAngle     = 60f;   // degrees
    public float hitSpeed     = 300f;  // deg/sec
    public float launchForce  = 6f;    // m/s initial ball speed

    Rigidbody rb;
    private float currentAngle = 0f;
    private bool isHitting     = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;  // so physics moves it
    }

    public void Hit()
    {
        isHitting     = true;
        currentAngle  = 0f;
    }

    public void ResetPaddle()
    {
        isHitting    = false;
        currentAngle = 0f;
        transform.localRotation = Quaternion.identity;
    }

    void Update()
    {
        if (!isHitting) return;

        float step = hitSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up, -step);
        currentAngle += step;

        if (currentAngle >= hitAngle)
            isHitting = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isHitting) return;                    // only hit during swing
        if (collision.gameObject.CompareTag("Ball"))
        {
            // Calculate launch direction: use paddle's forward axis
            Vector3 dir = transform.forward;       
            Rigidbody ballRb = collision.rigidbody;
            if (ballRb != null)
            {
                // Zero out any existing velocity, then apply new
                ballRb.linearVelocity = Vector3.zero;
                ballRb.AddForce(dir.normalized * launchForce, ForceMode.VelocityChange);
                Debug.Log("⚡ Ball physically launched at speed " + launchForce);
            }
        }
    }
}
