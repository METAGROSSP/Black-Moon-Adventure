using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject panelOpciones;
    [SerializeField] private Slider sliderMaster;
    [SerializeField] private Slider sliderMusica;
    [SerializeField] private Slider sliderSFX;

    private void OnEnable()
    {
        // Suscripciones a los eventos de C# (Observer Pattern)
        GameEvents.OnSolicitarOpciones += AbrirPanel;
        GameEvents.OnCerrarOpciones += CerrarPanel;
    }

    private void OnDisable()
    {
        // Desuscripciones obligatorias para evitar referencias nulas
        GameEvents.OnSolicitarOpciones -= AbrirPanel;
        GameEvents.OnCerrarOpciones -= CerrarPanel;
    }

    private void Start()
    {
        // Configuración inicial e inyección de escuchadores (Listeners)
        if (sliderMaster != null)
        {
            sliderMaster.value = PlayerPrefs.GetFloat("MasterVol", 0.75f);
            sliderMaster.onValueChanged.AddListener(SetMasterVolume);
        }

        if (sliderMusica != null)
        {
            sliderMusica.value = PlayerPrefs.GetFloat("MusicVol", 0.75f);
            sliderMusica.onValueChanged.AddListener(SetMusicVolume);
        }

        if (sliderSFX != null)
        {
            sliderSFX.value = PlayerPrefs.GetFloat("SFXVol", 0.75f);
            sliderSFX.onValueChanged.AddListener(SetSFXVolume);
        }
    }

    public void AbrirPanel()
    {
        if (panelOpciones != null)
        {
            // Sincroniza la posición de los sliders con los valores actuales antes de mostrar
            ActualizarPosicionSliders();
            panelOpciones.SetActive(true);
        }
    }

    public void CerrarPanel()
    {
        if (panelOpciones != null)
        {
            panelOpciones.SetActive(false);
        }
    }

    private void ActualizarPosicionSliders()
    {
        if (sliderMaster != null) sliderMaster.value = PlayerPrefs.GetFloat("MasterVol", 0.75f);
        if (sliderMusica != null) sliderMusica.value = PlayerPrefs.GetFloat("MusicVol", 0.75f);
        if (sliderSFX != null) sliderSFX.value = PlayerPrefs.GetFloat("SFXVol", 0.75f);
    }

    public void SetMasterVolume(float value)
    {
        PlayerPrefs.SetFloat("MasterVol", value);
        GameEvents.OnMasterVolumeChanged?.Invoke(value);
    }

    public void SetMusicVolume(float value)
    {
        PlayerPrefs.SetFloat("MusicVol", value);
        GameEvents.OnMusicVolumeChanged?.Invoke(value);
    }

    public void SetSFXVolume(float value)
    {
        PlayerPrefs.SetFloat("SFXVol", value);
        GameEvents.OnSFXVolumeChanged?.Invoke(value);
    }
}