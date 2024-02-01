using UnityEngine;

public class MusicWave : MonoBehaviour
{
    private Rigidbody2D rb;
    private Rigidbody2D parentRb;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        parentRb = transform.GetComponentInParent<Rigidbody2D>();
    }

    private void LateUpdate() {
        rb.velocity = parentRb.velocity;
    }
}
