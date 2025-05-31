using UnityEngine;
using TMPro;

public class BallTimeTracker : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public BallDropper ballDropper;

    private float timer = 0f;
    private bool isCounting = false;

    void Update()
    {
        if (!isCounting) return;

        timer += Time.deltaTime;
        if (timeText != null)
            timeText.text = $"Time: {timer:F2} s";
    }

    public void StartSimulation()
    {
        timer = 0f;
        isCounting = true;

        ballDropper?.DropBall();
    }

    public void ResetSimulation()
    {
        timer = 0f;
        isCounting = false;
        if (timeText != null)
            timeText.text = "Time: 0.00 s";
    }
}
