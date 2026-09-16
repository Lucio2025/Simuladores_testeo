using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float gravity = -20f;
    [SerializeField] private float jumpHeight = 1.5f;

    [Header("Referencias")]
    [SerializeField] private Transform cameraTransform; // opcional, para movimiento relativo a cámara

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        HandleGroundCheck();
        HandleMovement();
        HandleJump();
        ApplyGravity();
    }

    private void HandleGroundCheck()
    {
        isGrounded = controller.isGrounded;

        // Si está en el piso y la velocidad vertical es negativa, la reseteamos
        // para que no acumule gravedad infinita mientras está parado.
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal"); // A/D
        float vertical = Input.GetAxisRaw("Vertical");     // W/S

        Vector3 inputDir = new Vector3(horizontal, 0f, vertical).normalized;

        Vector3 moveDir;
        if (cameraTransform != null)
        {
            // Movimiento relativo a la cámara (ignorando inclinación vertical)
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;
            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            moveDir = camForward * inputDir.z + camRight * inputDir.x;
        }
        else
        {
            // Movimiento relativo al mundo (simple)
            moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;
        }

        controller.Move(moveDir * moveSpeed * Time.deltaTime);
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // Física de salto: v = sqrt(h * -2 * g)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}