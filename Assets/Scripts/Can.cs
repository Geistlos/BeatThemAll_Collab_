using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Can : MonoBehaviour
{
    //LINKS
    Rigidbody rb;

    //STATS
    float flightSpeed = 20f;
    float throwAngle = 0.20f;
    int numberOfBlinks = 5;
    float delayBetweenBlinks = .2f;

    //CONTROL
    bool onTheGround;
    bool carried;
    float targetY = -Mathf.Infinity;
    float offSetY = 1.5f;

    [SerializeField] AnimationCurve curve;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //WHEN THE CAN HIT THE GROUND
        if (!carried && !onTheGround && transform.position.y < targetY)
        {
            rb.velocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezePosition;
            onTheGround = true;
            transform.parent = null;
            Debug.Log("Drop Can");
            StartCoroutine(BlinkGameObject(this.gameObject, numberOfBlinks, delayBetweenBlinks));
        }

    }

    //THROW CAN, CALLED BY THE PLAYER. GET ORIENTATION FROM IT
    public void ThrowCan( bool flipped)
    {
        Debug.Log("Throw Can");
        rb.constraints = RigidbodyConstraints.None;
        carried = false;
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

    //BLINK
    public IEnumerator BlinkGameObject(GameObject gameObject, int numBlinks, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        // In this method it is assumed that your game object has a SpriteRenderer component attached to it
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        // disable animation if any animation is attached to the game object
        //      Animator animator = gameObject.GetComponent<Animator>();
        //      animator.enabled = false; // stop animation for a while
        for (int i = 0; i < numBlinks * 2; i++)
        {
            //toggle renderer
            renderer.enabled = !renderer.enabled;
            //wait for a bit
            yield return new WaitForSeconds(seconds);
        }
        //make sure renderer is enabled when we exit
        Destroy(gameObject);    
        //renderer.enabled = true;
        //    animator.enabled = true; // enable animation again, if it was disabled before
    }
}
