using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public enum EnemyState { Idle, Walk, Attack, Dead, Hurted }
    public EnemyState currentState;
    [SerializeField] StatsScriptable statsScriptable;
    [SerializeField] GameObject collecPrefab;

    Rigidbody2D rb2d;
    SpriteRenderer sr;
    Animator animator;

    Transform currentTarget;
    Transform player1;
    Transform player2;

    Vector2 dirMove;
    Vector3 dirMoveFixed;
    bool attackSwitch;
    bool firstAttack = true;
    float randomAttackSpeed;
    float health;
    float speed;
    float timer;
    float cooldown;
    int collectiblesMin;
    int collectiblesMax;

    void Start()
    {
        GetComponentInChildren<Animator>().runtimeAnimatorController = statsScriptable.animator;
        rb2d = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        health = statsScriptable.life;
        speed = statsScriptable.speed;
        collectiblesMin = statsScriptable.collectiblesMin;
        collectiblesMax = statsScriptable.collectiblesMax;

        player1 = GameManager.Instance.player1.transform;
        if (GameManager.Instance.player2 != null) player2 = GameManager.Instance.player2.transform;
        TransitionToState(EnemyState.Idle);
    }


    void OnStateEnter()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                /*
                if (firstAttack is true && currentTarget != null)
                {
                    firstAttack = false;
                    TransitionToState(EnemyState.Attack);
                }
                */
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
                gameObject.GetComponent<CapsuleCollider2D>().isTrigger = true;
                SpawnCollectibles(Random.Range(collectiblesMin,collectiblesMax+1));
                animator.SetTrigger("DeathTrigger");
                timer = 0f;
                Level1Manager.Instance.killCount++;
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

    void OnStateUpdate()
    {
        if (currentTarget != null && currentState != EnemyState.Dead)
        {

            dirMoveFixed = new Vector3(currentTarget.position.x, currentTarget.position.y - 0.75f, 0f);


            dirMove = dirMoveFixed - transform.position;

            if (dirMove != Vector2.zero) { if (dirMove.x > 0) sr.flipX = false; else sr.flipX = true; }
        }

        if (GameManager.Instance.isPlayer1Dead == true) currentTarget = null;

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
                if (currentTarget != null)
                {
                    randomAttackSpeed = Random.Range(0.2f, 0.4f);
                    timer += Time.deltaTime;
                    if (timer > statsScriptable.attackSpeed) TransitionToState(EnemyState.Attack);
                    if (timer > randomAttackSpeed && firstAttack)
                    {
                        TransitionToState(EnemyState.Attack);
                        firstAttack = false;
                    }

                    if (Vector2.Distance(transform.position, dirMoveFixed) > 1.2f)
                    {
                        firstAttack = true;
                        TransitionToState(EnemyState.Walk);
                    }
                }

                if (currentTarget == null && GameManager.Instance.isPlayer1Dead == false)
                {
                    if (Vector2.Distance(transform.position, player1.position) < 7.5f) currentTarget = player1;

                    if (player2 != null && GameManager.Instance.isPlayer2Dead == false)
                    {
                        if (Vector2.Distance(transform.position, player2.position) < 7.5f) currentTarget = player2;
                    }
                }
                break;
            case EnemyState.Walk:
                if (currentTarget == null)
                {
                    TransitionToState(EnemyState.Idle);
                    return;
                }

                if (Vector2.Distance(transform.position, dirMoveFixed) < 1f) TransitionToState(EnemyState.Idle);


                if (Vector2.Distance(transform.position, dirMoveFixed) > 7.5f)
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
                if (timer >= 2f) Destroy(gameObject);
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
                rb2d.velocity = dirMove.normalized * 0f;
                break;
            case EnemyState.Walk:
                if (currentTarget != null) rb2d.velocity = dirMove.normalized * speed;
                break;
            case EnemyState.Attack:
                rb2d.velocity = dirMove.normalized * 0f;
                break;
            case EnemyState.Dead:
                rb2d.velocity = dirMove.normalized * 0f;
                break;
            case EnemyState.Hurted:
                if (timer < 0.25f) rb2d.velocity = -dirMove.normalized * 2; else rb2d.velocity = Vector2.zero;
                break;
            default:
                break;
        }
    }

    void OnStateExit()
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

    void TransitionToState(EnemyState nextState)
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
            if (health <= 0) TransitionToState(EnemyState.Dead);
            else TransitionToState(EnemyState.Hurted);
            animator.SetTrigger("HurtTrigger");
        }
    }

    public void Attack()
    {
        Collider2D[] Colliders;

        Colliders = Physics2D.OverlapCircleAll(transform.position, .5f);

        foreach (Collider2D collider in Colliders)
        {
            if (collider.tag == "Player")
            {
                //Debug.Log(collider.gameObject.name + " HIT");
                collider.transform.root.GetComponent<PlayerBehavior>().TakeHit(statsScriptable.dmg);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Can")
        {
            if (!collision.GetComponent<Can>().onTheGround && !collision.GetComponent<Can>().carried)
            {
                TakeDamage(10);
            }
        }
    }

    void SpawnCollectibles(int quantity)
    {
        for (int i = 0; i < quantity; i++)
        {
            var coords = new Vector3(transform.position.x - .5f + Random.Range(0f, 1f),
                transform.position.y - .5f + Random.Range(0f, 1f),
                transform.position.z);
            Instantiate(collecPrefab, coords, Quaternion.identity);
            Debug.Log("Spawn collectible n: " + i);
        }
    }
}
