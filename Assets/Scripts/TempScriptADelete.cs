using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempScriptADelete : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag is ("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyBehavior>().TakeDamage(1);
        }
    }
}
