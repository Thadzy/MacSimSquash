// UIManager.cs
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    public TrajectorySimulator predictor;

    [Header("User Input (TMP Input Fields)")]
    public TMP_InputField paddleVelocityInput;
    public TMP_InputField corInput;
    public TMP_InputField targetXInput;

    [Header("UI Text Outputs")]
    public TMP_Text outputAngleText;
    public TMP_Text paddleVelocityText;
    public TMP_Text corValueText;

    public void OnPredictPressed()
    {
        if (predictor == null)
        {
            Debug.LogWarning("TrajectorySimulator not assigned!");
            return;
        }

        // Parse user inputs safely
        float paddleVelocity = float.TryParse(paddleVelocityInput.text, out float pv) ? pv : 10f;
        float cor = float.TryParse(corInput.text, out float c) ? c : 0.5f;
        float targetX = float.TryParse(targetXInput.text, out float tx) ? tx : 1.5f;

        predictor.paddleVelocity = paddleVelocity;
        predictor.cor = cor;
        predictor.minAngle = 1f;
        predictor.maxAngle = 70f; // Clamp angle to 70 max

        // Display updated values
        if (paddleVelocityText != null)
            paddleVelocityText.text = $"Paddle Velocity: {paddleVelocity:F2} m/s";

        if (corValueText != null)
            corValueText.text = $"COR: {cor:F2}";

        // Run simulation
        predictor.Simulate();
        float bestAngle = predictor.GetAngleForTargetX(targetX);
        bestAngle = Mathf.Clamp(bestAngle, predictor.minAngle, predictor.maxAngle); // Extra safety

        if (outputAngleText != null)
            outputAngleText.text = $"Suggested Paddle Angle: {bestAngle:F2}°";

        Debug.Log($"TargetX={targetX} → Best Angle: {bestAngle:F2}°");
    }
}
