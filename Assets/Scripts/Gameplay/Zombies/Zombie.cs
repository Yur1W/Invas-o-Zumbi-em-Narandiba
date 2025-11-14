using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    
[RequireComponent(typeof(Rigidbody2D))]
public class Zombie : MonoBehaviour
{
    public int hp = 3;
    public int damage = 1;
    public float speed = 1f;
    SpriteRenderer sprite;
    Rigidbody2D rb;
    Animator animator;
    GameController gameController;
    GameObject player;
    float horizontalInput;
    float verticalInput;
    bool playerVisto;

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
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, Vector2.left * 5f, Color.red);
        if (player == null)
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void FixedUpdate()
    {  
         //maquina de estado
        switch (zombieState)
        {
            case ZombieState.Idle:Idle();break;
            case ZombieState.Moving:Run();break;
            case ZombieState.Chasing:PlayerCheck();break;
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
    void Run()
    {
        
    }
    void Attack()
    {
        animator.Play("Attack");
        rb.velocity = Vector2.zero;
        if (playerVisto)
        {zombieState = ZombieState.Moving;}
    }
    void Die()
    {
        animator.Play("Die");
        rb.velocity = Vector2.zero;
        if (GetComponent<CircleCollider2D>() != null)
        {   
            damage = 10;
            GetComponent<CircleCollider2D>().enabled = true;
            animator.Play("Explode");
        }
        Destroy(gameObject, 1f);
    }
    void PlayerCheck()
    {
        if (Physics2D.Raycast(transform.position, Vector2.left, 5f, LayerMask.GetMask("Player")))
        { 
            playerVisto = true;
            zombieState = ZombieState.Chasing;
            if (sprite.flipX){}
        }
    }
    void ChasePlayer()
    {
        if (playerVisto)
        {
            //persuit
            Vector2 direction = (player.transform.position - transform.position).normalized;
            rb.velocity = direction * speed;
            animator.Play("Run");

            //transições
            if (!playerVisto)
            {
                zombieState = ZombieState.Idle;
            }
        }
    }
    void ChangeDirection()
    {
        if (horizontalInput != 0)
        {
            sprite.flipX = true;
        }
        else
        {
            sprite.flipX = false;
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        previousState = zombieState;
        if (collision.gameObject.CompareTag("PlayerWeapon"))
        {
            hp--;
            if (hp <= 0)
            {
                GameController.killCount++;
                zombieState = ZombieState.Dead;
                
            }
        }
        if (collision.gameObject.CompareTag("Player")&& previousState != ZombieState.Dead)
        {
            zombieState = ZombieState.Attacking;
        }
    }
}
