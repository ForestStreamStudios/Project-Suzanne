using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Transform cameraTransform;

    private float xRotation = 0f;

    private void Start()
    {
        
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        xRotation = Mathf.Clamp(xRotation - mouseY, -90f, 90f);

        transform.Rotate(0, mouseX, 0);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }
}
