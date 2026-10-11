using UnityEngine;

public class HitboxAtaque : MonoBehaviour
{
    [Header("Configuración de Ataque")]
    [SerializeField] private int danoAtaque = 1;

    [Header("Feedback de Impacto (Juice)")]
    [SerializeField] private GameObject hitVfxPrefab;
    [SerializeField] private AudioClip golpeSFX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. Verificar si el objeto impactado es un Slime/Enemigo
        if (collision.TryGetComponent<SlimeVida>(out SlimeVida slime))
        {
            // Calculamos el vector de dirección desde el jugador/hitbox hacia el Slime
            Vector2 direccionImpacto = (collision.transform.position - transform.position).normalized;

            // 2. Transmitir el daño junto con la dirección del empuje
            slime.RecibirDano(danoAtaque, direccionImpacto);

            // 3. Feedback Sensorial (VFX de impacto si está asignado)
            if (hitVfxPrefab != null)
            {
                Instantiate(hitVfxPrefab, collision.transform.position, Quaternion.identity); //[cite: 36]
            }

            // 4. Reproducir SFX de golpe usando el AudioManager Singleton
            if (AudioManager.Instance != null && golpeSFX != null)
            {
                AudioManager.Instance.PlaySFX(golpeSFX); //[cite: 46]
            }

            
        }
    }
}