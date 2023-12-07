using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GunUIManager : MonoBehaviour
{

    //visible in inspector variables
    [SerializeField] private Text AmmoText;

    //private variables
    private int currentAmmo = 0;
    private int totalAmmo = 0;
    private int ammoPerMag = 0;

    //class refrences
    private LongRangedWeapon activeGun;
    private GunSwitching gunRefrence;



    //methods
    private void Start()
    {
        //gets the gunSwitching class on this same gameobject
        gunRefrence = GetComponent<GunSwitching>();
    }

    // Update is called once per frame
    void Update()
    {
        
        //checks if we can get the current gun LongRangedWeapon class
        if (gunRefrence.GetCurrentGun() != null)
        { 
            //gets the current guns longRagedWeapon class
            if (gunRefrence.GetCurrentGun().GetComponent<LongRangedWeapon>() != null)
            {
                activeGun = gunRefrence.GetCurrentGun().GetComponent<LongRangedWeapon>();

                if (activeGun.GetComponent<WeaponAmmoManager>() != null)
                {
                    activeGun.GetComponent<WeaponAmmoManager>().GetAmmoInformation(out currentAmmo, out ammoPerMag, out totalAmmo);
                }
                else
                {
                    Debug.LogWarning("Add WeaponAmmoManger to " + activeGun.name);
                }
                

            }
            
        }
           
        UpdateAmmoUI();

    }

    private void UpdateAmmoUI()
    {

        AmmoText.text = currentAmmo.ToString() + " / " + ammoPerMag.ToString() + " \n      " + totalAmmo.ToString();

    }

}
