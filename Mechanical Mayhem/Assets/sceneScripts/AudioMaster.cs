using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMaster : MonoBehaviour
{

    //change script so that it takes to every other object audio script

    public static AudioMaster Instance;

    public Audio[] audios;

    // Start is called before the first frame update
    void Awake()
    {
        if(this != Instance && Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

    }

    private void Start()
    {
        foreach (Audio s in audios)
        {
            s.thisAudioSource = gameObject.AddComponent<AudioSource>();
            s.thisAudioSource.clip = s.GetAudioClip();

            s.thisAudioSource.pitch = s.GetPitch();
            s.thisAudioSource.volume = s.GetVolume();
            s.thisAudioSource.loop = s.GetIsLooping();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        



    }

    public void PlayAudio(string name)
    {
        //checks if the audio clip exists in the array then plays the audio
        Audio audio = Array.Find(audios, audios => audios.GetName() == name);
        audio.thisAudioSource.Play();

    }

}
