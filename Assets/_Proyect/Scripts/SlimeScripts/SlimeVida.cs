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
        // Opcional: Sumar puntos al GameManager si está presente
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SumarPuntos(puntosAlMorir);
        }

        Debug.Log("¡Slime destruido!");
        Destroy(gameObject);
    }
}