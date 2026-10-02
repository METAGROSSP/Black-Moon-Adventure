using UnityEngine;

public class SlimeEnemigo : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private Transform controladorSuelo;
    [SerializeField] private float distanciaSuelo = 0.5f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Detección de Pared")]
    [SerializeField] private float distanciaPared = 0.5f;

    [Header("Ataque")]
    [SerializeField] private int danoContacto = 1;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (Time.timeScale == 0f) return; // Pausa el movimiento en menú de pausa

        // 1. Movimiento constante usando linearVelocity (Unity 6)
        rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);

        // 2. Detección de bordes y paredes con Raycasts
        RaycastHit2D haySuelo = Physics2D.Raycast(controladorSuelo.position, Vector2.down, distanciaSuelo, groundLayer);
        Vector2 direccionFrente = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        RaycastHit2D hayPared = Physics2D.Raycast(controladorSuelo.position, direccionFrente, distanciaPared, groundLayer);

        // Visualización de rayos en Scene
        Debug.DrawRay(controladorSuelo.position, Vector2.down * distanciaSuelo, Color.red);
        Debug.DrawRay(controladorSuelo.position, direccionFrente * distanciaPared, Color.blue);

        // Cambiar de dirección si llega a un abismo o choca contra pared
        if (!haySuelo.collider || hayPared.collider)
        {
            Girar();
        }
    }

    private void Girar()
    {
        speed *= -1;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Uso de TryGetComponent para evitar GC Alloc y llamadas nulas
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.TryGetComponent<PlayerVida>(out PlayerVida playerVida))
        {
            playerVida.RecibirDanio(danoContacto);
        }
    }
}