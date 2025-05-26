using UnityEngine;

public class PaddleHitter : MonoBehaviour
{
    public float hitAngle = 45f;
    public float hitSpeed = 300f;

    private float currentAngle = 0f;
    private bool isHitting = false;

    public void Hit()
    {
        isHitting = true;
        currentAngle = 0f;
    }

    void Update()
    {
        if (isHitting)
        {
            float step = hitSpeed * Time.deltaTime;
            transform.Rotate(Vector3.up, step);
            currentAngle += step;

            if (currentAngle >= hitAngle)
                isHitting = false;
        }
    }
}
