using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideDoorMovement : MonoBehaviour
{
    public bool deleteWhenOpen;
    public bool startOpen;
    public AudioSource audio;

    private bool HasAudioPlayed = false; 
    private Animator openClose;

    
    // Start is called before the first frame update
    void Start()
    {
        openClose = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        if (startOpen)
        {
            openClose.Play("Door Slide");
        }
    }

    public void StartOpen()
    {
        if (!HasAudioPlayed)
        {
            audio.Play();
            HasAudioPlayed = true;
        }

        openClose.Play("Door Slide");

    }
    
    public void StartClose()
    {
        if (!HasAudioPlayed)
        {
            audio.Play();
            HasAudioPlayed = true;
        }

        openClose.Play("Door Slide Close");
    }

    public void SetHasAudioPlayed(bool HasAudioPlayed)
    {
        this.HasAudioPlayed = HasAudioPlayed;
    }
    
}
