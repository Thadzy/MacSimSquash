using UnityEngine;

public class BallTriggerHeight : MonoBehaviour
{
    public Transform ball;
    public PaddleHitter paddleHitter;
    public float triggerHeight = 0.3f;
    private bool hasTriggered = false;

    void Update()
    {
        if (!hasTriggered && ball.position.y <= triggerHeight)
        {
            Debug.Log("Trigger height reached. Calling paddle hit.");
            paddleHitter.Hit(); // ✅ Call the method from the PaddleHitter script
            hasTriggered = true;
        }
    }

    public void ResetTrigger()
    {
        hasTriggered = false;
    }
}
