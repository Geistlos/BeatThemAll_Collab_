using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource audio1;
    [SerializeField] AudioSource audio2;
    [SerializeField] float fadeDuration;
    [SerializeField] AnimationCurve animationCurve;

    public void ChangerMusique(bool battle)
    {
        StartCoroutine(battle ? FadeSound(audio1, audio2) : FadeSound(audio2, audio1));
    }

    IEnumerator FadeSound(AudioSource toStop, AudioSource toPlay)
    {
        toPlay.Play();

        float volume1 = toStop.volume;
        float volume2 = toPlay.volume;

        float t = 0;

        while (t < fadeDuration)
        {
            toStop.volume = Mathf.Lerp(volume1, 0, animationCurve.Evaluate(t / fadeDuration));
            toPlay.volume = Mathf.Lerp(volume2, 1, animationCurve.Evaluate(t / fadeDuration));

            t += Time.deltaTime;
            yield return null;
        }

        toStop.volume = 0;
        toPlay.volume = 1;

        toStop.Stop();

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
