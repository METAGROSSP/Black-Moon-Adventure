using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Música de Fondo")]
    [SerializeField] private AudioSource musicaSource;
    [SerializeField] private AudioClip musicaDeFondo;

    [Header("Efectos de Sonido (SFX)")]
    [SerializeField] private AudioSource sfxSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (musicaDeFondo != null && musicaSource != null)
        {
            musicaSource.clip = musicaDeFondo;
            musicaSource.loop = true;
            musicaSource.Play();
        }
    }

    // Método para reproducir sonidos de un solo disparo (como el espadazo)
    public void ReproducirSFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}