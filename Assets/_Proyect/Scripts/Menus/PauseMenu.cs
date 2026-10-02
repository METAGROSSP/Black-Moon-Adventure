using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // Requerido para Unity Input System Package

public class PauseMenu : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject panelPausa;

    public static bool EnPausa { get; private set; }

    private void OnEnable()
    {
        // Suscripción a eventos si necesitas reaccionar a cambios externos (Patrón Observer)
    }

    private void OnDisable()
    {
        // Garantizar el limpiado de suscripciones para evitar MissingReferenceException
    }

    private void Update()
    {
        // Detección moderna con Input System (remplaza a Input.GetKeyDown)
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (EnPausa)
                Reanudar();
            else
                Pausar();
        }
    }

    public void Pausar()
    {
        panelPausa.SetActive(true);
        Time.timeScale = 0f; // Congela la física y la interpolación por deltaTime
        EnPausa = true;
    }

    public void Reanudar()
    {
        panelPausa.SetActive(false);
        Time.timeScale = 1f; // Restablece la escala de tiempo
        EnPausa = false;
    }

    public void IrAlMenu()
    {
        RestablecerTiempo();
        SceneManager.LoadScene("MainMenu");
    }

    public void ReiniciarNivel()
    {
        RestablecerTiempo();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void RestablecerTiempo()
    {
        Time.timeScale = 1f;
        EnPausa = false;
    }
}