using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Can : MonoBehaviour
{
    //LINKS
    Rigidbody rb;
    SpriteRenderer sr;
    [SerializeField] Sprite redCan;
    [SerializeField] Sprite blueCan;
    [SerializeField] Sprite greenCan;
    [SerializeField] GameObject shadow;

    //STATS
    float flightSpeed = 20f;
    float fallSpeed = 1f;
    int numberOfBlinks = 5;
    float delayBetweenBlinks = .2f;
    float delayBeforeBlink = 2f;
    float radiusHitbox = .5f;

    //CONTROL
    bool onTheGround;
    public bool carried;
    float targetY = -Mathf.Infinity;
    float offSetY = 1.5f;
    int startY;
    public bool droped;
    public bool randomColor = true;

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
        if (randomColor)
            _canColor = (canColor)Random.Range(0, 2);
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
        rb = GetComponent<Rigidbody>();
        if (droped)
        {
            ThrowCan(false);
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
        if (Input.GetKeyDown(KeyCode.N))
        {
            pickUpCan();
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
        rb.velocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezePosition;
        onTheGround = true;
        transform.parent = null;
        StartCoroutine(BlinkGameObject(this.gameObject, numberOfBlinks, delayBetweenBlinks, delayBeforeBlink));
    }

    //THROW CAN, CALLED BY THE PLAYER. GET ORIENTATION FROM IT
    public void ThrowCan(bool flipped)
    {
        rb.constraints = RigidbodyConstraints.None;
        carried = false;
        transform.parent = null;

        if (!droped)
        {
            targetY = transform.position.y - offSetY;

            if (!flipped)
            {
                rb.velocity = new Vector3(.85f, -.1f, 0) * flightSpeed;
            }
            else
            {
                rb.velocity = new Vector3(-.85f, -.1f, 0) * flightSpeed;
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
}
