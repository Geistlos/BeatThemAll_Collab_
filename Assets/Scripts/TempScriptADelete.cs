using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempScriptADelete : MonoBehaviour
{
    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag is ("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyBehavior>().TakeDamage(1);
        }
    }
    */

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (collision.gameObject.tag is ("Enemy"))
            {
                collision.gameObject.GetComponent<EnemyBehavior>().TakeDamage(1);
            }
        }
    }
}
