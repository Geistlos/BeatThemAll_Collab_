using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    [SerializeField] GameObject graphics;
    [SerializeField] AnimationCurve curve;

    public enum PlayerState
    {
        IDLE,
        WALK,
        SPRINT,
        ATTACKING,
        JUMPING,
        DEATH
    }
    public PlayerState currentState;

    Animator animator;
    Rigidbody rb;
    
    bool playerIsMoving;
    Vector2 dirInput;
    //TEMPS
    float walkingSpeed = 7;
    float runningSpeed = 14;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        dirInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        OnStateUpdate();
    }

    void FixedUpdate()
    {
        OnStateFixedUpdate();
    }


    void OnStateEnter()
    {
        switch (currentState)
        {
            case PlayerState.IDLE:
                break;
            case PlayerState.WALK:
                animator.SetBool("IsRunning", true);
                animator.SetFloat("RunningSpeed", 0f);
                break;
            case PlayerState.SPRINT:
                animator.SetBool("IsRunning", true);
                animator.SetFloat("RunningSpeed", 1f);
                break;
            case PlayerState.ATTACKING:
                break;
            case PlayerState.JUMPING:
                animator.SetTrigger("Jump");
                Debug.Log("Jump");
                break;
            case PlayerState.DEATH:
                break;
            default:
                break;
        }
    }
    void OnStateFixedUpdate()
    {
        switch (currentState)
        {
            case PlayerState.IDLE:
                break;
            case PlayerState.WALK:
                rb.velocity = dirInput.normalized * walkingSpeed;
                break;
            case PlayerState.SPRINT:
                rb.velocity = dirInput.normalized * runningSpeed;
                break;
            case PlayerState.ATTACKING:
                break;
            case PlayerState.JUMPING:
                break;
            case PlayerState.DEATH:
                break;
            default:
                break;
        }

    }
    void OnStateUpdate()
    {
        switch (currentState)
        {
            case PlayerState.IDLE:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    TransitionToState(PlayerState.JUMPING);
                }
                if (dirInput != Vector2.zero && Input.GetKeyDown(KeyCode.LeftShift))
                {
                    TransitionToState(PlayerState.SPRINT);
                }
                else if (dirInput != Vector2.zero)
                {
                    TransitionToState(PlayerState.WALK);
                }
                break;
            case PlayerState.WALK:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    TransitionToState(PlayerState.JUMPING);
                }
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    TransitionToState(PlayerState.SPRINT);
                }
                else if (dirInput == Vector2.zero)
                {
                    TransitionToState(PlayerState.WALK);
                }
                break;
            case PlayerState.SPRINT:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    TransitionToState(PlayerState.JUMPING);
                }
                if (Input.GetKeyUp(KeyCode.LeftShift))
                {
                    TransitionToState(PlayerState.WALK);
                }
                if (dirInput == Vector2.zero)
                {
                    TransitionToState(PlayerState.IDLE);
                }
                break;
            case PlayerState.ATTACKING:
                break;
            case PlayerState.JUMPING:
                break;
            case PlayerState.DEATH:
                break;
            default:
                break;
        }
    }
    void OnStateLeave()
    {
        switch (currentState)
        {
            case PlayerState.IDLE:
                break;
            case PlayerState.WALK:
                break;
            case PlayerState.SPRINT:
                break;
            case PlayerState.ATTACKING:
                break;
            case PlayerState.JUMPING:
                break;
            case PlayerState.DEATH:
                break;
            default:
                break;
        }
    }


    void TransitionToState(PlayerState newState)
    {
        OnStateLeave();
        currentState = newState;
        OnStateEnter();
    }
}
