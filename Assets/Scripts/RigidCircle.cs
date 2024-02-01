using UnityEditor.UI;
using UnityEngine;

public class RigidCircle : MonoBehaviour
{
    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            rb.velocity = other.relativeVelocity * 0.5f;
        }
        else
        {
            rb.velocity = other.relativeVelocity;
        }
    }
}
