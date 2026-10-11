using UnityEngine;

public class PlayerVida : MonoBehaviour
{
    [Header("Configuración de Vida")]
    [SerializeField] private int vidaMaxima = 100;
    private int vidaActual;

    [Header("Inmunidad Temporal")]
    [SerializeField] private float tiempoInvulnerable = 0.5f;
    private float cooldownInmunidad;

    [Header("Referencias de Feedback")]
    [SerializeField] private AudioClip muerteSFX;

    private Animator animator;
    public bool EstaMuerto { get; private set; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        vidaActual = vidaMaxima;
        EstaMuerto = false;
        GameEvents.OnVidaCambiada?.Invoke(vidaActual, vidaMaxima);
    }

    private void Update()
    {
        if (cooldownInmunidad > 0)
        {
            cooldownInmunidad -= Time.deltaTime;
        }
    }

    public void RecibirDanio(int cantidad)
    {
        if (EstaMuerto || cooldownInmunidad > 0) return;

        vidaActual = Mathf.Max(0, vidaActual - cantidad);
        cooldownInmunidad = tiempoInvulnerable;

        GameEvents.OnVidaCambiada?.Invoke(vidaActual, vidaMaxima);

        if (vidaActual <= 0)
        {
            ProcesarMuerte();
        }
    }

    private void ProcesarMuerte()
    {
        EstaMuerto = true;

        // 1. Disparar el Trigger en la Máquina de Estados (Animator)
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        // 2. Reproducir sonido de muerte mediante AudioManager Singleton
        if (AudioManager.Instance != null && muerteSFX != null)
        {
            AudioManager.Instance.PlaySFX(muerteSFX);
        }

        // 3. Notificar a los suscriptores por el patrón Observer
        GameEvents.OnJugadorMuere?.Invoke();
    }
}