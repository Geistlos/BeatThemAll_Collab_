using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    int score0 = 100;
    int score1 = 500;
    int score;

    [SerializeField] RuntimeAnimatorController animator0;
    [SerializeField] RuntimeAnimatorController animator1;

    private void Awake()
    {
        var rand = Random.Range(0, 2);
        switch (rand)
        {
            case 0:
                transform.GetComponentInChildren<Animator>().runtimeAnimatorController = animator0;
                score = score0;
                break;
            case 1:
                transform.GetComponentInChildren<Animator>().runtimeAnimatorController = animator1;
                score = score1;
                break;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Hit player collec");
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.parent.parent.GetComponent<PlayerBehavior>().IncreaseScore(score);
            Destroy(gameObject);
        }
    }
}
