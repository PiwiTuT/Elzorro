using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 10f;
    public float dashForce = 20f;
    public float dashCooldown = 1f;
    public float dashTimeWindow = 0.3f; // Tiempo en segundos para detectar un doble toque
    public int maxHealth = 100; // Vida m�xima del jugador
    public int currentHealth;  // Vida actual del jugador
    public int damageAmount = 10; // Da�o recibido por defecto

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool canDash = true;
    private float lastMoveTime = -100f; // Inicialmente, un valor negativo para que no se active de inmediato
    private string lastMoveDirection = "";
    private float lastDashTime = 0f;

    public Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth; // Inicializar vida al m�ximo
    }

    void Update()
    {
        // Movimiento horizontal
        float move = Input.GetAxis("Horizontal");
        animator.SetFloat("movement", Mathf.Abs(move));

        if (move < 0)
        {
            transform.localScale = new Vector3(-5, 5, 5);
        }
        else if (move > 0)
        {
            transform.localScale = new Vector3(5, 5, 5);
        }

        if (move != 0)
        {
            rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);

            string currentDirection = move > 0 ? "right" : "left";

            // Verificar si se presion� dos veces hacia la derecha o izquierda
            if (currentDirection == lastMoveDirection && Time.time - lastMoveTime <= dashTimeWindow && canDash)
            {
                StartCoroutine(Dash(currentDirection));
            }

            lastMoveTime = Time.time;
            lastMoveDirection = currentDirection;
        }

        // Saltar
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private IEnumerator Dash(string direction)
    {
        // Asegurarse de que el dash no se ejecute hasta despu�s del enfriamiento
        if (Time.time - lastDashTime < dashCooldown)
            yield break;

        canDash = false;
        lastDashTime = Time.time;

        // Realizar el dash
        float dashDirection = direction == "right" ? 1f : -1f;
        Vector2 dashVelocity = new Vector2(dashDirection * dashForce, rb.linearVelocity.y);
        rb.linearVelocity = dashVelocity;

        yield return new WaitForSeconds(0.1f); // Duraci�n del dash

        // Detener el movimiento del dash despu�s de 0.1 segundos
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        yield return new WaitForSeconds(dashCooldown); // Tiempo de enfriamiento
        canDash = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        // Si colisiona con un enemigo, recibe da�o
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(damageAmount);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    /// <summary>
    /// Reduce la vida del jugador al recibir da�o.
    /// </summary>
    /// <param name="damage">Cantidad de da�o recibido.</param>
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"Jugador recibi� {damage} de da�o. Vida actual: {currentHealth}");

        // Verificar si el jugador muri�
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Acci�n al morir.
    /// </summary>
    private void Die()
    {
        Debug.Log("El jugador ha muerto.");
        animator.SetTrigger("Die"); // Asume que tienes una animaci�n de muerte configurada
        // Puedes implementar l�gica adicional, como reiniciar el nivel.
    }

    /// <summary>
    /// Regenera la vida del jugador.
    /// </summary>
    /// <param name="healAmount">Cantidad de vida a regenerar.</param>
    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        Debug.Log($"Jugador regener� {healAmount} de vida. Vida actual: {currentHealth}");
    }
}
