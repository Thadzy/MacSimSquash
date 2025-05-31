using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class PaddleHitter : MonoBehaviour
{
    public UIManager uiManager;
    public float hitSpeed = 300f;
    private float currentAngle = 0f;
    private bool isHitting = false;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        GetComponent<Collider>().isTrigger = true;
    }

    public void Hit()
    {
        isHitting = true;
        currentAngle = 0f;
        Debug.Log("Paddle starting rotation");
    }

    public void ResetPaddle()
    {
        isHitting = false;
        currentAngle = 0f;
        transform.localRotation = Quaternion.identity;
        Debug.Log("Paddle reset");
    }

    void Update()
    {
        if (!isHitting) return;

        float targetAngle = uiManager != null ? uiManager.lastPredictedAngle : 60f;
        float step = hitSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up, -step);
        currentAngle += step;

        if (currentAngle >= targetAngle)
        {
            isHitting = false;
            transform.localRotation = Quaternion.Euler(-targetAngle, 0, 0);
            Debug.Log($"Paddle reached angle: {targetAngle:F2}°");
        }
    }
}