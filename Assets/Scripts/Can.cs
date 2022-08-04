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
    float targetY;
    float offSetY = 1f;
    public bool flipped;

    [SerializeField] AnimationCurve curve;
    private void Start()
    {
        targetY = transform.position.y - offSetY;
        rb = GetComponent<Rigidbody>();

        if (!flipped)
        {
            rb.velocity = new Vector3(.85f, -.1f, 0) * flightSpeed;
        }
        else
        {
            rb.velocity = new Vector3(-.85f, -.1f, 0) * flightSpeed;
        }
    }

    private void Update()
    {
        if (!onTheGround && transform.position.y < targetY)
        {
            rb.velocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezePosition;
            onTheGround = true;
        }

    }
}
