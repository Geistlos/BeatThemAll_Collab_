using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEventManager : MonoBehaviour
{
    public void AttackHit()
    {
        GetComponentInParent<EnemyBehavior>().Attack();
    }
}
