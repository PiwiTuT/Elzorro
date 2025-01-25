using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 movement;
    private Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        ChooseNewDirection();
    }

    void Update()
    {
        rb2d.linearVelocity = movement * moveSpeed;
        if (Random.Range(0, 100) < 5)
        {
            ChooseNewDirection();
        }
    }

    void ChooseNewDirection()
    {
        float angle = Random.Range(0f, 360f);
        movement = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
    }
}
