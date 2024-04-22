using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target; // The object the camera will follow
    public float mouseSensitivity = 10f; // Sensitivity of mouse movement
    public float distanceFromTarget = 2f; // Distance the camera should maintain from the target
    public Vector2 pitchMinMax = new Vector2(-40, 85); // Minimum and maximum pitch angles of the camera

    private float yaw; // Horizontal rotation
    private float pitch; // Vertical rotation

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
    }

    void Update()
    {
        // Get mouse input
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        // Calculate desired rotation
        Vector3 targetRotation = new Vector3(pitch, yaw);
        target.rotation = Quaternion.Euler(0f, targetRotation.y, 0f); // Rotate the player horizontally
        transform.rotation = Quaternion.Euler(targetRotation.x, targetRotation.y, 0f); // Rotate the camera vertically

        // Set camera position behind the target
        transform.position = target.position - transform.forward * distanceFromTarget;
    }
}
