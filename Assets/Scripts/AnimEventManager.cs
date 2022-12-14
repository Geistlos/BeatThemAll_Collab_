using UnityEngine;

public class AnimEventManager : MonoBehaviour
{
    bool moving;
    float speed = 13f;
    Vector3 direction;

    public void AttackHit()
    {
        GetComponentInParent<EnemyBehavior>().Attack();
    }

    public void destroyEvent()
    {
        Destroy(gameObject);
    }

    public void Direction(Vector3 _direction)
    {
        moving = true;
        direction = _direction;
    }

    private void Update()
    {
        if (moving)
            transform.Translate(direction * Time.deltaTime * speed, Space.World);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null && collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyBehavior>().TakeDamage(10);
        }
    }
}
