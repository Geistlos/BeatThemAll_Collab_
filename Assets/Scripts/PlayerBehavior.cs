using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    //LINKS
    [SerializeField] AnimationCurve curve;
    [SerializeField] StatsScriptable stats;
    [SerializeField] GameObject canPosition;
    [SerializeField] GameObject canPrefab;

    //STATE MACHINE
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

    //COMPONENTS / GAMEOBJECTS
    Animator animator;
    Rigidbody rb;
    Transform graphics;


    //CONTROL
    float jumpTimer;
    bool flipped;
    bool isHoldingCan;
    bool onTheGround = true;

    //STATS
    float life;
    float dmg;
    float walkingSpeed;
    float runningSpeed;
    float airTime;
    float jumpHeight;
    float jumpingSpeed;
    float attackDelay;

    //INPUTS
    Vector2 dirInput;

    //RANDOM ATTACK ANIMATION
    List<float> randomAtk = new List<float> { 0f, 0.25f, 0.50f, .75f };

    private void Start()
    {
        //GET STATS
        life = stats.life;
        dmg = stats.dmg;
        walkingSpeed = stats.speed;
        runningSpeed = stats.runningSpeed;
        airTime = stats.airTime;
        jumpHeight = stats.jumpHeight;
        jumpingSpeed = stats.jumpHeight;
        attackDelay = stats.attackSpeed;

        //LINKS TO GAMEOBJECT / COMPONENTS
        graphics = GetComponentInChildren<Transform>();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {

        dirInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        //PLAYER DIRECTION
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

        //STATE MACHINE UPDATE
        OnStateUpdate();

        //TODO / TEMPS
        if (Input.GetKeyDown(KeyCode.H))
        {
            isHoldingCan = true;
            animator.SetFloat("Can", 1);
            canPosition.SetActive(true);
            var obj = Instantiate(canPrefab, canPosition.transform.position, Quaternion.identity);
            obj.GetComponent<Can>().flipped = flipped;
            /*if (flipped)
            {
                Instantiate(canPrefab, canPosition.transform.position, transform.rotation * Quaternion.Euler(0f, 180f, 0f));
                Debug.Log("rotation: " + transform.rotation * Quaternion.Euler(0f, 180f, 0f));
            }
            else
            {
                Instantiate(canPrefab, canPosition.transform.position, transform.rotation * Quaternion.Euler(0f, 0f, 0f));
                Debug.Log("rotation: " + transform.rotation * Quaternion.Euler(0f, 0f, 0f));
            }*/
        }

        if (!onTheGround)
        {
            JumpAnimation();
        }
    }

    void FixedUpdate()
    {
        //STATE MACHINE FIXED UPDATE
        OnStateFixedUpdate();
    }

    //CALLED WHEN ENTERING NEW STATE
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
                rb.velocity = Vector2.zero;
                animator.SetTrigger("Attacking");
                //PICK ON RANDOM ATTACK ANIMATION IF NOT HOLDING A CAN
                if (!isHoldingCan)
                {
                    animator.SetFloat("Can", randomAtk[Random.Range(0, 4)]);
                }
                //WAIT FOR ANIMATION END + DELAY BEFORE SWITCHING STATE
                StartCoroutine(waitAnimationEnd(attackDelay, PlayerState.IDLE));
                break;
            case PlayerState.JUMPING:
                animator.SetBool("IsJumping", true);
                break;
            case PlayerState.DEATH:
                break;
            case PlayerState.JUMPATTACK:
                animator.SetTrigger("Attacking");
                //PICK ON RANDOM ATTACK ANIMATION IF NOT HOLDING A CAN
                if (!isHoldingCan)
                {
                    animator.SetFloat("Can", randomAtk[Random.Range(0, 4)]);
                }
                //WAIT FOR ANIMATION END + DELAY BEFORE SWITCHING STATE
                StartCoroutine(waitAnimationEnd(attackDelay, PlayerState.IDLE));
                break;
            default:
                break;
        }
    }

    //WAIT FOR ANIMATION END + DELAY BEFORE SWITCHING STATE
    IEnumerator waitAnimationEnd(float time, PlayerState targetState)
    {
        yield return new WaitForSeconds(time);
        if (!isHoldingCan)
        {
            animator.SetFloat("Can", 0);
        }
        TransitionToState(targetState);
    }

    //STATE MACHINE FIXED UPDATE
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

    //STATE MACHINE UPDATE
    void OnStateUpdate()
    {
        switch (currentState)
        {
            case PlayerState.IDLE:
                //IDLE > JUMP
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    TransitionToState(PlayerState.JUMPING);
                }
                //IDLE > SPRINT
                if (dirInput != Vector2.zero && Input.GetKeyDown(KeyCode.LeftShift))
                {
                    TransitionToState(PlayerState.SPRINT);
                }
                //IDLE > ATTACK
                if (Input.GetMouseButtonDown(0))
                {
                    TransitionToState(PlayerState.ATTACKING);
                }
                //IDLE > WALK
                else if (dirInput != Vector2.zero)
                {
                    TransitionToState(PlayerState.WALK);
                }
                break;
            case PlayerState.WALK:
                //WALK > JUMP
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    TransitionToState(PlayerState.JUMPING);
                }
                //WALK > SPRINT
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    TransitionToState(PlayerState.SPRINT);
                }
                //WALK > ATTACK
                if (Input.GetMouseButtonDown(0))
                {
                    TransitionToState(PlayerState.ATTACKING);
                }
                //WALK > IDLE
                else if (dirInput == Vector2.zero)
                {
                    TransitionToState(PlayerState.IDLE);
                }
                break;
            case PlayerState.SPRINT:
                //SPRINT > JUMP
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    TransitionToState(PlayerState.JUMPING);
                }
                //SPRINT > WALK
                if (Input.GetKeyUp(KeyCode.LeftShift))
                {
                    TransitionToState(PlayerState.WALK);
                }
                //SPRINT > ATTACK
                if (Input.GetMouseButtonDown(0))
                {
                    TransitionToState(PlayerState.ATTACKING);
                }
                //SPRINT > IDLE
                if (dirInput == Vector2.zero)
                {
                    TransitionToState(PlayerState.IDLE);
                }
                break;
            case PlayerState.ATTACKING:
                //STOP THE PLAYER WHILE ATTACKING
                rb.velocity = Vector2.zero;
                break;
            case PlayerState.JUMPING:
                //CONTROL JUMPING ANIMATION
                onTheGround = false;
                //AIR ATTACK
                if (Input.GetKeyDown(KeyCode.Mouse0) && !isHoldingCan)
                {
                    TransitionToState(PlayerState.JUMPATTACK);
                }
                break;
            //PLAYER DEATH
            case PlayerState.DEATH:
                break;
            //JUMPING ATTACK
            case PlayerState.JUMPATTACK:
                //CONTROL JUMPING ANIMATION
                onTheGround = false;
                break;
            default:
                break;
        }
    }

    //CALLED ON LEAVING A STATE
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
                animator.SetBool("IsJumping", false);
                break;
            case PlayerState.JUMPING:
                animator.SetBool("IsJumping", false);
                break;
            case PlayerState.DEATH:
                break;
            case PlayerState.JUMPATTACK:
                animator.SetBool("IsJumping", false);
                break;
            default:
                break;
        }
    }

    //CALLED TO TRANSITION BETWEEN TWO STATES
    void TransitionToState(PlayerState newState)
    {
        OnStateLeave();
        currentState = newState;
        OnStateEnter();
    }

    //CONTROL JUMPING 
    public void JumpAnimation()
    {
        if (jumpTimer < airTime)
        {
            jumpTimer += Time.deltaTime;
            float y = curve.Evaluate(jumpTimer / airTime);
            graphics.localPosition = new Vector3(transform.localPosition.x, y * jumpHeight, transform.localPosition.z);
        }
        else
        {
            onTheGround = true;
            jumpTimer = 0f;
            TransitionToState(PlayerState.IDLE);
        }
    }
}
