using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(WeaponEffectProvider))]
[RequireComponent(typeof(WeaponAmmoManager))]
public class LongRangedWeapon : MonoBehaviour
{

    #region Varibles
    //refrences
    private Transform cam;
    private PlayerInput input;

    //inputs
    private bool shootInput;

    //gun type
    private enum TypesOfGuns
    {
        Pistol,
        SemiRifle,
        AutoRifle,
        HeavyMachineGun,
        RocketLauncher,
        MiniGun,
        ShotgunPump,
        ShotgunTactical
    };

   
    [Space]
    [Header("Gun Settings")]

    //type of gun
    [SerializeField] private TypesOfGuns gunType;

    //gun information
    [Tooltip("recomended 0.1 - 2.0")]
    [SerializeField] float fireRate = .2f;
    private float timeSenseLastShot = 0;

    //used so the player does not shot faster than the fire rate
    private float MaxTimeBetweenShots;
    private float timeBetweenShots;

    [SerializeField] float maxShootingDistance = 200f;

    [SerializeField] LayerMask LayersToHit;

    [Tooltip("if actived, you cant hold down the shoot button to shoot")]
    [SerializeField] bool isSemiAuto = false;

    private bool hasAudioStarted = false;

    [Space]
    [Header("Bullet Settings")]

    [Tooltip("How much damage each shot does to an enemy")]
    [SerializeField] float bulletDamage = 25f;
    //the object we are going to instatiate
    [SerializeField] Transform bulletOrgin;
    [Tooltip("Optional, you do not need to put anything here! it acts like a bullet, but does nothing")]
    [SerializeField] GameObject fakeBullet;

    //effects
    private WeaponEffectProvider weaponEffectProvider;

    [Space]


    //if the player hasn't shot yet
    private bool firstShot = true;

    private bool equipt = false;

    //class refrences and others
    private WeaponAmmoManager ammoManager;
    
    private bool justEnabled = true;
    private bool noOtherModules = true;
    #endregion

    #region Methods

    //expermental
    private bool isShooting = false;



    

    void Start()
    {
        // gets the input class so we can get input from player
        input = PlayerInput.instance;

        //cam = GetComponentInParent<GunSwitching>().GetCurrentGun().transform;
        

        //ammo class
        ammoManager = this.GetComponent<WeaponAmmoManager>();

        //effect class
        weaponEffectProvider = this.GetComponent<WeaponEffectProvider>();

        //checks if we want to use the regular shooting system or one like a shotgun
        noOtherModules = CheckIfNoModules();

    }

    private void OnEnable()
    {
        justEnabled = true;
    }
    
    void Update()
    {

        //stops anything from happening if the gun is equipt
        if (!equipt)
        {
            weaponEffectProvider.GetAnimator().enabled = false;

            if (ammoManager != null)
            {
                if (ammoManager.GetStartTimer())
                {
                    ammoManager.SetCurrentAmmoToLast();
                }

                ammoManager.SetCanReload(false);

            }
            shootInput = false;

            return;
        }

        MaxTimeBetweenShots = fireRate;

        //sets shoot input to a new bool
        if (isSemiAuto)
        {
            shootInput = input.shootSemi;
        }
        else
        {
            shootInput = input.shoot;
        }

        isShooting = Shooting();

    }

    //methods

    public void CancelAnimation()
    {
        
        if (weaponEffectProvider != null)
        {
             weaponEffectProvider.StopReloadAnimation();
        }

        if(ammoManager != null)
        {
            if (ammoManager.GetStartTimer())
            {
                ammoManager.SetCurrentAmmoToLast();
            }

            ammoManager.SetCanReload(false);

        }

    }

    private bool CheckIfNoModules()
    {
        //checks for both shotgun and rocket launcher modules
        bool noOtherModules = !(this.GetComponent<ShotGunModule>() != null);

        noOtherModules = !(this.GetComponent<RocketLauncherModule>() != null);

        return noOtherModules;
    }

