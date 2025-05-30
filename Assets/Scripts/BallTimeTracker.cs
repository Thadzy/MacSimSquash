using UnityEngine;
using TMPro;

public class BallTimeTracker : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public PaddleHitter paddle;         // Drag your paddle GameObject with PaddleHitter.cs
    public BallDropper ballDropper;     // Dropper that spawns the ball

    private float timer = 0f;
    private bool isCounting = false;
    private GameObject ballInstance;

    private bool hasHit = false;  // ✅ Prevents multiple paddle hits

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartSimulation();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            ResetSimulation();
        }

        if (isCounting)
        {
            timer += Time.deltaTime;
            timeText.text = $"Ball Time: {timer:F2} s";

            // ✅ Trigger paddle once at t = 0.55s
            if (!hasHit && timer >= 0.55f)
            {
                paddle?.Hit();
                hasHit = true;
                Debug.Log("✅ Paddle hit at t = 0.55s");
            }
        }
    }

    void StartSimulation()
    {
        if (ballInstance != null)
            Destroy(ballInstance);

        if (ballDropper != null)
            ballInstance = ballDropper.DropBall();

        timer = 0f;
        isCounting = true;
        hasHit = false; // reset hit trigger
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

        // 🔁 Reset paddle rotation
        paddle?.ResetPaddle();

        Debug.Log("⏹️ Simulation reset.");
    }

}
