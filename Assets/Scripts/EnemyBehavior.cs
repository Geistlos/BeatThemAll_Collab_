using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{

    public enum EnemyState { Idle, Walk, Attack, Dead}
    public EnemyState currentState;
    [SerializeField] StatsScriptable statsScriptable;

    Rigidbody2D rb2d;
    SpriteRenderer sr;
    Animator animator;
    Transform targetTransform;

    Vector2 dirMove;
    bool targetOnRange;
    bool attackSwitch;
    float health;
    float speed;
    float timer;
    float cooldown;
    bool isDead;


    void Start()
    {
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
                if(!isDead) animator.SetTrigger("DeathTrigger");
                isDead = true;
                timer = 0f;
                break;
            default:
                break;
        }
    }

    void OnStateUpdate ()
    {
        if (targetTransform != null && !isDead)
        {
            dirMove = targetTransform.position - transform.position;

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
                if(targetOnRange == true)
                {
                    timer += Time.deltaTime;
                    if (timer > statsScriptable.attackSpeed) TransitionToState(EnemyState.Attack);

                    if (Vector2.Distance(transform.position, targetTransform.position) > 1f) TransitionToState(EnemyState.Walk);
                }
                break;
            case EnemyState.Walk:
                if(targetOnRange == false)
                {
                    TransitionToState(EnemyState.Idle);
                    return;
                }

                if (Vector2.Distance(transform.position, targetTransform.position) < 0.5f) TransitionToState(EnemyState.Idle);

                break;
            case EnemyState.Attack:
                timer += Time.deltaTime;
                if (timer > cooldown) TransitionToState(EnemyState.Idle);
                break;
            case EnemyState.Dead:
                timer += Time.deltaTime;
                if(timer >= 2f) Destroy(gameObject);
                break;
            default:
                break;
        }
    }

    void OnStateFixedUpdate()
    {

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
        health -= DamageAmount;
        if(health <= 0) TransitionToState(EnemyState.Dead);
        else animator.SetTrigger("HurtTrigger");
    }

    public void PlayerDetected(Transform target)
    {
        targetTransform = target;
        targetOnRange = true;
    }

    public void PlayerLost()
    {
        targetTransform = null;
        targetOnRange = false;
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
        switch (currentState)
        {
            case EnemyState.Idle:
                break;
            case EnemyState.Walk:
                if (targetOnRange == true) rb2d.velocity = dirMove.normalized * speed;
                break;
            case EnemyState.Attack:
                break;
            case EnemyState.Dead:
                break;
            default:
                break;
        }
    }
}
