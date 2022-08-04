using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag is ("Player"))
        {
            GetComponentInParent<EnemyBehavior>().PlayerDetected(collision.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag is ("Player"))
        {
            GetComponentInParent<EnemyBehavior>().PlayerLost();
        }
    }
}
