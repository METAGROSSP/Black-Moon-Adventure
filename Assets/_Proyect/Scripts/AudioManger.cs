using UnityEngine;
using UnityEngine.Audio;

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

    private void OnEnable()
    {
        GameEvents.OnMasterVolumeChanged += SetMasterVolume;
        GameEvents.OnMusicVolumeChanged += SetMusicVolume;
        GameEvents.OnSFXVolumeChanged += SetSFXVolume;
    }

    private void OnDisable()
    {
        GameEvents.OnMasterVolumeChanged -= SetMasterVolume;
        GameEvents.OnMusicVolumeChanged -= SetMusicVolume;
        GameEvents.OnSFXVolumeChanged -= SetSFXVolume;
    }

    private void Start()
    {
        if (musicaDeFondo != null && musicaSource != null)
        {
            musicaSource.clip = musicaDeFondo;
            musicaSource.loop = true;
            musicaSource.Play();
        }

        // Cargar y aplicar volúmenes guardados al iniciar
        CargarVolumenes();
    }

    private void CargarVolumenes()
    {
        SetMasterVolume(PlayerPrefs.GetFloat("MasterVol", 0.75f));
        SetMusicVolume(PlayerPrefs.GetFloat("MusicVol", 0.75f));
        SetSFXVolume(PlayerPrefs.GetFloat("SFXVol", 0.75f));
    }

    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            // Pitch aleatorio para evitar fatiga auditiva (Game Feel profesional)
            sfxSource.pitch = Random.Range(0.9f, 1.1f);
            sfxSource.PlayOneShot(clip);
        }
    }

    public void CambiarMusica(AudioClip nuevaMusica)
    {
        // Si no hay nueva música o si es la misma que ya está sonando, no hace nada
        if (nuevaMusica == null || (musicaSource.clip == nuevaMusica && musicaSource.isPlaying))
            return;

        musicaSource.Stop();
        musicaSource.clip = nuevaMusica;
        musicaSource.Play();
    }

    public void SetMasterVolume(float sliderValue)
    {
        if (audioMixer != null)
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20f);
    }

    public void SetMusicVolume(float sliderValue)
    {
        if (audioMixer != null)
            audioMixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20f);
    }

    public void SetSFXVolume(float sliderValue)
    {
        if (audioMixer != null)
            audioMixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20f);
    }
}