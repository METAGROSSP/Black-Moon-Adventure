using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine; // En Unity 6 se utiliza la librería Unity.Cinemachine

public class PlayerMove : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;

    [Header("Detección de Suelo")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float radioDeteccion = 0.25f;

    [Header("Efectos de Movimiento (VFX)")]
    [SerializeField] private ParticleSystem polvoParticles;

    [Header("Game Feel / Screen Shake (Impulse)")]
    [SerializeField] private float fuerzaImpactoCaida = 0.05f; // Fuerza sutil para no marear al jugador

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveInput;
    private ParticleSystem.EmissionModule polvoEmission;
    private PlayerVida playerVida;

    // Referencias para el seguimiento del estado de suelo e impulso
    private CinemachineImpulseSource impulseSource;
    private bool estabaEnSuelo;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerVida = GetComponent<PlayerVida>();

        // Referencia al emisor de impulsos adjunto al personaje
        impulseSource = GetComponent<CinemachineImpulseSource>();

        if (polvoParticles != null)
        {
            polvoEmission = polvoParticles.emission;
        }
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (Time.timeScale == 0f) return;

        if (value.isPressed)
        {
            bool estaEnSuelo = Physics2D.OverlapCircle(groundCheck.position, radioDeteccion, groundLayer);
            if (estaEnSuelo)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
        }
    }

    private void Update()
    {
        if (Time.timeScale == 0f) return;

        if (playerVida != null && playerVida.EstaMuerto)
        {
            moveInput = Vector2.zero;
            if (polvoParticles != null) polvoEmission.enabled = false;
            return;
        }

        bool estaEnSuelo = Physics2D.OverlapCircle(groundCheck.position, radioDeteccion, groundLayer);

        // --- DETECCIÓN DE ATERRIZAJE (SCREEN SHAKE) ---
        // Si en el fotograma anterior estaba flotando/cayendo y en este toca suelo
        if (!estabaEnSuelo && estaEnSuelo)
        {
            DispararTemblorCaida();
        }
        estabaEnSuelo = estaEnSuelo; // Guardar estado para el siguiente fotograma

        // Control de partículas de movimiento (polvo)
        if (polvoParticles != null)
        {
            bool seEstaMoviendo = Mathf.Abs(moveInput.x) > 0.01f;
            polvoEmission.enabled = seEstaMoviendo && estaEnSuelo;
        }

        // Animaciones de movimiento
        if (animator != null)
        {
            animator.SetFloat("Speed", Mathf.Abs(moveInput.x));
        }

        // Voltear el sprite según la dirección
        if (moveInput.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveInput.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void DispararTemblorCaida()
    {
        if (impulseSource != null)
        {
            // Genera la sacudida basada en las propiedades (Bump / Velocity Y = -1) configuradas en el Inspector
            impulseSource.GenerateImpulse(fuerzaImpactoCaida);
        }
    }

    private void FixedUpdate()
    {
        if (playerVida != null && playerVida.EstaMuerto)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        rb.linearVelocity = new Vector2(moveInput.x * speed, rb.linearVelocity.y);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, radioDeteccion);
        }
    }
}