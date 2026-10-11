using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    // Patrón Singleton para acceso global
    public static UIManager Instance { get; private set; }

    [Header("Referencias de UI (HUD)")]
    [SerializeField] private Image barraVida;
    [SerializeField] private TMP_Text textoPuntos;

    [Header("Feedback Visual de Daño (Juice)")]
    [SerializeField] private Image overlayDanio;
    [SerializeField] private float duracionFlash = 0.2f;
    [SerializeField] private float alphaMaximo = 0.4f; // Intensidad del destello rojo (0 a 1)

    private Coroutine rutinaFlash;
    private int ultimaVida; // Guarda el valor previo para verificar si la vida bajó (Daño)

    private void Awake()
    {
        // Implementación del Patrón Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnEnable()
    {
        // Suscripción al canal de eventos (Patrón Observer)
        GameEvents.OnVidaCambiada += ActualizarVida;
        GameEvents.OnPuntosCambiados += ActualizarPuntos;
    }

    private void OnDisable()
    {
        // Desuscripción obligatoria para evitar Memory Leaks y MissingReferenceException
        GameEvents.OnVidaCambiada -= ActualizarVida;
        GameEvents.OnPuntosCambiados -= ActualizarPuntos;
    }

    private void Start()
    {
        // Muestra los puntos iniciales (0) al arrancar la escena desde el GameManager
        if (GameManager.Instance != null)
        {
            ActualizarPuntos(GameManager.Instance.Puntos);
        }
    }

    private void ActualizarVida(int vidaActual, int vidaMaxima)
    {
        if (barraVida != null && vidaMaxima > 0)
        {
            // Modifica el rellenado de la imagen (Image Type debe ser Filled)
            barraVida.fillAmount = (float)vidaActual / vidaMaxima;
        }

        // Disparar el destello rojo en la UI solo si la vida es menor que la anterior
        if (vidaActual < ultimaVida)
        {
            MostrarFlashDanio();
        }

        ultimaVida = vidaActual; // Actualizar variable de control
    }

    private void ActualizarPuntos(int puntos)
    {
        if (textoPuntos != null)
        {
            // Formatea los puntos a 4 dígitos (ejemplo: 0005)
            textoPuntos.text = puntos.ToString("D4");
        }
    }

    public void MostrarFlashDanio()
    {
        if (overlayDanio == null) return;

        if (rutinaFlash != null)
        {
            StopCoroutine(rutinaFlash);
        }

        rutinaFlash = StartCoroutine(RutinaFlashOverlay());
    }

    private IEnumerator RutinaFlashOverlay()
    {
        float tiempo = 0f;
        Color colorBase = overlayDanio.color;

        // Se usa Time.unscaledDeltaTime para que funcione aun si hay Hitstop (Time.timeScale = 0)
        while (tiempo < duracionFlash)
        {
            tiempo += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(alphaMaximo, 0f, tiempo / duracionFlash);
            overlayDanio.color = new Color(colorBase.r, colorBase.g, colorBase.b, alpha);
            yield return null;
        }

        // Garantiza transparencia total al terminar la corrutina
        overlayDanio.color = new Color(colorBase.r, colorBase.g, colorBase.b, 0f);
    }
}