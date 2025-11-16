using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    
[RequireComponent(typeof(Rigidbody2D))]
public class BombZombie : MonoBehaviour
{
    public int hp = 3;
    public int damage = 3;
    public float speed = 1f;
    public float attackDuration = 1f;
    public float deathDuration = 1f;
    SpriteRenderer sprite;
    Rigidbody2D rb;
    Animator animator;
    GameController gameController;
    GameObject player;
    float horizontalInput;
    float verticalInput;
    bool playerVisto;
    Vector2 direçãoPlayer;

    //cohesion
    float otherZombiesArea = 3f;
    float effect = 0.5f;

    static List<Zombie> allZombies = new List<Zombie>();

    enum ZombieState { Idle, Moving, Chasing, Attacking, Dead }
    ZombieState zombieState = ZombieState.Idle;
    ZombieState previousState;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        gameController = FindObjectOfType<GameController>();
        player = GameObject.FindGameObjectWithTag("Player");
        GmaeOverCheck();    
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.DrawRay(transform.position, Vector2.left * 5f, Color.red);
        if (player == null)
        player = GameObject.FindGameObjectWithTag("Player");
        PlayerCheck();
        ChangeDirection();
        if (GameController.isGameOver)
        {
            Destroy(gameObject);
        }
    }
    void FixedUpdate()
    {  
         //maquina de estado
        switch (zombieState)
        {
            case ZombieState.Idle:Idle();break;
            case ZombieState.Moving:Run();break;
            case ZombieState.Chasing:ChasePlayerCohesion();break;
            case ZombieState.Attacking:Attack();break;
            case ZombieState.Dead:Die();break;
        }

    }
    void Idle()
    {
        rb.velocity = Vector2.zero;
        animator.Play("Idle");
        //transições
        if (playerVisto)
        {
            zombieState = ZombieState.Chasing;
        }
    }
    void GmaeOverCheck()
    {
        if (GameController.isGameOver)
        {
            Destroy(gameObject);
        }
    }
    void Run()
    {
        
    }
    void Attack()
    {
        animator.Play("Attack");
        rb.velocity = Vector2.zero;
        StartCoroutine(AttackDuration());
    }
    IEnumerator AttackDuration()
    {
        yield return new WaitForSeconds(attackDuration);
        if (playerVisto)
        {zombieState = ZombieState.Chasing;}
        else {zombieState = ZombieState.Idle;}
    }
    void Die()
    {
        StartCoroutine(DeathAction());
    }
    IEnumerator DeathAction()
    {
        animator.Play("Die");
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(deathDuration);
        CircleCollider2D bomb = GetComponent<CircleCollider2D>();
        if (bomb != null)
        {   
            damage = 10;
            bomb.enabled = true;
            animator.Play("Explode");
        }
        Destroy(gameObject, 0.5f);
    }
    void PlayerCheck()
    {
        if (zombieState == ZombieState.Dead||zombieState == ZombieState.Attacking)
        {
            return;
        } 
        GmaeOverCheck();
        direçãoPlayer = (player.transform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direçãoPlayer,2.5f, LayerMask.GetMask("Player"));
        if (hit.collider != null)
        {
            playerVisto = true;
            zombieState = ZombieState.Chasing;
        }
        else
        {
            playerVisto = false;
        }
    }
    void ChasePlayerCohesion()
    {
        if (!playerVisto)
            {
                zombieState = ZombieState.Idle;
                return;
            }

        if (playerVisto)
        {
            //persuit
            Vector2 direction = (player.transform.position - transform.position).normalized;
            Vector2 calculatedDirection = direction;
            //try cohesion
            Collider2D[] viewArea = Physics2D.OverlapCircleAll(transform.position, otherZombiesArea, LayerMask.GetMask("Enemy"));
            if (viewArea.Length > 1)
            {
                Vector2 averagePosition = Vector2.zero;
                int count = 0;
                foreach (Collider2D col in viewArea)
                {
                    if (col.gameObject != this.gameObject)
                    {
                        averagePosition += (Vector2)col.transform.position;
                        count++;
                    }
                }
                if (count > 0)
                {
                        averagePosition /= count;
                        Vector2 cohesion = (averagePosition - (Vector2)transform.position).normalized;
                        calculatedDirection = direction + cohesion * effect;
                        calculatedDirection.Normalize();
                }
            }                
            rb.velocity = calculatedDirection * speed;
            animator.Play("Run"); 
        }
            
            
    }
    void ChangeDirection()
    {
        if (rb.velocity.x > 0.1f)
        {
            sprite.flipX = false;
        }
        else if (rb.velocity.x < -0.1f)
        {
            sprite.flipX = true;
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(zombieState == ZombieState.Dead)
        {
            return;
        }
        if (collision.gameObject.CompareTag("PlayerWeapon"))
        {
           if (player.GetComponent<PlayerControllerIso>() == null)
            {
                return;
            }
            SuperPlayerControllerIso super = player.GetComponent<SuperPlayerControllerIso>();
            PlayerControllerIso normal = player.GetComponent<PlayerControllerIso>();
            
            if (player.GetComponent<PlayerControllerIso>() == null)
            hp -= super.attackDamage;
            else
            {
                hp -= normal.attackDamage;
            }
            StartCoroutine(BlinkEffect());
            if (hp <= 0)
            {
                GameController.killCount++;
                zombieState = ZombieState.Dead;
                
            }
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            zombieState = ZombieState.Attacking;
        }
    }
    private IEnumerator BlinkEffect()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
        yield return new WaitForSeconds(0.1f);
    }
}
