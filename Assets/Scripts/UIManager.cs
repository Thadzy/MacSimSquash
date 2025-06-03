using UnityEngine;

using TMPro;

using UnityEngine.UI;



public class UIManager : MonoBehaviour

{

    [Header("References")]

    public TrajectorySimulator predictor;

    public BallTimeTracker timeTracker;



    [Header("User Input")]

    public TMP_InputField paddleVelocityInput;

    public TMP_InputField corInput;

    public TMP_InputField targetXInput;



    [Header("UI Outputs")]

    public TMP_Text outputAngleText;

    public TMP_Text paddleVelocityText;

    public TMP_Text corValueText;

    public TMP_Text zoneColorText;

    public Image zoneImage;



    [Header("Zone Colors")]

    public Color redZone;

    public Color orangeZone;

    public Color yellowZone;

    public Color greenZone;

    public Color blueZone;

    public Color outZone;



    [HideInInspector] public float lastPredictedAngle;

    [HideInInspector] public float lastPredictedVelocity;

    [HideInInspector] public float lastTargetX;



    void Start()

    {

        if (zoneImage != null)

            zoneImage.color = Color.white;

    }



    public void OnPredictPressed()

    {

        if (predictor == null)

        {

            Debug.LogWarning("TrajectorySimulator not assigned.");

            return;

        }



        if (!float.TryParse(paddleVelocityInput.text, out float paddleVelocity))

            paddleVelocity = 10f;



        if (!float.TryParse(corInput.text, out float cor))

            cor = 0.5f;



        if (!float.TryParse(targetXInput.text, out float targetX))

            targetX = 1.5f;



        if (targetX > 4.6f)

        {

            targetX = 4.6f;

            Debug.LogWarning("Clamped Target X to max range (4.6m)");

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

        lastTargetX = targetX;



        if (paddleVelocityText != null)

            paddleVelocityText.text = $"Paddle Velocity: {paddleVelocity:F2} m/s";



        if (corValueText != null)

            corValueText.text = $"COR: {cor:F2}";



        if (outputAngleText != null)

            outputAngleText.text = $"Paddle Angle: {bestAngle:F2}°";



        string zone = predictor.GetZoneColor(targetX);

        if (zoneColorText != null)

            zoneColorText.text = zone;



        UpdateZoneImageColor(zone);



        Debug.Log($"TargetX={targetX} → Best Angle: {bestAngle:F2}°");



        if (timeTracker != null)

            timeTracker.StartSimulation();

    }



    void UpdateZoneImageColor(string zone)

    {

        if (zoneImage == null)

        {

            Debug.LogWarning("ZoneImage is not assigned.");

            return;

        }



        Debug.Log("Zone Detected: " + zone);



        switch (zone)

        {

            case "Red":

            case "Red 1":

            case "Red 2":

                zoneImage.color = redZone;

                break;

            case "Orange":

            case "Orange 1":

            case "Orange 2":

                zoneImage.color = orangeZone;

                break;

            case "Yellow":

            case "Yellow 1":

            case "Yellow 2":

                zoneImage.color = yellowZone;

                break;

            case "Green":

            case "Green 1":

            case "Green 2":

                zoneImage.color = greenZone;

                break;

            case "Light Blue":

            case "Light Blue 1":

            case "Light Blue 2":

                zoneImage.color = blueZone;

                break;

            case "Goal Area":

                zoneImage.color = yellowZone;

                break;

            default:

                zoneImage.color = outZone;

                break;

        }



        Debug.Log("Color changed to: " + zoneImage.color);

    }





    // Optional: For in-editor visualization

    void OnDrawGizmos()

    {

        Gizmos.color = Color.red;

        Gizmos.DrawSphere(new Vector3(lastTargetX, 0.05f, 0), 0.05f);

    }

    public void OnSettingsPressed()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SettingsScene");
    }

}