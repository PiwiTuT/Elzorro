using UnityEngine;

public class CharacterDeath : MonoBehaviour
{
    public GameObject deathEffect; // Efecto de muerte, como una animación o partícula

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.magnitude > 5) // Ajusta el valor según la fuerza del impacto
        {
            Die();
        }
    }

    void Die()
    {
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject); // Destruye el objeto del personaje
    }
}
