using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public enum EnemyState { Idle, Walk, Attack, Dead, Hurted}
    public EnemyState currentState;
    [SerializeField] StatsScriptable statsScriptable;

    Rigidbody2D rb2d;
    SpriteRenderer sr;
    Animator animator;

    Transform currentTarget;
    [SerializeField] Transform player1;
    [SerializeField] Transform player2;

    Vector2 dirMove;
    bool attackSwitch;
    float health;
    float speed;
    float timer;
    float cooldown;


    void Start()
    {
        GetComponentInChildren<Animator>().runtimeAnimatorController = statsScriptable.animator;
        rb2d = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        health = statsScriptable.life;
        speed = statsScriptable.speed;
        TransitionToState(EnemyState.Idle);
    }
    

    void OnStateEnter ()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                timer = 0f;
                break;
            case EnemyState.Walk:
                animator.SetBool("IsWalking", true);
                break;
            case EnemyState.Attack:
                timer = 0f;
                animator.SetBool("AttackSwitch", attackSwitch);
                animator.SetTrigger("AttackTrigger");
                if (attackSwitch is true) attackSwitch = false;
                else attackSwitch = true;
                cooldown = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
                break;
            case EnemyState.Dead:
                animator.SetTrigger("DeathTrigger");
                timer = 0f;
                break;
            case EnemyState.Hurted:
                animator.SetTrigger("HurtTrigger");
                timer = 0f;
                cooldown = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
                break;
            default:
                break;
        }
    }

    void OnStateUpdate ()
    {
        if (currentTarget != null && currentState != EnemyState.Dead)
        {
            dirMove = currentTarget.position - transform.position;

            if (dirMove != Vector2.zero) { if (dirMove.x > 0) sr.flipX = false; else sr.flipX = true; }
        }

        #region inputtest
        /*
        dirMove = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rb2d.velocity = dirMove.normalized * speed;
        if (dirMove != Vector2.zero)
        {
            if (dirMove.x > 0) sr.flipX = false; else sr.flipX = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift)) TakeDamage();
        if (Input.GetKeyDown(KeyCode.Mouse0)) TransitionToState(EnemyState.Attack);
        if (Input.GetKeyDown(KeyCode.Mouse1)) TransitionToState(EnemyState.Dead);
        if (Input.GetKeyDown(KeyCode.Space)) animator.SetBool("IsInAir", true);
        */
        #endregion

        switch (currentState)
        {
            case EnemyState.Idle:
                if(currentTarget != null)
                {
                    timer += Time.deltaTime;
                    if (timer > statsScriptable.attackSpeed) TransitionToState(EnemyState.Attack);

                    if (Vector2.Distance(transform.position, currentTarget.position) > 1f) TransitionToState(EnemyState.Walk);
                }

                if (currentTarget == null)
                {
                    if (Vector2.Distance(transform.position, player1.position) < 5f) currentTarget = player1;

                    if (Vector2.Distance(transform.position, player2.position) < 5f) currentTarget = player2;
                }
                break;
            case EnemyState.Walk:
                if(currentTarget == null)
                {
                    TransitionToState(EnemyState.Idle);
                    return;
                }

                if (Vector2.Distance(transform.position, currentTarget.position) < 0.5f) TransitionToState(EnemyState.Idle);


                if (Vector2.Distance(transform.position, currentTarget.position) > 5f)
                {
                    currentTarget = null;
                    TransitionToState(EnemyState.Idle);
                }
                break;
            case EnemyState.Attack:
                timer += Time.deltaTime;
                if (timer > cooldown) TransitionToState(EnemyState.Idle);
                break;
            case EnemyState.Dead:
                timer += Time.deltaTime;
                if(timer >= 2f) Destroy(gameObject);
                break;
            case EnemyState.Hurted:
                timer += Time.deltaTime;
                if (timer >= cooldown) TransitionToState(EnemyState.Idle);
                break;
            default:
                break;
        }
    }

    void OnStateFixedUpdate()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                break;
            case EnemyState.Walk:
                if (currentTarget != null) rb2d.velocity = dirMove.normalized * speed;
                break;
            case EnemyState.Attack:
                break;
            case EnemyState.Dead:
                break;
            case EnemyState.Hurted:
                if (timer < 0.25f) rb2d.velocity = -dirMove.normalized * 2; else rb2d.velocity = Vector2.zero;
                break;
            default:
                break;
        }
    }

    void OnStateExit ()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                break;
            case EnemyState.Walk:
                animator.SetBool("IsWalking", false);
                rb2d.velocity = Vector2.zero;
                break;
            case EnemyState.Attack:
                break;
            case EnemyState.Dead:
                break;
            case EnemyState.Hurted:
                timer = 0f;
                break;
            default:
                break;
        }
    }

    void TransitionToState (EnemyState nextState)
    {
        OnStateExit();
        currentState = nextState;
        OnStateEnter();
    }

    public void TakeDamage(int DamageAmount)
    {
        if (currentState != EnemyState.Dead)
        {
        health -= DamageAmount;
        if(health <= 0) TransitionToState(EnemyState.Dead);
        else TransitionToState(EnemyState.Hurted);
        animator.SetTrigger("HurtTrigger");
        }
    }

    public void Attack()
    {
        Collider2D[] Colliders;

        Colliders = Physics2D.OverlapCircleAll(transform.position, 4f);

        foreach (Collider2D collider in Colliders)
        {
            if (collider.tag == "Player")
            {
                //Debug.Log(collider.gameObject.name + " HIT");
            }
        }
    }

    void Update()
    {
        OnStateUpdate();
    }

    void FixedUpdate()
    {
        OnStateFixedUpdate();
    }
}
