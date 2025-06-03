using UnityEngine;



public class BallDropper : MonoBehaviour

{

    public GameObject ballPrefab;

    public Transform dropPoint;

    public UIManager uiManager;

    public GameObject markerPrefab;



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

            float angle = uiManager.lastPredictedAngle;

            float speed = uiManager.lastPredictedVelocity;



            float verticalAngle = angle;

            float horizontalPhi = 22f;



            Quaternion verticalRot = Quaternion.Euler(-verticalAngle, 0, 0);

            Quaternion horizontalRot = Quaternion.Euler(0, horizontalPhi, 0);



            Vector3 launchDir = horizontalRot * (verticalRot * Vector3.forward);

            rb.linearVelocity = launchDir.normalized * speed;



            launched = true;



            if (uiManager.timeTracker != null && uiManager.timeTracker.paddle != null)

                uiManager.timeTracker.paddle.Hit();



            // Marker for debug

            if (markerPrefab != null)

            {

                Vector3 markerPos = dropPoint.position + launchDir.normalized * uiManager.lastTargetX;

                markerPos.y = 0.05f;

                Instantiate(markerPrefab, markerPos, Quaternion.identity);

            }



            Debug.Log($"Launched at t={timeSinceDrop:F2}s → Angle={angle:F1}°, Speed={speed:F2}");

        }

    }

}