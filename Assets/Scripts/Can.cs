using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Can : MonoBehaviour
{
    //LINKS
    Rigidbody rb;

    //STATS
    float flightSpeed = 15f;

    //CONTROL
    bool onTheGround;
    float targetY;
    float offSetY = 1f;

    [SerializeField] AnimationCurve curve;
    private void Start()
    {
        targetY = transform.position.y - offSetY;
        rb = GetComponent<Rigidbody>();


        rb.velocity = new Vector3(.45f, .45f, 0) * flightSpeed;
    }

    private void Update()
    {
        if(transform.position.y < targetY)
        {
            rb.velocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezePosition;
            onTheGround = true;
        }
        
    }
}
