using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Audio
{

    [SerializeField] string audioName = "default";

    [SerializeField] AudioClip clip;

    [Range(0, 3)]
    [SerializeField] float pitch = 1;
    [Range(0, 1)]
    [SerializeField] float volume = 1;
    [SerializeField] bool isLooping = false;


    public AudioSource thisAudioSource;
    

    //Getters
    public float GetPitch()
    {
        return pitch;
    }
    public float GetVolume()
    {
        return volume;
    } 
    public bool GetIsLooping()
    {
        return isLooping;
    }
    public AudioClip GetAudioClip() 
    {
        return clip;
    }
    public string GetName()
    {
        return audioName;
    }

    public void setAudioToAudioSource()
    {
        thisAudioSource.clip = this.clip;
    }



}

[System.Serializable]
public class WeaponAudio : Audio
{
    public enum AudioType
    {
        none,
        firingGun,
        reload,

    }

    [SerializeField] AudioType audioType;
    
    public AudioType GetAudioType() { return this.audioType; }

    public void SetAudioType(WeaponAudio.AudioType audioType) { this.audioType = audioType; }

}
