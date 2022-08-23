using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    int score0 = 100;
    int score1 = 500;
    int numberOfBlinks = 5;
    float delayBetweenBlinks = .2f;
    float delayBeforeBlink = 8f;
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
        StartCoroutine(BlinkGameObject(gameObject, numberOfBlinks, delayBetweenBlinks, delayBeforeBlink));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.parent.parent.GetComponent<PlayerBehavior>().IncreaseScore(score);
            Destroy(gameObject);
        }
    }

   

    //BLINK
    public IEnumerator BlinkGameObject(GameObject gameObject, int numBlinks, float seconds, float delayBeforeBlink)
    {
        yield return new WaitForSeconds(delayBeforeBlink);

        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();

        for (int i = 0; i < numBlinks * 2; i++)
        {
            renderer.enabled = !renderer.enabled;
            yield return new WaitForSeconds(seconds);
        }
        Destroy(gameObject);
    }
}
