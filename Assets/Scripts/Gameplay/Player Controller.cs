using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControllerIso : MonoBehaviour
{
    [SerializeField]
    public float speed = 2f;
    public float attackRange = 1f;
    public float dashAttackSpeed = 4f;
    public float attackCooldown = 0.5f;
    public float dashAttackCooldown = 0.5f;
    [SerializeField]
    SpriteRenderer sprite;
    Rigidbody2D rb;
    Animator animator;
    GameController gameController;
    Vector2 inputMovement;
    bool inputAttack;
    Vector3 mousePos;
    Vector3 spawnPoint;
    Vector3 lookDir;
    public int enemyDamage = 0;
    public enum PlayerState { Idle, Moving, Attacking, RunAttack }
    public PlayerState playerState = PlayerState.Idle;
    public enum PlayerHpState { Hurt, Normal, Invincible }
    public PlayerHpState playerHpState = PlayerHpState.Normal;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        sprite = GetComponent<SpriteRenderer>();
        gameController = GameObject.FindObjectOfType<GameController>();
        // set spawn point
        spawnPoint = transform.position;
    }
    void Update()
    {
        //inputMovement catch
        inputMovement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        inputMovement = Vector2.ClampMagnitude(inputMovement, 1f);
        inputAttack = Input.GetKey(KeyCode.Mouse0);
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //sprite direction
        ChangeDirection();

    }

    void FixedUpdate()
    {

        //state machine actions
        switch (playerState)
        {
            case PlayerState.Idle: Idle(); break;
            case PlayerState.Moving: Run(); break;
            case PlayerState.Attacking: Attack(); break;
            case PlayerState.RunAttack: RunAttack(); break;
            default:; break;
        }
        //hp state machine 
        switch (playerHpState)
        {
            case PlayerHpState.Hurt: TakeDamage(enemyDamage); break;
            case PlayerHpState.Normal:; break;
            case PlayerHpState.Invincible: Invicibility(); break;
            default:; break;
        }
    }
    void Idle()
    {   
        rb.velocity = Vector2.zero;
        //transições
        if (inputMovement != Vector2.zero)
        {
            playerState = PlayerState.Moving;
        }
        if (inputAttack)
        {
            playerState = PlayerState.Attacking;
        }
    }
    void Run()
    {
        //comportamento
        rb.velocity = inputMovement * speed;
        //transições
        if (inputMovement == Vector2.zero)
        {
            playerState = PlayerState.Idle;
        }
        if (inputAttack)
        {
            playerState = PlayerState.Attacking;
        }
        if (inputMovement != Vector2.zero && inputAttack)
        {
            playerState = PlayerState.RunAttack;
        }
    }
    void Attack()
    {
        //comportamento
        rb.velocity = Vector2.zero;

        //transições
        StartCoroutine(AttackCooldown());   
    }
    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        
        if (!inputAttack && inputMovement != Vector2.zero)
        {
            playerState = PlayerState.Moving;
        }
        if (inputMovement != Vector2.zero && inputAttack)
        {
            playerState = PlayerState.RunAttack;
        }
        playerState = PlayerState.Idle;
    }
    void RunAttack()
    {
        //comportamento
        rb.velocity = inputMovement * dashAttackSpeed;

        //transições
        StartCoroutine(DashAttackCooldown());
    }
    private IEnumerator DashAttackCooldown()
    {
        yield return new WaitForSeconds(dashAttackCooldown);
        
        if (!inputAttack && inputMovement != Vector2.zero)
        {
            playerState = PlayerState.Moving;
        }
        if (inputMovement != Vector2.zero && inputAttack)
        {
            playerState = PlayerState.RunAttack;
        }
        playerState = PlayerState.Idle;
    }
    private IEnumerator BlinkEffect()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
        yield return new WaitForSeconds(0.1f);

    }
    private void Invicibility()
    {
        StartCoroutine(InvicibilityDuration());
    }
    private IEnumerator InvicibilityDuration()
    {
        GetComponent<CapsuleCollider2D>().enabled = false;
        yield return new WaitForSeconds(1f);
        GetComponent<CapsuleCollider2D>().enabled = true;
        playerHpState = PlayerHpState.Normal;
    }

    // change sprite direction based on mouse position
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
    public void TakeDamage(int damage)
    {
        GameController.lifes -= damage;
        StartCoroutine(BlinkEffect());

        if (GameController.lifes <= 0)
        {
            gameController.GameOver();
            gameObject.SetActive(false);
        }
    }
    public void Respawn()
    {
        transform.position = Vector3.zero;
        rb.velocity = Vector2.zero;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemyDamage = collision.GetComponent<Zombie>().damage;
            playerHpState = PlayerHpState.Hurt;
            Destroy(collision.gameObject);

        }
        if (collision.CompareTag("Item"))
        {
            if (GameController.lifes < 5)
            {
                GameController.lifes += 1;
                gameController.ui.UpdateLives(GameController.lifes);
            }
            Destroy(collision.gameObject);
        }
    }
}

