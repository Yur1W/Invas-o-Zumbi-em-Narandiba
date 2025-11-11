using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControllerIso : MonoBehaviour
{
    [SerializeField]
    public float speed = 2f;
    [SerializeField]
    SpriteRenderer sprite;
    Rigidbody2D rb;
    Vector2 input;
    Vector3 mousePos;
    Vector3 lookDir;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        sprite = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        input = Vector2.ClampMagnitude(input, 1f);
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        ChangeDirection();
    }

    void FixedUpdate()
    {
        rb.velocity = input * speed;
    }
    void ChangeDirection()
    {
       Vector3 lookDir = mousePos - transform.position;
        if (lookDir.x >= 0)
        {
            sprite.flipX = false;
        }
        else
        {
            sprite.flipX = true;
        } 
    }
}

