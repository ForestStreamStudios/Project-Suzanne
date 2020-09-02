using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10f;
    public float sprintModifier = 1.5f;

    private Vector3 velocity;
    private CharacterController controller;
    private const float gravity = -9.8f;

    // Start is called before the first frame update
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        velocity = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    private void Update()
    {
        velocity.y += gravity * Time.deltaTime;
        if (controller.isGrounded)
        {
            velocity.y = 0;
            if (Input.GetKey(KeyCode.Space))
            {
                velocity.y = 3;
            }
        }

        float verticalSpeed = speed;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            verticalSpeed *= sprintModifier;
        }

        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        Vector3 translation = transform.right * speed * hAxis + transform.forward * verticalSpeed * vAxis;

        controller.Move((translation + velocity) * Time.deltaTime);
    }

}
