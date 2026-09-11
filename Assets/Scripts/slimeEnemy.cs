using UnityEngine;

public class SlimeEnemigo : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private Transform controladorSuelo;
    [SerializeField] private float distanciaSuelo = 0.5f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Detección de Pared")]
    [SerializeField] private float distanciaPared = 0.5f; // Aumentamos ligeramente la distancia

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 1. Movimiento constante
        rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);

        // 2. Raycast hacia abajo (para detectar bordes/vacío)
        RaycastHit2D haySuelo = Physics2D.Raycast(controladorSuelo.position, Vector2.down, distanciaSuelo, groundLayer);

        // 3. Raycast hacia adelante (Lanzado desde controladorSuelo para no chocar con el propio Slime)
        Vector2 direccionFrente = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        RaycastHit2D hayPared = Physics2D.Raycast(controladorSuelo.position, direccionFrente, distanciaPared, groundLayer);

        // Dibuja los rayos en la pestaña Scene para visualizarlos
        Debug.DrawRay(controladorSuelo.position, Vector2.down * distanciaSuelo, Color.red);
        Debug.DrawRay(controladorSuelo.position, direccionFrente * distanciaPared, Color.blue);

        // Si se acaba el suelo O si detecta una pared al frente, da la vuelta
        if (haySuelo.collider == false || hayPared.collider == true)
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
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerVida playerVida = collision.gameObject.GetComponent<PlayerVida>();
            if (playerVida != null)
            {
                playerVida.TomarDano(1);
            }
        }
    }
}