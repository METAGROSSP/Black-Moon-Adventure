using UnityEngine;

public class PicosDano : MonoBehaviour
{
    [Header("Configuración de Daño")]
    [SerializeField] private int cantidadDano = 1;
    [SerializeField] private float fuerzaEmpujeArriba = 8f; // Knockback vertical para la trampa

    private void OnTriggerEnter2D(Collider2D otro)
    {
        // Verificar si colisiona con el Player (utilizando el Tag estándar)
        if (otro.CompareTag("Player"))
        {
            // 1. Intentar obtener el componente de Vida del jugador (ej. PlayerHealth o SlimeVida)
            var vidaJugador = otro.GetComponent<PlayerVida>();

            if (vidaJugador != null)
            {
                // Aplicar el daño (que internamente actualiza el HUD y dispara el FlashDanio en el UIManager)
                vidaJugador.RecibirDanio(cantidadDano);
            }

            // 2. Aplicar un pequeño empuje (Knockback) en Y para que el jugador rerebote al pisar los pinchos
            var rbJugador = otro.GetComponent<Rigidbody2D>();
            if (rbJugador != null)
            {
                // API actualizada de Unity 6
                rbJugador.linearVelocity = new Vector2(rbJugador.linearVelocity.x, fuerzaEmpujeArriba);
            }
        }
    }
}