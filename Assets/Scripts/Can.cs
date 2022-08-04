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

    //CONTROL
    bool onTheGround;
    bool carried;
    float targetY;
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
        }

    }

    //THROW CAN, CALLED BY THE PLAYER. GET ORIENTATION FROM IT
    public void ThrowCan( bool flipped)
    {
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
}
