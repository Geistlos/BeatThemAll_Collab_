using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Can : MonoBehaviour
{
    //LINKS
    Rigidbody2D rb;
    SpriteRenderer sr;
    BoxCollider2D _collider;

    [SerializeField] Sprite redCan;
    [SerializeField] Sprite blueCan;
    [SerializeField] Sprite greenCan;
    [SerializeField] GameObject shadow;

    //STATS
    float flightSpeed = 20f;
    float fallSpeed = 1f;
    int numberOfBlinks = 5;
    float delayBetweenBlinks = .2f;
    float delayBeforeBlink = 5f;
    float radiusHitbox = .5f;

    //CONTROL
    public bool onTheGround;
    public bool carried;
    float targetY = -Mathf.Infinity;
    float offSetY = 1.5f;
    public bool droped = true;
    public bool randomColor = true;
    bool inTheAir;

    public enum canColor
    {
        red,
        green,
        blue
    }
    canColor _canColor;

    [SerializeField] AnimationCurve curve;
    private void Start()
    {
        sr = transform.GetComponent<SpriteRenderer>();
        _collider = gameObject.GetComponent<BoxCollider2D>();

        if (randomColor)
            _canColor = (canColor)Random.Range(0, 3);
        else
        {
            _canColor = canColor.green;
        }
        switch (_canColor)
        {
            case canColor.red:
                sr.sprite = redCan;
                break;
            case canColor.green:
                sr.sprite = greenCan;
                break;
            case canColor.blue:
                sr.sprite = blueCan;
                break;
            default:
                break;
        }
        rb = GetComponent<Rigidbody2D>();
        if (droped)
        {
            ThrowCan(false);
        }
        //hitGround();
        if (carried == false &&
            Camera.main.WorldToViewportPoint(transform.position).x <= 1 &&
            Camera.main.WorldToViewportPoint(transform.position).x >= 0 &&
            Camera.main.WorldToViewportPoint(transform.position).y <= 1 &&
            Camera.main.WorldToViewportPoint(transform.position).y >= 0)
        {
            hitGround();
        }
    }

    private void Update()
    {
        //WHEN THE CAN HIT THE GROUND
        if (!carried && !onTheGround && transform.position.y < targetY)
        {
            hitGround();
        }

        //DETECTION PICK UP PLAYER
        if (Input.GetMouseButtonDown(1))
        {
            pickUpCan();
        }

        if (onTheGround == false &&
            carried == false &&
            inTheAir == false &&
            Camera.main.WorldToViewportPoint(transform.position).x <= 1 &&
            Camera.main.WorldToViewportPoint(transform.position).x >= 0 &&
            Camera.main.WorldToViewportPoint(transform.position).y <= 1 &&
            Camera.main.WorldToViewportPoint(transform.position).y >= 0)
        {
            Debug.Log("See can");
            hitGround();
        }
        //SHADOW
        /*if (transform.position.y <= targetY + 1)
        {
            Debug.Log("Shadow Scale = " + (transform.position.y - targetY));
            shadow.transform.localScale = new Vector3(transform.position.y - targetY, (transform.position.x - targetY) / 2, 0);
        }*/
    }

    void pickUpCan()
    {
        Collider2D[] Colliders;

        Colliders = Physics2D.OverlapCircleAll(transform.position, radiusHitbox);

        foreach (Collider2D collider in Colliders)
        {
            if (collider.tag == "Player")
            {
                Debug.Log("Hit Player");
                var player = collider.transform.parent.parent.GetComponent<PlayerBehavior>();
                if (_canColor == canColor.red)
                {
                    player.GetHealth();
                    Destroy(gameObject);
                }
                if (_canColor == canColor.green && !player.IsPlayerHoldingACan())
                {
                    player.PickUpCan();
                    Destroy(gameObject);
                    carried = true;
                }
                if (_canColor == canColor.blue)
                {
                    player.GetEnergy();
                    Destroy(gameObject);
                }
            }
        }
    }

    void hitGround()
    {
        inTheAir = false;
        rb.velocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        onTheGround = true;
        transform.parent = null;
        _collider.isTrigger = false;
        StartCoroutine(BlinkGameObject(this.gameObject, numberOfBlinks, delayBetweenBlinks, delayBeforeBlink));
    }

    //THROW CAN, CALLED BY THE PLAYER. GET ORIENTATION FROM IT
    public void ThrowCan(bool flipped)
    {
        inTheAir = true;
        rb.constraints = RigidbodyConstraints2D.None;
        carried = false;
        transform.parent = null;

        _collider.isTrigger = true;

        if (!droped)
        {
            targetY = transform.position.y - offSetY;

            if (!flipped)
            {
                rb.velocity = new Vector3(.55f, -.20f, 0) * flightSpeed;
            }
            else
            {
                rb.velocity = new Vector3(-.55f, -.20f, 0) * flightSpeed;
            }
        }
        else if (droped)
        {
            targetY = 0;
            rb.velocity = Vector3.down * fallSpeed;
        }
    }

    //BLINK
    public IEnumerator BlinkGameObject(GameObject gameObject, int numBlinks, float seconds, float delayBeforeBlink)
    {
        yield return new WaitForSeconds(2f);

        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();



        for (int i = 0; i < numBlinks * 2; i++)
        {
            renderer.enabled = !renderer.enabled;
            yield return new WaitForSeconds(seconds);
        }

        Destroy(gameObject);
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision");
        if (collision.gameObject.tag == "Enemy")
        {
            if (!onTheGround && !carried)
            {
                collision.gameObject.GetComponent<EnemyBehavior>().TakeDamage(1);
            }
        }
    }*/
}
