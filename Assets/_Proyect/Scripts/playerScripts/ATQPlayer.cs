using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAtaque : MonoBehaviour
{
    [Header("Configuración de Ataque")]
    [SerializeField] private GameObject hitboxAtaque;
    [SerializeField] private float tiempoAtaque = 0.2f;
    [SerializeField] private float cooldownAtaque = 0.5f;

    private bool puedeAtacar = true;
    private Animator animator;
    private Coroutine rutinaAtaque;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        if (hitboxAtaque != null)
        {
            hitboxAtaque.SetActive(false);
        }
    }

    private void Update()
    {
        if (Time.timeScale == 0f) return;

        if (Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame && puedeAtacar)
        {
            EjecutarAtaque();
        }
    }

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if (context.started && puedeAtacar && Time.timeScale > 0f)
        {
            EjecutarAtaque();
        }
    }

    private void EjecutarAtaque()
    {
        if (rutinaAtaque != null) StopCoroutine(rutinaAtaque);
        rutinaAtaque = StartCoroutine(RutinaAtaqueCo());
    }

    private IEnumerator RutinaAtaqueCo()
    {
        puedeAtacar = false;

        // Lanza evento global (El AudioManager o PlayerAudio responderán a esto)
        GameEvents.OnPlayerAtaque?.Invoke();

        if (animator != null)
        {
            animator.SetTrigger("Atacar");
        }

        if (hitboxAtaque != null)
        {
            hitboxAtaque.SetActive(true);
        }

        yield return new WaitForSeconds(tiempoAtaque);

        if (hitboxAtaque != null)
        {
            hitboxAtaque.SetActive(false);
        }

        float cooldownRestante = Mathf.Max(0f, cooldownAtaque - tiempoAtaque);
        yield return new WaitForSeconds(cooldownRestante);

        puedeAtacar = true;
    }

    private void OnDisable()
    {
        if (rutinaAtaque != null)
        {
            StopCoroutine(rutinaAtaque);
            puedeAtacar = true;
            if (hitboxAtaque != null) hitboxAtaque.SetActive(false);
        }
    }
}