using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Manages UI interactions for trajectory prediction system
/// Handles user input, displays results, and updates zone visualization
/// </summary>
public class UIManager : MonoBehaviour
{
    #region Component References
    [Header("References")]
    public TrajectorySimulator predictor;    // Physics simulation component
    public BallTimeTracker timeTracker;      // Time tracking component
    #endregion

    #region Input Fields
    [Header("User Input")]
    public TMP_InputField paddleVelocityInput;  // Speed of paddle input
    public TMP_InputField corInput;             // Coefficient of Restitution input
    public TMP_InputField targetXInput;         // Target distance input
    public TMP_InputField targetYInput;         // Target height input
    #endregion

    #region Output UI Elements
    [Header("UI Outputs")]
    public TMP_Text outputAngleText;      // Display calculated paddle angle
    public TMP_Text machineAngleText;     // Display calculated machine angle
    public TMP_Text paddleVelocityText;   // Display paddle velocity
    public TMP_Text corValueText;         // Display COR value
    public TMP_Text zoneColorText;        // Display target zone
    public TMP_Text targetHeightText;     // Display calculated height at target
    public Image zoneImage;               // Visual zone indicator
    #endregion

    #region Zone Color Configuration
    [Header("Zone Colors")]
    public Color redZone;     // High scoring zone
    public Color orangeZone;  // Medium-high scoring zone
    public Color yellowZone;  // Medium scoring zone
    public Color greenZone;   // Medium-low scoring zone
    public Color blueZone;    // Low scoring zone
    public Color outZone;     // Out of bounds color
    #endregion

    #region Cached Results
    [HideInInspector] public float lastPredictedAngle;    // Last calculated paddle angle
    [HideInInspector] public float lastPredictedVelocity; // Last calculated velocity
    [HideInInspector] public float lastTargetX;          // Last target distance
    [HideInInspector] public float lastTargetY;          // Last target height
    [HideInInspector] public float lastMachineAngle;     // Last calculated machine angle
    [HideInInspector] public float lastCalculatedHeight; // Last calculated height at target
    #endregion

    #region Unity Lifecycle
    void Start()
    {
        // Initialize zone image to white
        if (zoneImage != null)
            zoneImage.color = Color.white;
    }
    #endregion

    #region Main Prediction Logic
    /// <summary>
    /// Called when predict button is pressed - main calculation function
    /// </summary>
    public void OnPredictPressed()
    {
        // Validate predictor component
        if (predictor == null)
        {
            Debug.LogWarning("TrajectorySimulator not assigned.");
            return;
        }

        // Parse user inputs with defaults
        float paddleVelocity = ParseInput(paddleVelocityInput.text, 10f);
        float cor = ParseInput(corInput.text, 0.5f);
        float targetX = ParseInput(targetXInput.text, 1.5f);
        float targetY = ParseInput(targetYInput.text, 0f);

        // Clamp target distance to valid range
        if (targetX > 4.6f)
        {
            targetX = 4.6f;
            Debug.LogWarning("Clamped Target X to max range (4.6m)");
        }

        // Clamp target height to reasonable range
        if (targetY > 2f)
        {
            targetY = 2f;
            Debug.LogWarning("Clamped Target Y to max height (2m)");
        }
        if (targetY < 0f)
        {
            targetY = 0f;
            Debug.LogWarning("Clamped Target Y to minimum height (0m)");
        }

        // Configure simulation parameters
        SetupSimulation(paddleVelocity, cor);

        // Run physics simulation
        predictor.Simulate();

        // Calculate optimal angle for target
        float bestAngle = CalculateOptimalAngle(targetX);

        // Calculate machine angle for target position
        float machineAngle = CalculateMachineAngle(targetX, targetY);

        // Calculate height at target X position
        float calculatedHeight = CalculateHeightAtTarget(targetX, bestAngle);

        // Cache results for reference
        CacheResults(bestAngle, targetX, targetY, machineAngle, calculatedHeight);

        // Update all UI elements
        UpdateUI(paddleVelocity, cor, bestAngle, targetX, targetY, machineAngle, calculatedHeight);

        // Start time tracking if available
        if (timeTracker != null)
            timeTracker.StartSimulation();
    }
    #endregion

    #region Helper Methods
    /// <summary>
    /// Parse input field text to float with fallback default
    /// </summary>
    private float ParseInput(string input, float defaultValue)
    {
        return float.TryParse(input, out float result) ? result : defaultValue;
    }

    /// <summary>
    /// Configure simulation parameters
    /// </summary>
    private void SetupSimulation(float paddleVelocity, float cor)
    {
        predictor.paddleVelocity = paddleVelocity;
        predictor.cor = cor;
        predictor.minAngle = 1f;
        predictor.maxAngle = 70f;
    }

