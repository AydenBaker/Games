using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponEffect
{
    //variables
    public enum effectType
    {
        none,
        MuzzleFlash,
        Impact,
        shellCasing
    };

    //checks if its an impact, muzzleFlash, or other
    [SerializeField] private effectType typeOfEffect;

    //effect info
    [SerializeField] ParticleSystem particleSystem;
    [SerializeField] bool isLooping = false;

    //particle system
    public void SetParticleSystem(ParticleSystem particle)
    {
        particleSystem = particle;
    }

    public ParticleSystem GetParticleSystem()
    {
        return particleSystem;
    }

    public effectType GetEffectType() { return this.typeOfEffect; }

    //is looping
    public bool GetisLooping()
    {
        return isLooping;
    }

    //gameObject
    public GameObject GetGameObject()
    {
        return particleSystem.transform.gameObject;
    }

    public void SetTypeOfEffect(effectType typeOfEffect)
    {
        this.typeOfEffect = typeOfEffect;
    }
}
