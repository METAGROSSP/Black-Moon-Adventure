using UnityEngine;

public class HitboxAtaque : MonoBehaviour
{
    [SerializeField] private int danoAtaque = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<SlimeVida>(out SlimeVida slime))
        {
            slime.RecibirDano(danoAtaque);
        }
    }
}