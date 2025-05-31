using UnityEngine;

public class ManualPaddleTrigger : MonoBehaviour
{
    public PaddleHitter paddle;  // Drag your PaddleRoot object with PaddleHitter script here

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (paddle != null)
            {
                paddle.Hit();
                Debug.Log("Paddle triggered manually at: " + Time.time + "s");
            }
            else
            {
                Debug.LogWarning("Paddle reference is not assigned!");
            }
        }
    }
}
