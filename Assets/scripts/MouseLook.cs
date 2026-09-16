using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Header("Sensibilidad")]
    [SerializeField] private float mouseSensitivity = 200f;

    [Header("Referencias")]
    [SerializeField] private Transform cameraPivot; // el Empty CameraPivot

    private float verticalRotation = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotación horizontal: rota todo el player (body)
        transform.Rotate(Vector3.up * mouseX);

        // Rotación vertical: solo rota el pivot de la cámara, clampeada para no dar vueltas raras
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -80f, 80f);
        cameraPivot.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }
}