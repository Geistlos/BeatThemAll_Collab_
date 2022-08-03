using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{

    public enum EnemyState { Idle, Walk, Attack, Dead}
    public EnemyState currentState;

    Rigidbody2D rb2d;
    SpriteRenderer sr;
    Animator animator;

    Vector2 dirInput;
    float randomSwitch;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
    }
    

    void OnStateEnter ()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                break;
            case EnemyState.Walk:
                break;
            case EnemyState.Attack:
                animator.SetFloat("Blend", randomSwitch);
                animator.SetTrigger("AttackTrigger");
                if (randomSwitch == 1f) randomSwitch = 0f;
                else randomSwitch = 1f;
                break;
            case EnemyState.Dead:
                animator.SetTrigger("DeathTrigger");
                break;
            default:
                break;
        }
    }

    void OnStateUpdate ()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                break;
            case EnemyState.Walk:
                break;
            case EnemyState.Attack:
                break;
            case EnemyState.Dead:
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

    public void TakeDamage()
    {
        animator.SetTrigger("HurtTrigger");
    }

    void Update()
    {
        OnStateUpdate();

        dirInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rb2d.velocity = dirInput;
        animator.SetFloat("VelocityX", dirInput.x);
        if (dirInput != Vector2.zero)
        {
            if(dirInput.x > 0) sr.flipX = false; else sr.flipX = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift)) TakeDamage();
        if(Input.GetKeyDown(KeyCode.Mouse0)) TransitionToState(EnemyState.Attack);
        if(Input.GetKeyDown(KeyCode.Mouse1)) TransitionToState(EnemyState.Dead);
        if (Input.GetKeyDown(KeyCode.Space)) animator.SetBool("IsInAir", true);
    }
}
