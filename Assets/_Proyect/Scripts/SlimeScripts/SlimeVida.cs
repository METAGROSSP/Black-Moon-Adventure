using UnityEngine;

public class SlimeVida : MonoBehaviour
{
    [Header("Sistema de Vida")]
    [SerializeField] private int vidaMaxima = 3;
    [SerializeField] private int puntosAlMorir = 10;

    private int vidaActual;

    private void Start()
    {
        vidaActual = vidaMaxima;
    }

    public void RecibirDano(int cantidad)
    {
        vidaActual -= cantidad;
        Debug.Log($"Slime golpeado. Vida restante: {vidaActual}");

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    private void Morir()
    {
        // 1. Notificar al GameManager PRIMERO
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SumarPuntos(puntosAlMorir);
        }
        else
        {
            Debug.LogWarning("No se encontró una instancia de GameManager en la escena.");
        }

        Debug.Log("¡Slime destruido!");

        // 2. Destruir el GameObject AL FINAL
        Destroy(gameObject);
    }
}