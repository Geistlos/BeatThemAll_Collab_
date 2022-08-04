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
        DEATH,
        JUMPATTACK
    }
    public PlayerState currentState;

    Animator animator;
    Rigidbody rb;
    Transform graphics;

    bool playerIsMoving;
    Vector2 dirInput;

    float jumpTimer;
    float attackAnimationDuration;
    bool flipped;
    //TEMPS
    float walkingSpeed = 7;
    float runningSpeed = 14;
    float airTime = 1f;
    float jumpHeight = 1.5f;
    float jumpingSpeed = 7f;

    //WIP CAN
    int can;
    bool isHoldingCan;

    private void Start()
    {
        graphics = GetComponentInChildren<Transform>();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        dirInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (dirInput.x < 0 && !flipped)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
            flipped = true;
        }
        else if (dirInput.x > 0 && flipped)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            flipped = false;
        }
        OnStateUpdate();

        //TODO
        if (Input.GetKeyDown(KeyCode.H))
        {
            can = 1;
            isHoldingCan = true;
            animator.SetFloat("Can", 1);
        }
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
                animator.SetBool("IsRunning", false);
                animator.SetBool("IsMoving", false);
                animator.SetBool("IsJumping", false);
                break;
            case PlayerState.WALK:
                animator.SetBool("IsMoving", true);
                break;
            case PlayerState.SPRINT:
                animator.SetBool("IsRunning", true);
                break;
            case PlayerState.ATTACKING:
                animator.SetTrigger("Attacking");
                attackAnimationDuration = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
                StartCoroutine(waitAnimationEnd(attackAnimationDuration, PlayerState.IDLE));
                break;
            case PlayerState.JUMPING:
                animator.SetTrigger("Jump");
                animator.SetBool("IsJumping", true);
                break;
            case PlayerState.DEATH:
                break;
            case PlayerState.JUMPATTACK:
                animator.SetTrigger("Attacking");
                attackAnimationDuration = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
                StartCoroutine(waitAnimationEnd(attackAnimationDuration, PlayerState.JUMPING));
                break;
            default:
                break;
        }
    }

    IEnumerator waitAnimationEnd(float time, PlayerState targetState)
    {
        yield return new WaitForSeconds(time);
        TransitionToState(targetState);
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
            case PlayerState.JUMPATTACK:
                rb.velocity = dirInput.normalized * jumpingSpeed;
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
                if (Input.GetMouseButtonDown(0))
                {
                    TransitionToState(PlayerState.ATTACKING);
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
                if (Input.GetMouseButtonDown(0))
                {
                    TransitionToState(PlayerState.ATTACKING);
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
                if (Input.GetMouseButtonDown(0))
                {
                    TransitionToState(PlayerState.ATTACKING);
                }
                if (dirInput == Vector2.zero)
                {
                    TransitionToState(PlayerState.IDLE);
                }
                break;
            case PlayerState.ATTACKING:
                break;
            case PlayerState.JUMPING:
                if (jumpTimer < airTime)
                {
                    jumpTimer += Time.deltaTime;
                    float y = curve.Evaluate(jumpTimer / airTime);
                    graphics.localPosition = new Vector3(transform.localPosition.x, y * jumpHeight, transform.localPosition.z);

                }
                else
                {
                    jumpTimer = 0f;
                    TransitionToState(PlayerState.IDLE);
                }
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    TransitionToState(PlayerState.JUMPATTACK);
                }
                break;
            case PlayerState.DEATH:
                break;
            case PlayerState.JUMPATTACK:
                if (jumpTimer < airTime)
                {
                    jumpTimer += Time.deltaTime;
                    float y = curve.Evaluate(jumpTimer / airTime);
                    graphics.localPosition = new Vector3(transform.localPosition.x, y * jumpHeight, transform.localPosition.z);

                }
                else
                {
                    jumpTimer = 0f;
                    TransitionToState(PlayerState.JUMPING);
                }
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
