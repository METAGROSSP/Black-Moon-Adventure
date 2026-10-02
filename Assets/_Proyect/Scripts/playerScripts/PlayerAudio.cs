using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [Header("Audios de Jugador")]
    [SerializeField] private AudioClip sonidoAtaque;

    private void OnEnable()
    {
        GameEvents.OnPlayerAtaque += ReproducirAtaque;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerAtaque -= ReproducirAtaque;
    }

    private void ReproducirAtaque()
    {
        if (AudioManager.Instance != null && sonidoAtaque != null)
        {
            AudioManager.Instance.ReproducirSFX(sonidoAtaque);
        }
    }
}