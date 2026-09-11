using UnityEngine;
using UnityEngine.InputSystem; // Importante añadir esto arriba

public class PlayerAtaque : MonoBehaviour
{
    [Header("Configuración de Ataque")]
    [SerializeField] private GameObject hitboxAtaque;
    [SerializeField] private float tiempoAtaque = 0.2f;
    [SerializeField] private float cooldownAtaque = 0.5f;

    private bool puedeAtacar = true;
    private Animator animator;
    [Header("Audio")]
    [SerializeField] private AudioClip sonidoAtaque;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        if (hitboxAtaque != null)
        {
            hitboxAtaque.SetActive(false);
        }
    }

    void Update()
    {
        // Detecta la tecla Z usando el nuevo Input System
        if (Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame && puedeAtacar)
        {
            Atacar();
        }
    }

    private void Atacar()
    {
        puedeAtacar = false;
        if (AudioManager.Instance != null && sonidoAtaque != null)
        {
            AudioManager.Instance.ReproducirSFX(sonidoAtaque);
        }

        if (animator != null)
        {
            animator.SetTrigger("Atacar");
        }

        if (hitboxAtaque != null)
        {
            hitboxAtaque.SetActive(true);
        }

        Invoke(nameof(DesactivarHitbox), tiempoAtaque);
        Invoke(nameof(ResetearAtaque), cooldownAtaque);
    }

    private void DesactivarHitbox()
    {
        hitboxAtaque.SetActive(false);
    }

    private void ResetearAtaque()
    {
        puedeAtacar = true;
    }
}