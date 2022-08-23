using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    //LINKS
    [SerializeField] AnimationCurve curve;
    [SerializeField] StatsScriptable stats;
    [SerializeField] GameObject canPosition;
    [SerializeField] GameObject hitPosition;
    [SerializeField] GameObject canPrefab;
    [SerializeField] GameObject healthBarCanevas;
    [SerializeField] GameObject energyBarCanevas;
    [SerializeField] GameObject shadow;

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
    Rigidbody2D rb;
    Transform graphics;
    HealthBar _healthBar;
    EnergyBar _energyBar;

    //CONTROL
    float jumpTimer;
    bool flipped;
    bool isHoldingCan;
    bool onTheGround = true;
    bool invulnerability;

    //STATS
    int playerLife;
    float playerEnergy;
    int playerDmg;
    float walkingSpeed;
    float runningSpeed;
    float airTime;
    float jumpHeight;
    float jumpingSpeed;
    float attackDelay;
    float invulnerabilityDuration;
    float radiusHitbox;
    RuntimeAnimatorController _animator;
    int numberOfBlinks;
    float delayBetweenBlinks;
    int healthPerCan;
    int energyPerCan;
    float energyPetAtk;

    //INPUTS
    Vector2 dirInput;

    //RANDOM ATTACK ANIMATION
    List<float> randomAtk = new List<float> { 0f, 0.25f, 0.50f, .75f };

    private void Start()
    {
        //GET STATS
        playerLife = stats.life;
        playerEnergy = stats.energy;
        playerDmg = stats.dmg;
        walkingSpeed = stats.speed;
        runningSpeed = stats.runningSpeed;
        airTime = stats.airTime;
        jumpHeight = stats.jumpHeight;
        jumpingSpeed = stats.jumpHeight;
        attackDelay = stats.attackSpeed;
        invulnerabilityDuration = stats.invulnerabilityDuration;
        radiusHitbox = stats.radiusHitbox;
        _animator = stats.animator;
        numberOfBlinks = stats.numberOfBlinks;
        delayBetweenBlinks = stats.delayBetweenBlinks;
        healthPerCan = stats.healthPerCan;
        energyPerCan = stats.energyPerCan;
        energyPetAtk = stats.energyPerAtk;

        transform.GetComponentInChildren<Animator>().runtimeAnimatorController = _animator;

        //LINKS TO GAMEOBJECT / COMPONENTS
        graphics = this.gameObject.transform.GetChild(0).transform;
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        //SET UI
        _healthBar = healthBarCanevas.GetComponentInChildren<HealthBar>();
        _healthBar.SetMaxHealth(playerLife);
        _energyBar = energyBarCanevas.GetComponentInChildren<EnergyBar>();
        _energyBar.SetMaxEnergy(playerEnergy);
        playerEnergy = 0;
    }

    private void Update()
    {
        dirInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        //PLAYER DIRECTION
        if (dirInput.x < 0 && !flipped && currentState != PlayerState.DEATH)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            flipped = true;
        }
        else if (dirInput.x > 0 && flipped && currentState != PlayerState.DEATH)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            flipped = false;
        }

        //STATE MACHINE UPDATE
        OnStateUpdate();

        //TODO / TEMPS PICK A CAN
        if (Input.GetKeyDown(KeyCode.H))
        {
            isHoldingCan = true;
            animator.SetFloat("Can", 1);
            canPosition.SetActive(true);
            var obj = Instantiate(canPrefab, canPosition.transform.position, Quaternion.identity);
            obj.transform.parent = canPosition.transform;
        }

        //TODO / GENERATE CAN
        if (Input.GetKeyDown(KeyCode.J))
        {
            var playerX = transform.position.x;
            var obj = Instantiate(canPrefab, new Vector3(Random.Range(playerX - 10, playerX + 10), 15, 0), Quaternion.identity);
            obj.GetComponent<Can>().droped = true;
        }

        //TODO / TEMPS TAKE DMG
        /*if (Input.GetKeyDown(KeyCode.J))
        {
            TakeHit(playerDmg);
        }*/

        //GESTION DE LA HAUTEUR DU JOUEUR POUR QU'IL RETOUCHE LE SOL
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

    #region ON STATE ENTER
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
                else
                {
                    isHoldingCan = false;
                    canPosition.GetComponentInChildren<Can>().ThrowCan(flipped);
                    //canPosition.transform.parent = null;
                }
                Attack();
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
                Attack();
                //WAIT FOR ANIMATION END + DELAY BEFORE SWITCHING STATE
                StartCoroutine(waitAnimationEnd(attackDelay, PlayerState.IDLE));
                break;
            default:
                break;
        }
    }
    #endregion

    #region ON STATE FIXED UPDATE
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
    #endregion

    #region ON STATE UPDATE
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
    #endregion

    #region ON STATE LEAVE
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
    #endregion

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
            var y = curve.Evaluate(jumpTimer / airTime);
            graphics.localPosition = new Vector3(graphics.localPosition.x, y * jumpHeight, graphics.localPosition.z);
            var shadowScale = 1 - y;
            shadow.transform.localScale = new Vector3(shadowScale, shadowScale / 2, shadowScale);
        }
        else
        {
            onTheGround = true;
            jumpTimer = 0f;
            TransitionToState(PlayerState.IDLE);
        }
    }

    //PLAYER ATTACK
    public void Attack()
    {
        Collider2D[] Colliders;

        Colliders = Physics2D.OverlapCircleAll(hitPosition.transform.position, radiusHitbox);

        foreach (Collider2D collider in Colliders)
        {
            if (collider.tag == "Enemy")
            {
                collider.GetComponent<EnemyBehavior>().TakeDamage(playerDmg);
                playerEnergy += energyPetAtk;
                _energyBar.SetEnergy(playerEnergy);
                Debug.Log("HIT");
            }
        }
    }


    public bool IsPlayerHoldingACan()
    {
        if (isHoldingCan)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void PickUpCan()
    {
        if (isHoldingCan)
        {
            return;
        }
        Debug.Log("Pick up can");
        isHoldingCan = true;
        animator.SetFloat("Can", 1);
        canPosition.SetActive(true);
        var obj = Instantiate(canPrefab, canPosition.transform.position, Quaternion.identity);
        obj.transform.parent = canPosition.transform;
        obj.GetComponent<Can>().randomColor = false;
    }

    //GIVE PLAYER ENERGY WHEN PICKING UP A CAN
    public void GetEnergy()
    {
        playerEnergy += energyPerCan;
        _energyBar.SetEnergy(playerEnergy);
    }

    //HEAL PLAYER WHEN PICKING UP A CAN
    public void GetHealth()
    {
        playerLife += healthPerCan;
        _healthBar.SetHealth(playerLife);
    }

    //PLAYER TAKE DMG
    public void TakeHit(int dmgTaken)
    {
        if (!invulnerability)
        {
            playerLife -= dmgTaken;
            _healthBar.SetHealth(playerLife);
            if (playerLife > 0)
            {
                StartCoroutine(startInvulnerabiliy());

                Debug.Log("New Life= " + playerLife);
            }
            else
            {
                TransitionToState(PlayerState.DEATH);
                Debug.Log("You Died");
            }

        }
    }

    //TIMER D'INVULNERABILITE QUAND LE JOUEUR SE FAIT TAPER
    IEnumerator startInvulnerabiliy()
    {
        invulnerability = true;
        StartCoroutine(BlinkGameObject(graphics, numberOfBlinks, delayBetweenBlinks));
        yield return new WaitForSeconds(invulnerabilityDuration);
        invulnerability = false;
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

    //BLINK
    public IEnumerator BlinkGameObject(Transform transform, int numBlinks, float seconds)
    {
        yield return new WaitForSeconds(seconds);

        SpriteRenderer renderer = transform.GetComponent<SpriteRenderer>();

        for (int i = 0; i < numBlinks * 2; i++)
        {
            renderer.enabled = !renderer.enabled;

            yield return new WaitForSeconds(seconds);
        }

        renderer.enabled = true;
    }
}
