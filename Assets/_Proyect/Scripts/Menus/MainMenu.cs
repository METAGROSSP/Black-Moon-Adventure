using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Carga la escena del juego asegurando tiempo a 1 (1.0f)
    public void Jugar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level_01"); // Debe estar registrado en Build Profiles
    }

    // Notifica que se pulsó el botón sin necesidad de guardar referencias al panel en este script
    public void AbrirOpciones()
    {
        GameEvents.OnSolicitarOpciones?.Invoke();
    }

    // Permite probar el botón Salir tanto en la Build final como en el Editor de Unity 6
    public void Salir()
    {
        Debug.Log("Saliendo del juego...");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}