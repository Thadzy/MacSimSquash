using UnityEngine;

public class PaddleHitter : MonoBehaviour
{
    public float hitAngle = 60f;
    public float hitSpeed = 200f;
    private float current = 0f;
    private bool hitting = false;

    void Update()
    {
        if (hitting && current < hitAngle)
        {
            float rotateAmount = hitSpeed * Time.deltaTime;
            transform.Rotate(Vector3.right, rotateAmount); // Try forward or up if it doesn’t move
            current += rotateAmount;
        }
    }

    public void Hit()
    {
        hitting = true;
        current = 0f;
    }
}
