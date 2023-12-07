using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SurfaceEffect
{

    //private variables
    [Tooltip("Tag must be present in your project")]
    [SerializeField] private string surfaceTag = "Dirt";

    [SerializeField] private WeaponEffect effect;

    /*
    [Tooltip("set true if you want to activate an Audio clip")]
    [SerializeField] private bool hasAudio = true;
    [SerializeField] private string audioName = "DirtImpactSound";
    */

    //methods
  


    //getters 
    

    public string GetSurfaceTag()
    {
        return surfaceTag;
    }

    public WeaponEffect GetWeaponEffect()
    {
        return effect;
    }

}
