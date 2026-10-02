using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject panelPausa;

    public static bool EnPausa { get; private set; }

    private void Update()
    {
        // Detección moderna con Input System
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
        if (panelPausa != null) panelPausa.SetActive(true);
        Time.timeScale = 0f; // Congela la física
        EnPausa = true;
    }

    public void Reanudar()
    {
        Debug.Log("Botón presionado");
        if (panelPausa != null) panelPausa.SetActive(false);
        Time.timeScale = 1f; // Restablece el tiempo
        EnPausa = false;
    }

    public void IrAlMenu()
    {
        Debug.Log("Botón presionado");
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