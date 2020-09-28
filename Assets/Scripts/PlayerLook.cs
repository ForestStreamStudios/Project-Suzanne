using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Transform cameraTransform;
    public float sensitivity = 0f;

    private float xRotation = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        xRotation = Mathf.Clamp(xRotation - mouseY, -90f, 90f);

        transform.Rotate(0, mouseX, 0);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }
}
