using UnityEngine;
using TMPro;

public class BallDropper : MonoBehaviour
{
    [Header("Ball Drop Settings")]
    public GameObject ballPrefab;

    [Tooltip("Fixed drop position for the ball.")]
    private Vector3 dropPosition = new Vector3(-3.087f, 2.0542f, -1.37f);

    [Header("UI Settings")]
    public TextMeshProUGUI dropPromptUI; // Assign in Inspector

    private bool playerInRange = false;

    void Start()
    {
        if (dropPromptUI != null)
        {
            dropPromptUI.enabled = false;
        }
    }

    void Update()
    {
        if (playerInRange)
        {
            if (dropPromptUI != null)
                dropPromptUI.enabled = true;

            if (Input.GetKeyDown(KeyCode.E))
            {
                DropBall();

                // Optional: hide prompt after drop
                if (dropPromptUI != null)
                    dropPromptUI.enabled = false;
            }
        }
        else
        {
            if (dropPromptUI != null)
                dropPromptUI.enabled = false;
        }
    }

    void DropBall()
    {
        if (ballPrefab != null)
        {
            Instantiate(ballPrefab, dropPosition, Quaternion.identity);
            Debug.Log("✅ Ball Dropped at: " + dropPosition);
        }
        else
        {
            Debug.LogWarning("⚠️ Ball Prefab is not assigned!");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
