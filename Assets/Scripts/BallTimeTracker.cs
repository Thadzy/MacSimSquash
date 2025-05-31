using UnityEngine;
using TMPro;

public class BallTimeTracker : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public PaddleHitter paddle;
    public BallDropper ballDropper;
    public float triggerTime = 0.55f;

    private float timer = 0f;
    private bool isCounting = false;
    private GameObject ballInstance;
    private bool hasHit = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) StartSimulation();
        if (Input.GetKeyDown(KeyCode.T)) ResetSimulation();

        if (isCounting)
        {
            timer += Time.deltaTime;
            timeText.text = $"Ball Time: {timer:F2} s";

            if (!hasHit && timer >= triggerTime)
            {
                paddle?.Hit();
                hasHit = true;
            }
        }
    }

    void StartSimulation()
    {
        if (ballInstance != null)
            Destroy(ballInstance);

        ballInstance = ballDropper?.LaunchBall();

        timer = 0f;
        isCounting = true;
        hasHit = false;
    }

    void ResetSimulation()
    {
        isCounting = false;
        timer = 0f;
        hasHit = false;
        timeText.text = "Ball Time: 0.00 s";

        if (ballInstance != null)
        {
            Destroy(ballInstance);
            ballInstance = null;
        }

        paddle?.ResetPaddle();
    }
}
