using UnityEngine;
using UnityEngine.UI;
using TMPro; // Asegúrate de tener TextMeshPro importado

public class UIManager : MonoBehaviour
{
    [Header("Referencias de UI")]
    [SerializeField] private Image barraVida;
    [SerializeField] private TMP_Text textoPuntos;

    private void OnEnable()
    {
        // Suscribirse a los eventos del canal GameEvents
        GameEvents.OnVidaCambiada += ActualizarVida;
        GameEvents.OnPuntosCambiados += ActualizarPuntos;
    }

    private void OnDisable()
    {
        // Desuscribirse SIEMPRE para evitar fugas de memoria o errores nulos
        GameEvents.OnVidaCambiada -= ActualizarVida;
        GameEvents.OnPuntosCambiados -= ActualizarPuntos;
    }

    private void ActualizarVida(int vidaActual, int vidaMaxima)
    {
        if (barraVida != null && vidaMaxima > 0)
        {
            // Modifica el rellenado de la imagen (Image Type debe ser Filled)
            barraVida.fillAmount = (float)vidaActual / vidaMaxima;
        }
    }

    private void ActualizarPuntos(int puntos)
    {
        if (textoPuntos != null)
        {
            // Formatea los puntos (ejemplo: 0005) o simplemente puntos.ToString()
            textoPuntos.text = puntos.ToString("D4");
        }
    }
}