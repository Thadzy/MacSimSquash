using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    public TrajectorySimulator predictor;

    [Header("User Input")]
    public Slider forceSlider;
    public Slider corSlider;
    public TMP_InputField targetXInput;

    [Header("UI Text Outputs")]
    public TMP_Text outputAngleText;
    public TMP_Text forceValueText;
    public TMP_Text corValueText;

    public void OnPredictPressed()
    {
        if (predictor == null)
        {
            Debug.LogWarning("TrajectorySimulator not assigned!");
            return;
        }

        float userTargetX = float.Parse(targetXInput.text);

        // ✅ Sync slider values to simulator
        predictor.launchVelocity = forceSlider.value;
        predictor.cor = corSlider.value;

        // ✅ Update display values
        if (forceValueText != null)
            forceValueText.text = $"Force (u): {predictor.launchVelocity:F2} m/s";

        if (corValueText != null)
            corValueText.text = $"COR: {predictor.cor:F2}";

        // 🔁 Recalculate simulation and find angle
        predictor.Simulate();  // ✅ Correct
        float bestAngle = predictor.GetAngleForTargetX(userTargetX);
        outputAngleText.text = $"Suggested Paddle Angle: {bestAngle:F2}°";

        Debug.Log($"🎯 TargetX={userTargetX} → Angle: {bestAngle:F2}°");

        float effectiveVelocity = predictor.launchVelocity * predictor.cor;
        forceValueText.text = $"Effective Launch Speed: {effectiveVelocity:F2} m/s";

    }
}
