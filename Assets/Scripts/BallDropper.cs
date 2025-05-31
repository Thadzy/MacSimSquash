using UnityEngine;
using TMPro;

public class BallDropper : MonoBehaviour
{
    [Header("Ball Drop Settings")]
    public GameObject ballPrefab;

    [Tooltip("Drop position for the ball")]
    public Transform dropPoint;

    [Header("UI Settings")]
    public TextMeshProUGUI dropPromptUI;

    private bool playerInRange = false;

    void Start()
    {
        if (dropPromptUI != null)
            dropPromptUI.enabled = false;
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

    public GameObject DropBall()
    {
        if (ballPrefab != null && dropPoint != null)
        {
            GameObject newBall = Instantiate(ballPrefab, dropPoint.position, Quaternion.identity);
            Debug.Log("Ball dropped at " + dropPoint.position);
            return newBall;
        }
        else
        {
            Debug.LogWarning("Ball prefab or dropPoint is not assigned.");
            return null;
        }
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
}
