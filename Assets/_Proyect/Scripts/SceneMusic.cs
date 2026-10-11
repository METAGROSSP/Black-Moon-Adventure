using UnityEngine;

public class SceneMusic : MonoBehaviour
{
    [Header("Música de esta Escena")]
    [SerializeField] private AudioClip musicaDeEscena;

    private void Start()
    {
        if (AudioManager.Instance != null && musicaDeEscena != null)
        {
            AudioManager.Instance.CambiarMusica(musicaDeEscena);
        }
    }
}