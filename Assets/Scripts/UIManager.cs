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
    public TMP_Text zoneColorText;

    [HideInInspector] public float lastPredictedAngle;
    [HideInInspector] public float lastPredictedVelocity;

    public void OnPredictPressed()
    {
        if (predictor == null)
        {
            Debug.LogWarning("TrajectorySimulator not assigned!");
            return;
        }

        float paddleVelocity = float.TryParse(paddleVelocityInput.text, out float pv) ? pv : 10f;
        float cor = float.TryParse(corInput.text, out float c) ? c : 0.5f;
        float targetX = float.TryParse(targetXInput.text, out float tx) ? tx : 1.5f;

        if (targetX > 2.65f)
        {
            targetX = 2.65f;
            Debug.LogWarning("Clamped targetX to map max.");
        }

        predictor.paddleVelocity = paddleVelocity;
        predictor.cor = cor;
        predictor.minAngle = 1f;
        predictor.maxAngle = 70f;

        predictor.Simulate();
        float bestAngle = predictor.GetAngleForTargetX(targetX);
        bestAngle = Mathf.Clamp(bestAngle, predictor.minAngle, predictor.maxAngle);

        lastPredictedAngle = bestAngle;
        lastPredictedVelocity = predictor.GetVelocityForAngle(bestAngle);

        paddleVelocityText.text = $"Paddle Velocity: {paddleVelocity:F2} m/s";
        corValueText.text = $"COR: {cor:F2}";
        outputAngleText.text = $"Suggested Paddle Angle: {bestAngle:F2}°";

        string zone = predictor.GetZoneColor(targetX);
        zoneColorText.text = $"Zone Color: {zone}";

        Debug.Log($"TargetX={targetX} → Best Angle: {bestAngle:F2}°");
    }
}
