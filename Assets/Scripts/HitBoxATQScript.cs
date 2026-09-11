using UnityEngine;

public class HitboxAtaque : MonoBehaviour
{
    [SerializeField] private int danoAtaque = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica si chocó contra el Slime
        SlimeVida slime = collision.GetComponent<SlimeVida>();
        if (slime != null)
        {
            slime.RecibirDano(danoAtaque);
        }
    }
}