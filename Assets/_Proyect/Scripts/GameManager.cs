using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Audio de Estado")]
    [SerializeField] private AudioClip muerteSFX;

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

    private void OnEnable()
    {
        // Regla de arquitectura: Suscripción a eventos del sistema
        GameEvents.OnJugadorMuere += ManejarMuerteJugador;
    }

    private void OnDisable()
    {
        // Regla de arquitectura: Cancelar suscripción siempre para evitar MissingReferenceException
        GameEvents.OnJugadorMuere -= ManejarMuerteJugador;
    }

    public void SumarPuntos(int cantidad)
    {
        Puntos += cantidad; // 1. Acumulamos la cantidad
        // 2. Disparamos el evento para que UIManager lo escuche
        GameEvents.OnPuntosCambiados?.Invoke(Puntos);
    }

    private void ManejarMuerteJugador()
    {
        // 1. Reproducir SFX de muerte mediante AudioManager Singleton
        if (AudioManager.Instance != null && muerteSFX != null)
        {
            AudioManager.Instance.PlaySFX(muerteSFX);
        }

        // 2. Reiniciar el nivel tras un pequeño retardo para permitir escuchar el SFX y ver el Game Feel
        Invoke(nameof(ReiniciarNivel), 1.5f);
    }

    private void ReiniciarNivel()
    {
        // Asegurar que el tiempo vuelva a la normalidad si el juego venía de Pausa
        Time.timeScale = 1f;

        // Recargar la escena activa de forma limpia
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}