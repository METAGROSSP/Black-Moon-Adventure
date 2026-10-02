using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Variable para guardar el puntaje actual
    public int Puntos { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SumarPuntos(int cantidad)
    {
        Puntos += cantidad; // 1. Acumulamos la cantidad

        // 2. Disparamos el evento para que UIManager lo escuche
        GameEvents.OnPuntosCambiados?.Invoke(Puntos);
    }
}