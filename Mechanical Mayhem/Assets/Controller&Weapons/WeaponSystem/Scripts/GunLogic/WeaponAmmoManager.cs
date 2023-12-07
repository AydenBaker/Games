using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAmmoManager : MonoBehaviour
{

    //variables
    
    //max ammo per mag
    [SerializeField] private int maxAmmoPerMag = 45;
    
    //total Ammo
    [SerializeField] private int totalAmmo = 250;
    
    //total ammo you can carry
    [SerializeField] private int maxTotalAmmo = 250;
    
    //current ammo
    [SerializeField] private int currentAmmo = 45;
    private int lastAmmoAmount;

    [SerializeField] private bool autoReload = false;

    private bool canReload = true;

    [SerializeField] private float reloadTime = 1.25f;
    private float time;
    private bool startTimer = false;
    private bool isReloading = false;

    private PlayerInput input;
    private WeaponEffectProvider weaponEffectProvider;


    //methods
    private void Start()
    {
        
        input = PlayerInput.instance;
        weaponEffectProvider = this.GetComponent<WeaponEffectProvider>();
    }

    //checks if the ammo is more than or less the max and min ammo
    private void Update()
    {
        //checks if you are holding your maximum amount of ammo
        if (totalAmmo > maxTotalAmmo)
        {
            totalAmmo = maxTotalAmmo;
        }

        //checks if the player wants to reload
        if (input.reload && totalAmmo > 0 && currentAmmo != maxAmmoPerMag && canReload)
        {
            
            if(reloadTime <= time)
            {
                Reload();
                startTimer = true;
            }
            
        }


        if (startTimer)
        {
            time -= Time.deltaTime;

            if(time <= 0)
            {
                startTimer = false;
            }
        }
        else
        {
            time = reloadTime;
        }

        canReload = true;
    }

    //changing amount of ammo methods
    public void ChangeAmmoAmount(int ammo)
    {
        currentAmmo += ammo;
    }

    public void ChangeTotalAmmo(int ammo)
    {
        totalAmmo += ammo;
    }
    
    public void Reload()
    {

        weaponEffectProvider.StartReloadAnimation();

        
        int neededAmmo = maxAmmoPerMag - currentAmmo;
        int toMuchAmmo = 0;


        totalAmmo -= neededAmmo;
        if (totalAmmo < 0)
        {
            toMuchAmmo = totalAmmo *= -1;
            totalAmmo = 0;
        }

        lastAmmoAmount = currentAmmo;
        currentAmmo += neededAmmo - toMuchAmmo;

    }

    public void SetCurrentAmmoToLast()
    {
        currentAmmo = lastAmmoAmount;
        totalAmmo += Mathf.Abs(currentAmmo - lastAmmoAmount);
    }

    public int GetMaxTotalAmmo()
    {
        return maxTotalAmmo;
    }

    //getter
    public void GetAmmoInformation(out int currentAmmo, out int maxAmmoPerMag, out int totalAmmo)
    {
        currentAmmo = this.currentAmmo;
        maxAmmoPerMag = this.maxAmmoPerMag;
        totalAmmo = this.totalAmmo;
    }
    public void GetAmmoInformation(out int currentAmmo, out int maxAmmoPerMag, out int totalAmmo, out int maxTotalAmmo)
    {
        currentAmmo = this.currentAmmo;
        maxAmmoPerMag = this.maxAmmoPerMag;
        totalAmmo = this.totalAmmo;
        maxTotalAmmo = this.maxTotalAmmo;
    }
    
    public int GetCurrentAmmo()
    {
        return currentAmmo; 
    }

    public bool GetStartTimer()
    {
        return this.startTimer;
    }

    public int GetTotalAmmo()
    {
        return totalAmmo;
    }

    public int GetMaxAmmoPerMag()
    {
        return maxAmmoPerMag;
        
    }

    public bool GetAutoReload() { return this.autoReload; }

    public void SetCanReload(bool canReload)
    {
        this.canReload = canReload;
    }

    public void SetTotalAmmo(int totalAmmo)
    {
        this.totalAmmo = totalAmmo;
    }

    public void SetCurrentAmmo(int currentAmmo)
    {
        this.currentAmmo = currentAmmo;
    }

}