    /// <summary>
    /// Calculate and clamp optimal angle for target distance
    /// </summary>
    private float CalculateOptimalAngle(float targetX)
    {
        float bestAngle = predictor.GetAngleForTargetX(targetX);
        return Mathf.Clamp(bestAngle, predictor.minAngle, predictor.maxAngle);
    }

    /// <summary>
    /// Calculate machine angle for target position (X, Y)
    /// </summary>
    private float CalculateMachineAngle(float targetX, float targetY)
    {
        return predictor.GetMachineAngle(targetX, targetY);
    }

    /// <summary>
    /// Calculate height at target X position for given angle
    /// </summary>
    private float CalculateHeightAtTarget(float targetX, float bestAngle)
    {
        return predictor.GetHeightAtTargetX(targetX, bestAngle);
    }

    /// <summary>
    /// Store calculation results for later reference
    /// </summary>
    private void CacheResults(float bestAngle, float targetX, float targetY, float machineAngle, float calculatedHeight)
    {
        lastPredictedAngle = bestAngle;
        lastPredictedVelocity = predictor.GetVelocityForAngle(bestAngle);
        lastTargetX = targetX;
        lastTargetY = targetY;
        lastMachineAngle = machineAngle;
        lastCalculatedHeight = calculatedHeight;
    }

    /// <summary>
    /// Update all UI elements with calculated values
    /// </summary>
    private void UpdateUI(float paddleVelocity, float cor, float bestAngle, float targetX, float targetY, float machineAngle, float calculatedHeight)
    {
        // Update text displays
        UpdateText(paddleVelocityText, $"Paddle Velocity: {paddleVelocity:F2} m/s");
        UpdateText(corValueText, $"COR: {cor:F2}");
        UpdateText(outputAngleText, $"Paddle Angle: {bestAngle:F2}°");
        UpdateText(machineAngleText, $"Machine Angle: {machineAngle:F2}°");
        UpdateText(targetHeightText, $"Height at Target: {calculatedHeight:F2}m");

        // Update zone information
        string zone = predictor.GetZoneColor(targetX);
        UpdateText(zoneColorText, zone);
        UpdateZoneImageColor(zone);

        Debug.Log($"Target({targetX:F2}, {targetY:F2}) → Paddle: {bestAngle:F2}°, Machine: {machineAngle:F2}°");
    }

    /// <summary>
    /// Safely update text component if not null
    /// </summary>
    private void UpdateText(TMP_Text textComponent, string text)
    {
        if (textComponent != null)
            textComponent.text = text;
    }
    #endregion

    #region Zone Visualization
    /// <summary>
    /// Update zone image color based on target zone
    /// </summary>
    void UpdateZoneImageColor(string zone)
    {
        if (zoneImage == null)
        {
            Debug.LogWarning("ZoneImage is not assigned.");
            return;
        }

        Debug.Log("Zone Detected: " + zone);

        // Map zone names to colors
        Color targetColor = GetZoneColor(zone);
        zoneImage.color = targetColor;

        Debug.Log("Color changed to: " + zoneImage.color);
    }

    /// <summary>
    /// Get color for specific zone name
    /// </summary>
    private Color GetZoneColor(string zone)
    {
        switch (zone)
        {
            case "Red":
            case "Red 1":
            case "Red 2":
                return redZone;
            
            case "Orange":
            case "Orange 1":
            case "Orange 2":
                return orangeZone;
            
            case "Yellow":
            case "Yellow 1":
            case "Yellow 2":
            case "Goal Area":
                return yellowZone;
            
            case "Green":
            case "Green 1":
            case "Green 2":
                return greenZone;
            
            case "Light Blue":
            case "Light Blue 1":
            case "Light Blue 2":
                return blueZone;
            
            default:
                return outZone;
        }
    }
    #endregion

    #region Scene Management
    /// <summary>
    /// Navigate to settings scene
    /// </summary>
    public void OnSettingsPressed()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SettingsScene");
    }
    #endregion

    #region Debug Visualization
    /// <summary>
    /// Draw target position in scene view for debugging
    /// </summary>
    void OnDrawGizmos()
    {
        // Draw target X position
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(new Vector3(lastTargetX, 0.05f, 0), 0.05f);
        
        // Draw target Y position if specified
        if (lastTargetY > 0f)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(new Vector3(lastTargetX, lastTargetY, 0), 0.03f);
            
            // Draw line connecting ground and target height
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(new Vector3(lastTargetX, 0f, 0), new Vector3(lastTargetX, lastTargetY, 0));
        }
        
        // Draw calculated height at target
        if (lastCalculatedHeight > 0f)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(new Vector3(lastTargetX, lastCalculatedHeight, 0), 0.02f);
        }
    }
    #endregion
}