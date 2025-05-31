using UnityEngine;

public class BallDropper : MonoBehaviour
{
    public GameObject ballPrefab;
    public Transform dropPoint;
    public UIManager uiManager;

    private GameObject currentBall;
    private Rigidbody rb;
    private bool launched = false;
    private float timeSinceDrop = 0f;
    public float launchDelay = 0.589f;

    public void DropBall()
    {
        if (ballPrefab == null || dropPoint == null) return;

        if (currentBall != null)
            Destroy(currentBall);

        currentBall = Instantiate(ballPrefab, dropPoint.position, Quaternion.identity);
        rb = currentBall.GetComponent<Rigidbody>();
        rb.useGravity = true;
        launched = false;
        timeSinceDrop = 0f;
    }

    void Update()
    {
        if (currentBall == null || launched || rb == null) return;

        timeSinceDrop += Time.deltaTime;

        if (timeSinceDrop >= launchDelay)
        {
            // Launch horizontally in X-Y plane
            float angle = uiManager.lastPredictedAngle;
            float speed = uiManager.lastPredictedVelocity;

            Vector3 launchDir = Quaternion.Euler(0, 0, -angle) * Vector3.right; // launch in X
            rb.linearVelocity = launchDir.normalized * speed;
            launched = true;

            Debug.Log($"Launched at t={timeSinceDrop:F2}s → Angle={angle:F1}°, Speed={speed:F2}");
        }
    }
}
