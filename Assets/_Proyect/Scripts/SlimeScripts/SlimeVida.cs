using System.Collections;
using UnityEngine;

public class SlimeVida : MonoBehaviour
{
    [Header("Sistema de Vida")]
    [SerializeField] private int vidaMaxima = 3;
    [SerializeField] private int puntosAlMorir = 10;

    [Header("Reacción al Golpe (Knockback & Stun)")]
    [SerializeField] private float fuerzaKnockback = 5f;
    [SerializeField] private float tiempoStun = 0.35f;

    [Header("Inmersión Sensorial / Juice (VFX & SFX)")]
    [SerializeField] private GameObject vfxMuertePrefab;
    [SerializeField] private AudioClip muerteSFX;

    private int vidaActual;
    private Rigidbody2D rb;
    private Animator animator;
    private Collider2D col2D;

    public bool EstaStuneado { get; private set; }
    public bool EstaMuerto { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        col2D = GetComponent<Collider2D>();
    }

    private void Start()
    {
        vidaActual = vidaMaxima;
    }

    public void RecibirDano(int cantidad, Vector2 direccionImpacto)
    {
        if (EstaMuerto || vidaActual <= 0) return;

        vidaActual -= cantidad;
        Debug.Log($"Slime golpeado. Vida restante: {vidaActual}");

        if (vidaActual <= 0)
        {
            Morir();
        }
        else
        {
            if (animator != null && TieneParametro("Hurt"))
            {
                animator.SetTrigger("Hurt");
            }
            AplicarKnockback(direccionImpacto);
        }
    }

    private void AplicarKnockback(Vector2 direccion)
    {
        rb.linearVelocity = Vector2.zero;
        Vector2 empuje = (direccion.normalized + Vector2.up * 0.2f).normalized * fuerzaKnockback;
        rb.AddForce(empuje, ForceMode2D.Impulse);
        StartCoroutine(RutinaStun());
    }

    private IEnumerator RutinaStun()
    {
        EstaStuneado = true;
        yield return new WaitForSeconds(tiempoStun);
        if (!EstaMuerto)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            EstaStuneado = false;
        }
    }

    private void Morir()
    {
        EstaMuerto = true;
        EstaStuneado = true;

        // 1. Forzar la reproducción de la animación en el Animator inmediatamente
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        // 2. Apagar colisiones y físicas
        if (col2D != null) col2D.enabled = false;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;

        // 3. Feedback Audiovisual (Juice / Hito 2)
        if (vfxMuertePrefab != null)
        {
            Instantiate(vfxMuertePrefab, transform.position, Quaternion.identity);
        }

        if (AudioManager.Instance != null && muerteSFX != null)
        {
            AudioManager.Instance.PlaySFX(muerteSFX);
        }

        // 4. Sumar Puntos al GameManager (Singleton)
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SumarPuntos(puntosAlMorir);
        }

        // 5. Tiempo suficiente para reproducir los 11 fotogramas (0.95s a 12 FPS)
        Destroy(gameObject, 0.95f);
    }

    private bool TieneParametro(string nombreParametro)
    {
        if (animator == null) return false;
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == nombreParametro) return true;
        }
        return false;
    }
}