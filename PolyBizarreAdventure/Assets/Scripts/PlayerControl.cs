using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerControl : MonoBehaviour
{
    public float speed = 5f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private Animator animator;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>(); // Asume que el personaje tiene Animator
    }

    void Update()
    {
        // Detectar si está en el suelo
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            animator.SetBool("IsJumping", false); // ya está en el suelo
        }

        // Movimiento horizontal
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        // Animación: velocidad absoluta
        float currentSpeed = new Vector2(x, z).magnitude;
        animator.SetFloat("Speed", currentSpeed);

        // Saltar
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetBool("IsJumping", true);
        }

        // Aplicar gravedad
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
