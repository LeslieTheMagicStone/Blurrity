using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 1f;
    public float rotationSpeed = 3f;
    private CharacterController characterController;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Vector3 movement = new(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        var speedUpFactor = Input.GetKey(KeyCode.LeftShift) ? 2 : 1;

        transform.Translate(speed * speedUpFactor * Time.deltaTime * movement, Space.Self);

        if (!Input.GetKey(KeyCode.LeftAlt))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float rotationAmount = mouseX * rotationSpeed;

            transform.Rotate(Vector3.up, rotationAmount);
        }
    }
}
