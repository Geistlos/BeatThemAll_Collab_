using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Can : MonoBehaviour
{
    //LINKS
    Rigidbody rb;

    //STATS
    float flightSpeed = 20f;
    float fallSpeed = 1f;
    //float throwAngle = 0.20f;
    int numberOfBlinks = 5;
    float delayBetweenBlinks = .2f;
    float delayBeforeBlink = 2f;

    //CONTROL
    bool onTheGround;
    public bool carried;
    float targetY = -Mathf.Infinity;
    float offSetY = 1.5f;
    int startY;
    public bool droped;

    [SerializeField] AnimationCurve curve;
    private void Start()
    {
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
