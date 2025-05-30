using UnityEngine;

public class PaddleHitter : MonoBehaviour
{
    public float hitAngle = 45f;
    public float hitSpeed = 300f;

    private float currentAngle = 0f;
    private bool isHitting = false;
    private bool isReturning = false;

    private Quaternion originalRotation;

    void Start()
    {
        originalRotation = transform.rotation;
    }

    public void Hit()
    {
        isHitting = true;
        currentAngle = 0f;
    }

    public void ResetPaddle()
    {
        isHitting = false;
        isReturning = false;
        currentAngle = 0f;
        transform.rotation = originalRotation;
    }

    void Update()
    {
        if (isHitting)
        {
            float step = hitSpeed * Time.deltaTime;
            transform.Rotate(Vector3.up, -step);  // Adjust direction if needed
            currentAngle += step;

            if (currentAngle >= hitAngle)
            {
                isHitting = false;
                isReturning = true;
                currentAngle = 0f;
            }
        }
        else if (isReturning)
        {
            float step = hitSpeed * Time.deltaTime;
            transform.Rotate(Vector3.up, step);  // Return in opposite direction
            currentAngle += step;

            if (currentAngle >= hitAngle)
            {
                isReturning = false;
                currentAngle = 0f;
                transform.rotation = originalRotation;
            }
        }
    }
}
