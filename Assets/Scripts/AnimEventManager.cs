using UnityEngine;

public class AnimEventManager : MonoBehaviour
{
    public void AttackHit()
    {
        GetComponentInParent<EnemyBehavior>().Attack();
    }

    public void destroyEvent()
    {
        Destroy(gameObject);
    }
}
