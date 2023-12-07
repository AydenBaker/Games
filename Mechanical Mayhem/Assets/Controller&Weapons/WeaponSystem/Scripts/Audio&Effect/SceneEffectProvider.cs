using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneEffectProvider : MonoBehaviour
{
    //holds different effects for different tags
    //different surfaces

    //class refrence
    public static SceneEffectProvider Instance { get; private set; }

    [Space]
    [Header("Surfaces partical effects")]

    //surface tag
    [SerializeField] string defaultTag = "Default";

    //array of surfaces
    [Tooltip("make sure one of your surfaces has a tag with the same name as your defaultTag")]
    [SerializeField] private SurfaceEffect[] surfaces = new SurfaceEffect[1];

    [Space]
    [Header("Surfaces Audio Clips")]
    [Tooltip("just a place holder, this String does nothing.")]
    public string toBeContinued = "WorkInProgress";


    private void Awake()
    {
        //makes sure this is the only instance of the class
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        foreach(SurfaceEffect surface in surfaces)
        {
            WeaponEffect effect = surface.GetWeaponEffect();
            if (effect != null)
            {
                effect.SetTypeOfEffect(WeaponEffect.effectType.Impact);
            }
        }
        
    }

    public WeaponEffect GetSurfaceEffect(RaycastHit hitInfo)
    {

        string tag = hitInfo.collider.gameObject.tag;
        WeaponEffect effect = new WeaponEffect();

        //checks if there is no tag, if thats the case then it will make the tag equal to the default
        //also checks if there is a surface with that tag, if not, tag is equal to defualtTag
        if (tag == null)
        {
            tag = defaultTag;
        }
        else if (tag == "Untagged")
        {
            tag = defaultTag;
        }
        else if (!DoesTagExist(tag))
        {
            tag = defaultTag;
        }


        //iterates through each surface
        for (int i = 0; i < surfaces.Length; i++)
        {

            //checks if tags match
            if (surfaces[i].GetSurfaceTag() == tag)
            {

                //plays effect
                effect = surfaces[i].GetWeaponEffect();

            }
        }

        return effect;

    }

    private bool DoesTagExist(string tag)
    {
        bool doesExist = false;

        for (int i = 0; i < surfaces.Length; i++)
        {
            if (surfaces[i].GetSurfaceTag() == tag)
            {
                doesExist = true;
            }

        }

        return doesExist;
    }


}
