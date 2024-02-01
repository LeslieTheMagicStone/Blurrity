using UnityEngine;

public class MonoMagnet : MonoBehaviour
{
    public Rigidbody2D target;
    public float force;
    public float distanceThreshold;
    public bool freezeXpos, freezeYpos;

    private void Update()
    {
        var deltaPos = target.transform.position - transform.position;
        var direction = deltaPos.normalized;

        if(freezeXpos) direction.x = 0;
        if(freezeYpos) direction.y = 0;

        var distance = deltaPos.magnitude;
        if (distance < distanceThreshold) return;
        target.AddForce(direction * force / Mathf.Pow(distance, 2));
    }
}
