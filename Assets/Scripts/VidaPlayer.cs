using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerVida : MonoBehaviour
{
    [Header("Sistema de Vida")]
    [SerializeField] private int vidaMaxima = 15;
    private int vidaActual;

    [Header("Inmunidad Temporal")]
    [SerializeField] private float tiempoInvulnerable = 0.5f; 
    private float cooldownInmunidad;

    void Start()
    {
        vidaActual = vidaMaxima;
    }

    void Update()
    {
        if (cooldownInmunidad > 0)
        {
            cooldownInmunidad -= Time.deltaTime;
        }
    }

    public void TomarDano(int cantidad)
    {
        // Solo recibe daño si ya pasó el tiempo de inmunidad
        if (cooldownInmunidad <= 0)
        {
            vidaActual -= cantidad;
            cooldownInmunidad = tiempoInvulnerable;

            Debug.Log("Vida restante: " + vidaActual);

            if (vidaActual <= 0)
            {
                // Reinicia la escena al llegar a 0 vidas (15 toques)
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}