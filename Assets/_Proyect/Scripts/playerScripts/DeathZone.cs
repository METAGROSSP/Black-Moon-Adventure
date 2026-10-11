using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathZone : MonoBehaviour
{
    [SerializeField] private AudioClip muerteSFX;

    private void OnTriggerEnter2D(Collider2D otro)
    {
        // Verifica si el objeto que cae tiene el Tag "Player"
        if (otro.CompareTag("Player"))
        {
            // Reproducir sonido con el AudioManager persitente
            if (AudioManager.Instance != null && muerteSFX != null)
            {
                AudioManager.Instance.PlaySFX(muerteSFX);
            }

            // Restablecer la velocidad del tiempo por si veníamos de Pausa
            Time.timeScale = 1f;

            // Recargar la escena actual de forma limpia
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}