using UnityEngine;

public class SlimeVida : MonoBehaviour
{
    [SerializeField] private int vidaMaxima = 3; // Golpes necesarios para destruirlo
    private int vidaActual;

    void Start()
    {
        vidaActual = vidaMaxima;
    }

    public void RecibirDano(int cantidad)
    {
        vidaActual -= cantidad;
        Debug.Log("¡Slime golpeado! Vida restante: " + vidaActual);

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    private void Morir()
    {
        Debug.Log("¡Slime destruido!");
        Destroy(gameObject);
    }
}