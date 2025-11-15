using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SuperPlayerControllerIso : MonoBehaviour
{
    
    [Header("Player Stats")]
    [SerializeField]
    public float speed = 6f;
    [SerializeField]
    public int attackDamage = 3;
    [SerializeField]
    public float attackRange = 1f;
    [SerializeField]
    public float dashAttackSpeed = 7f;
    [SerializeField]
    public float attackCooldown = 0.5f;
    [SerializeField]
    public float dashAttackCooldown = 0.5f;
    
    [Header("Components")]
     [SerializeField]
    GameObject axeSwingPrefab;
    GameObject axeSwingInstance;
    SpriteRenderer sprite;
    Rigidbody2D rb;
    Animator animator;
    GameController gameController;
   
    Vector2 axeSwingPosition;
    Vector2 inputMovement;
    bool inputAttack;
    Vector3 mousePos;
    Vector3 spawnPoint;
    Vector3 lookDir;
    public int enemyDamage = 0;
    bool isAttacking = false;
    bool isDashAttacking = false;
    
    public enum PlayerState { Idle, Moving, Attacking, RunAttack, Dead }
    public PlayerState playerState = PlayerState.Idle;
    public enum PlayerHpState { Hurt, Normal, Invincible }
    public PlayerHpState playerHpState = PlayerHpState.Normal;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        sprite = GetComponent<SpriteRenderer>();
        gameController = GameObject.FindObjectOfType<GameController>();
        animator = GetComponent<Animator>();
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
            case PlayerState.Dead: Die(); break;
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
        animator.Play("Idle_barbarian");
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
    void Die()
    {
        rb.velocity = Vector2.zero;
        animator.Play("Die_barbarian");
        
    }
    void Run()
    {
        //comportamento
        rb.velocity = inputMovement * speed;
        animator.Play("Run_barbarian");
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
        if (axeSwingInstance == null)
        axeSwingInstance = Instantiate(axeSwingPrefab, transform.position, Quaternion.identity);
        animator.Play("Attack_barbarian");

        //transições
        if (!isAttacking)
        {
            isAttacking = true;
            StartCoroutine(AttackCooldown());
        }
           
    }
    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        if (axeSwingInstance != null)
        Destroy(axeSwingInstance);
        axeSwingInstance = null;
        isAttacking = false;
        
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
        if (axeSwingInstance == null)
        animator.Play("Attack_barbarian");
        axeSwingInstance = Instantiate(axeSwingPrefab, transform.position, Quaternion.identity);

        //transições
        if (!isDashAttacking)
        {
            isDashAttacking = true;
            StartCoroutine(DashAttackCooldown());
        }
        
    }
    private IEnumerator DashAttackCooldown()
    {
        yield return new WaitForSeconds(dashAttackCooldown);
        if (axeSwingInstance != null)
        Destroy(axeSwingInstance);
        axeSwingInstance = null;
        isDashAttacking = false;
        
        if (!inputAttack && inputMovement != Vector2.zero)
        {
            playerState = PlayerState.Moving;
        }
        if (inputAttack)
        {
            playerState = PlayerState.Attacking;
        }
        playerState = PlayerState.Idle;
    }
    private IEnumerator BlinkEffect()
    {
        playerHpState = PlayerHpState.Invincible;
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        playerHpState = PlayerHpState.Normal;
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
        
        //if (inputMovement.x >= 0)
        {
            sprite.flipX = false;
        }
        else
        {
            sprite.flipX = true;
            if ((playerState == PlayerState.Attacking || playerState == PlayerState.RunAttack) && axeSwingInstance != null)
                {
                    axeSwingInstance.transform.position = new Vector2(transform.position.x - 0.6f,transform.position.y);
                }
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

        }
        if (collision.CompareTag("Item"))
        {
            if (GameController.lifes < 5)
            {
                GameController.lifes += 50;
                gameController.ui.UpdateLives(GameController.lifes);
                collision.GetComponent<Animator>().SetBool("Open", true);
            }
        }
    }
}

