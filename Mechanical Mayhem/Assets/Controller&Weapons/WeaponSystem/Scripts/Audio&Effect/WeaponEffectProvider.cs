using System;
using System.Collections;
using UnityEngine;


public class WeaponEffectProvider : MonoBehaviour
{
    //particals
    [SerializeField] private WeaponEffect muzzleFlash;

    //light
    [SerializeField] private Light muzzleLight;

    //audio
    [SerializeField] private WeaponAudio[] audioClips = new WeaponAudio[1];


    //animation reset
    [SerializeField] private Transform[] gunPartTransforms;
    private Vector3[] gunPartPositions;
    private Quaternion[] gunPartRotations;

    //animation
     private Animator animator;

    //get weaponClassRefrence
    private SceneEffectProvider SceneEffects;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //gets our SceneEffectProvider refrence
        SceneEffects = SceneEffectProvider.Instance;

        //it sets weather or not the effect is looping
        muzzleFlash.GetParticleSystem().loop = muzzleFlash.GetisLooping();

        //set audio setting to the audio source
        foreach(WeaponAudio a in audioClips)
        {
            a.thisAudioSource.pitch = a.GetPitch();
            a.thisAudioSource.volume = a.GetVolume();
            a.thisAudioSource.loop = a.GetIsLooping();
            a.thisAudioSource.clip = a.GetAudioClip();
        }

        gunPartPositions = new Vector3[0];
        gunPartRotations = new Quaternion[0];


    }

    public void TurnLightOn()
    {
        if(muzzleLight != null)
        muzzleLight.enabled = true;
        
    }

    public void TurnOffLight()
    {
        if (muzzleLight != null)
        muzzleLight.enabled = false;
    }

    //for particle System effects
    public void PlayEffectType(WeaponEffect.effectType effectType)
    {

        //for regular effects
        switch (effectType)
        {
            case WeaponEffect.effectType.MuzzleFlash:
                PlayEffect(muzzleFlash);
               break;

            case WeaponEffect.effectType.shellCasing://not used for now
                //play shellCasings
                break;
        } 

    }

    public void StopEffectType(WeaponEffect.effectType effectType)
    {

        //for regular effects
        switch (effectType)
        {
            case WeaponEffect.effectType.MuzzleFlash:
                StopEffect(muzzleFlash);
                break;

            case WeaponEffect.effectType.shellCasing://not used for now
                //play shellCasings
                break;
        } 

    }
    
    public GameObject GetImpactEffectForSurface(WeaponEffect.effectType effectType, RaycastHit hit)
    {
        //for different surfaces
        if(effectType == WeaponEffect.effectType.Impact)
        {
            WeaponEffect effect = SceneEffects.GetSurfaceEffect(hit);
            return effect.GetGameObject();

        }
        else
        {
            return null;
        }
    }

    public void PlayEffect(WeaponEffect effect)
    {
        effect.GetParticleSystem().Play();
    }

    public void StopEffect(WeaponEffect effect)
    {
        effect.GetParticleSystem().Stop();
    }

    
    
    //audio
    public void PlayAudio(WeaponAudio.AudioType audioType)
    {
       
        switch (audioType)
        {
            case WeaponAudio.AudioType.firingGun:
                for(int i = 0; i < audioClips.Length; i++)
                {
                    if(audioClips[i].GetAudioType() == WeaponAudio.AudioType.firingGun)
                    {
                        audioClips[i].thisAudioSource.Play();
                    }
                }
                break;

            case WeaponAudio.AudioType.reload:
                for (int i = 0; i < audioClips.Length; i++)
                {
                    if (audioClips[i].GetAudioType() == WeaponAudio.AudioType.reload)
                    {
                        audioClips[i].thisAudioSource.Play();
                    }
                }
                break;

        }

    }

    public void StopAudio(WeaponAudio.AudioType audioType)
    {
        switch (audioType)
        {
            case WeaponAudio.AudioType.firingGun:
                for(int i = 0; i < audioClips.Length; i++)
                {
                    if(audioClips[i].GetAudioType() == WeaponAudio.AudioType.firingGun)
                    {
                        audioClips[i].thisAudioSource.Stop();
                    }
                }
                break;

            case WeaponAudio.AudioType.reload:
                for (int i = 0; i < audioClips.Length; i++)
                {
                    if (audioClips[i].GetAudioType() == WeaponAudio.AudioType.reload)
                    {
                        audioClips[i].thisAudioSource.Stop();
                    }
                }
                break;

        }
    }
    


    //animation
    public void StartReloadAnimation()
    {
        animator.enabled = true;
        animator.SetTrigger("Reload");
        PlayAudio(WeaponAudio.AudioType.reload);
    }

    public void SetGunOrigins()
    {
       

        if(gunPartPositions.Length == 0 && gunPartRotations.Length == 0)
        {
            //gets all of the original transforms for the animator
            gunPartPositions = new Vector3[gunPartTransforms.Length];
            gunPartRotations = new Quaternion[gunPartTransforms.Length];

            for (int t = 0; t < gunPartTransforms.Length; t++)
            {
                gunPartPositions[t] = gunPartTransforms[t].transform.localPosition;
                gunPartRotations[t] = gunPartTransforms[t].transform.localRotation;
            }
        }
        

    }

    public void StopReloadAnimation()
    {

        if(gunPartPositions.Length != 0 && gunPartRotations.Length != 0)
        {
            //transform.localPosition = gunPartPositions[0];
            transform.localRotation = gunPartRotations[0];

            for (int i = 0; i < transform.childCount; i++)
            {
                 transform.GetChild(i).localPosition = gunPartPositions[i + 1];
                 transform.GetChild(i).localRotation = gunPartRotations[i + 1];
            }
        }
        
    }

    public Animator GetAnimator()
    {
        return this.animator;
    }

}