    private Vector3 GetBulletDirection()
    {

        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.transform.TransformDirection(Vector3.forward), out hit, maxShootingDistance, LayersToHit))
        {
            Debug.DrawLine(cam.position, cam.TransformDirection(Vector3.forward * maxShootingDistance), Color.blue);
            return hit.point;
        }
        else
        {
            Debug.DrawLine(cam.position, cam.TransformDirection(Vector3.forward * maxShootingDistance), Color.blue);
            return cam.position + cam.TransformDirection(Vector3.forward * maxShootingDistance);
        }

    }

    //checks if the player is currently shooting
    private bool Shooting()
    {

        bool _isShooting = false;

        
        //check if there is ammo left
        if (ammoManager.GetCurrentAmmo() <= ammoManager.GetMaxAmmoPerMag() && ammoManager.GetCurrentAmmo() > 0 && !ammoManager.GetStartTimer())
        {
            

            //does the player want to shoot 
            if (shootInput && timeBetweenShots >= MaxTimeBetweenShots|| justEnabled && shootInput)
            {

               
                //starts delay timer
                timeSenseLastShot += Time.deltaTime;
                if (firstShot)
                {
                    weaponEffectProvider.TurnLightOn();
                    timeSenseLastShot = fireRate;
                    firstShot = false;
                }
                //shoots if the timer is greater than the timer
                if (timeSenseLastShot >= fireRate || isSemiAuto)
                {
                    weaponEffectProvider.TurnLightOn();


                    _isShooting = true;
                    if (noOtherModules)
                    {
                        RayCastShoot();
                    }

                    

                    if (!isSemiAuto && !hasAudioStarted)
                    {
                        
                        //calls shootEffects to get muzzleflash, bullet impact, and sounds
                        ShootingEffect();
                        hasAudioStarted = true;
                    }
                    else
                    {
                        ShootingEffect();
                    }
                   
                    ammoManager.ChangeAmmoAmount(-1);

                    if (ammoManager.GetAutoReload() && ammoManager.GetCurrentAmmo() < 1 && ammoManager.GetTotalAmmo() > 0)
                    {
                        ammoManager.Reload();
                    }

                    //resets timer
                    timeSenseLastShot = 0;
                    timeBetweenShots = 0;
                }

                justEnabled = false;
            }
            else
            {
                timeSenseLastShot = 0;
                timeBetweenShots += Time.deltaTime;
                firstShot = true;
                weaponEffectProvider.TurnOffLight();

            }

           
            
        }

        if (!shootInput && !isSemiAuto || ammoManager.GetCurrentAmmo() <= 0 || ammoManager.GetStartTimer())
        {
            hasAudioStarted = false;
            if (!isSemiAuto)
            {
                StopShootingEffects();
            }
            
            weaponEffectProvider.TurnOffLight();
        }

        return _isShooting;

    }

    //this is the default shoot mode. others include shotGun and RocketLauncher
    private void RayCastShoot()
    {
        //shoots

       //spawns fake bullet if the gun uses one
        if (fakeBullet != null)
        {
            GameObject newbullet = Instantiate(fakeBullet, bulletOrgin.position, bulletOrgin.rotation);
            newbullet.transform.LookAt(GetBulletDirection());
        }

        //raycast bullet damage
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.transform.TransformDirection(Vector3.forward), out hit, maxShootingDistance, LayersToHit))
        {
            //creates the bullet impact effect at hit
            InstantiateBulletImpactAtPoint(hit);

            //deals damage to the enemy
            Debug.DrawLine(cam.position, cam.TransformDirection(Vector3.forward * maxShootingDistance), Color.red);
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                GameObject objectHit = hit.collider.gameObject;
                //do damage
                objectHit.GetComponent<Health>().ChangeHealthValue(-bulletDamage);

            }
            else if (hit.collider.gameObject.CompareTag("Target"))
            {
                DummyMain dummy = hit.collider.gameObject.GetComponentInParent<DummyMain>();
                dummy.Hit();
            }
            else if (hit.collider.gameObject.CompareTag("Glass"))
            {
                Debug.Log("glass");
                GlassBehavior glass = hit.collider.gameObject.GetComponent<GlassBehavior>();
                glass.GlassHit();
            }
        }

    }
    


    //effects
    public void InstantiateBulletImpactAtPoint(RaycastHit hit)
    {
        //checks if we actually hit something
        if (hit.collider != null)
        {
            //instantiates surface impact effect where the racast hit the mesh
            GameObject _impactEffect = Instantiate(weaponEffectProvider.GetImpactEffectForSurface(WeaponEffect.effectType.Impact, hit), hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(_impactEffect, 2f);
        }

    }

    private void ShootingEffect()
    {
        weaponEffectProvider.PlayEffectType(WeaponEffect.effectType.MuzzleFlash);

        //muzzle audio
        weaponEffectProvider.PlayAudio(WeaponAudio.AudioType.firingGun);

    }

    private void StopShootingEffects()
    {
        weaponEffectProvider.StopEffectType(WeaponEffect.effectType.MuzzleFlash);

        //muzzle audio
        weaponEffectProvider.StopAudio(WeaponAudio.AudioType.firingGun);
    }

    //setters
    public void SetCam(GameObject cam) { this.cam = cam.transform; }

    public void SetTimeSenseLastShot(float timeSenseLastShot) { this.timeSenseLastShot = timeSenseLastShot; }

    public void SetEquipt(bool value)
    {
        equipt = value;
    }
    
    //getters
    public bool GetIsShooting() { return this.isShooting; }

    public Transform GetCam() { return this.cam; }

    public float GetMaxRayDistance() { return this.maxShootingDistance; }

    public LayerMask GetLayerToHit() { return this.LayersToHit; }

    public float GetRayCastBulletDamage() { return this.bulletDamage; }

    public float GetFireRate()
    {
        return this.fireRate; 
    }

    public Enum GetGunType()
    {
        return gunType;
    }
    
    public int GetGunTypeInt()
    {
        return (int)gunType;
    }
    
    public bool GetEquipt()
    {
        return equipt;
    }

    public Transform GetBulletOrgin()
    {
        return this.bulletOrgin; 
    }

    public WeaponAmmoManager GetWeaponAmmoManager()
    {
        return this.ammoManager;
    }

    public WeaponEffectProvider GetWeaponEffectProvider()
    {
        return this.weaponEffectProvider;
    }

    public bool GetJustEnabled()
    {
        return this.justEnabled;
    }
    #endregion

}
