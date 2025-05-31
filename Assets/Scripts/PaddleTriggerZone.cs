using UnityEngine;

public class PaddleTriggerZone : MonoBehaviour
{
    public PaddleHitter paddleHitter; // Drag your PaddleRoot here in Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            // paddleHitter.SetBall(other.gameObject);   // Set ball reference
            paddleHitter.Hit();                        // 👈 Automatically trigger hit
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            // paddleHitter.ClearBall();
        }
    }
}
