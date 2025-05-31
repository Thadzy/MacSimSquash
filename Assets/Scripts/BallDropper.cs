using UnityEngine;
using TMPro;

public class BallDropper : MonoBehaviour
{
    public GameObject ballPrefab;
    public Transform dropPoint;
    public UIManager uiManager;
    public TextMeshProUGUI dropPromptUI;

    private bool playerInRange = false;

    public GameObject LaunchBall()
    {
        if (ballPrefab == null || dropPoint == null || uiManager == null) return null;

        GameObject ball = Instantiate(ballPrefab, dropPoint.position, Quaternion.identity);
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        if (rb != null)
        {
            float angle = uiManager.lastPredictedAngle;
            float velocity = uiManager.lastPredictedVelocity;
            Vector3 dir = Quaternion.Euler(-angle, 0f, 0f) * Vector3.forward;

            rb.linearVelocity = dir.normalized * velocity;
        }
        return ball;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }

    void Update()
    {
        if (dropPromptUI != null)
            dropPromptUI.enabled = playerInRange;

        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            LaunchBall();
            dropPromptUI.enabled = false;
        }
    }
}
