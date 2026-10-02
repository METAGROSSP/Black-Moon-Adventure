using UnityEngine;

public class PlayerVida : MonoBehaviour
{
    [Header("Configuración de Vida")]
    [SerializeField] private int vidaMaxima = 100;
    private int vidaActual;

    [Header("Inmunidad Temporal")]
    [SerializeField] private float tiempoInvulnerable = 0.5f;
    private float cooldownInmunidad;

    private void Start()
    {
        vidaActual = vidaMaxima;
        // Notificamos el estado inicial para inicializar el HUD/UI[cite: 9]
        GameEvents.OnVidaCambiada?.Invoke(vidaActual, vidaMaxima);
    }

    private void Update()
    {
        if (cooldownInmunidad > 0)
        {
            cooldownInmunidad -= Time.deltaTime;
        }
    }

    public void RecibirDanio(int cantidad)
    {
        // Validación de invulnerabilidad
        if (cooldownInmunidad > 0) return;

        vidaActual = Mathf.Max(0, vidaActual - cantidad);
        cooldownInmunidad = tiempoInvulnerable;

        // Disparar evento Observer para UI, Audio y Partículas[cite: 9, 11, 19]
        GameEvents.OnVidaCambiada?.Invoke(vidaActual, vidaMaxima);

        if (vidaActual <= 0)
        {
            GameEvents.OnJugadorMuere?.Invoke();
        }
    }
}