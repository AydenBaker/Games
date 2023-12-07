using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayAudioScript : MonoBehaviour
{

    public AudioSource audioSource;
    public float timeDelay;


    void Start()
    {
        StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(timeDelay);

        audioSource.Play();
    }
}
