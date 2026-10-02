using UnityEngine;
using UnityEngine.Audio; // Requerido para trabajar con AudioMixer

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Configuración de AudioMixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Fuentes de Audio (AudioSources)")]
    [SerializeField] private AudioSource musicaSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Música de Fondo")]
    [SerializeField] private AudioClip musicaDeFondo;

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

    private void Start()
    {
        if (musicaDeFondo != null && musicaSource != null)
        {
            musicaSource.clip = musicaDeFondo;
            musicaSource.loop = true;
            musicaSource.Play();
        }
    }

    // Método desacoplado para efectos de sonido (SFX)
    public void ReproducirSFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    // Métodos para controlar volúmenes (conectables a Sliders de la UI)
    public void SetMasterVolume(float sliderValue)
    {
        // Convertimos el valor lineal del Slider (0.0001 a 1) a escala logarítmica (dB)
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20);
    }

    public void SetMusicVolume(float sliderValue)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20);
    }

    public void SetSFXVolume(float sliderValue)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20);
    }
}