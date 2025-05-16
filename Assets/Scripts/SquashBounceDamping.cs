using UnityEngine;

public class SquashBounceDamping : MonoBehaviour
{
    private Rigidbody rb;

    void Start(){
        rb = GetComponent<Rigidbody>();
    }
    void OnCollisionEnter(Collision collision){
        rb.linearVelocity *=0.85f;
    }
}
