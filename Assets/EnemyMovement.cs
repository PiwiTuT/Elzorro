using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyMovement : MonoBehaviour
{
    public float speed = 2f; // Velocidad del enemigo
    public Transform pointA; // Primer punto de movimiento
    public Transform pointB; // Segundo punto de movimiento
    private Transform targetPoint; // Punto al que se dirige el enemigo
    private Vector3 originalScale; // Escala inicial del enemigo

    private bool isDead = false; // Estado del enemigo (muerto o no)

    public Animator animator; // Para animaciones, si las tienes

    void Start()
    {
        // Inicializa el objetivo del movimiento
        targetPoint = pointA; // Inicia dirigi�ndose hacia el punto A
        originalScale = transform.localScale;
    }

    void Update()
    {
        // Si el enemigo est� muerto, no se mueve
        if (isDead) return;

        // Mueve al enemigo hacia el punto objetivo
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        // Cambia el punto objetivo cuando llegue a uno de los puntos
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            // Alterna entre los puntos A y B
            targetPoint = (targetPoint == pointA) ? pointB : pointA;

            // Voltea el sprite dependiendo del punto al que se dirija
            Flip();
        }
    }

    /// <summary>
    /// Cambia la direcci�n del enemigo (volteando el sprite).
    /// </summary>
    void Flip()
    {
        // Alterna la escala X para que el enemigo se voltee
        Vector3 newScale = originalScale;
        newScale.x *= targetPoint == pointB ? 5 : -5;
        transform.localScale = newScale;
    }

    // M�todo para manejar el da�o recibido al golpearse en la cabeza
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si el enemigo es golpeado en la cabeza (con un tag espec�fico, por ejemplo "PlayerHead")
        if (collision.gameObject.CompareTag("PlayerHead"))
        {
            Die(); // Llamamos al m�todo de muerte
        }
    }

    /// <summary>
    /// M�todo que maneja la muerte del enemigo.
    /// </summary>
    void Die()
    {
        if (isDead) return;

        isDead = true; // Marca al enemigo como muerto

        // Llamar a la animaci�n de muerte, si tienes una configurada
        if (animator != null)
        {
            animator.SetTrigger("Die"); // Asumiendo que tienes una animaci�n de muerte
        }

        // Destruir el enemigo despu�s de un tiempo (opcional)
        Destroy(gameObject, 1f); // 1 segundo despu�s de la muerte, destruye el enemigo
    }
}