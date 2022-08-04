using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    //[SerializeField] GameObject graphics;
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
    Transform graphics;

    bool playerIsMoving;
    Vector2 dirInput;

    float jumpTimer;
    //TEMPS
    float walkingSpeed = 7;
    float runningSpeed = 14;
    float airTime = 1f;
    float jumpHeight = 1.5f;
    float jumpingSpeed = 10f;

    private void Start()
    {
        graphics = GetComponentInChildren<Transform>();
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
                rb.velocity = Vector2.zero;
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
                rb.velocity = dirInput.normalized * jumpingSpeed;
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
                    TransitionToState(PlayerState.IDLE);
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
                Debug.Log("JumpTimer: " + jumpTimer);
                Debug.Log("airTime: " + airTime);
                if (jumpTimer < airTime)
                {
                    Debug.Log("Jumping");
                    jumpTimer += Time.deltaTime;
                    float y = curve.Evaluate(jumpTimer / airTime);
                    graphics.localPosition = new Vector3(transform.localPosition.x, y * jumpHeight, transform.localPosition.z);

                }
                else
                {
                    Debug.Log("End of jump");
                    jumpTimer = 0f;
                    TransitionToState(PlayerState.IDLE);
                }
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
        Debug.Log("Leave state: " + currentState);
        Debug.Log("Enter state: " + newState);
        OnStateLeave();
        currentState = newState;
        OnStateEnter();
    }
}
