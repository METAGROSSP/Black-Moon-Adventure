using UnityEngine;
using UnityEngine.InputSystem;

public class NewMonoBehaviourScript : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    // Variables de la velocidad y movimiento
    [SerializeField] private float speed = 5f;
    private Vector2 moveinput;

    // Variables del salto
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float jumpForce = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Función mover (Input System)
    public void OnMove(InputValue value)
    {
        moveinput = value.Get<Vector2>();
    }

    // Función salto (Input System)
    public void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            bool estaEnSuelo = Physics2D.OverlapCircle(groundCheck.position, 0.25f, groundLayer);

            if (estaEnSuelo)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
        }
    }
    void Update()
    {
        // 3. Enviamos el valor absoluto de moveinput.x al Animator
        // Usamos Mathf.Abs para que al ir a la izquierda (-1) cambie a positivo (1) y la condición > 0.1 se cumpla.
        if (animator != null)
        {
            animator.SetFloat("Speed", Mathf.Abs(moveinput.x));
        }

        // Voltear el sprite según la dirección
        if (moveinput.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1); // Mirar a la derecha
        }
        else if (moveinput.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // Mirar a la izquierda
        }
    }
    // Las físicas y el movimiento del Rigidbody DEBEN ir en FixedUpdate
    void FixedUpdate()
    {
        // Aplicamos la velocidad horizontal en el Rigidbody.
        // Mantenemos 'rb.linearVelocity.y' intacta para que la gravedad y el salto funcionen solos.
        rb.linearVelocity = new Vector2(moveinput.x * speed, rb.linearVelocity.y);
    }
}