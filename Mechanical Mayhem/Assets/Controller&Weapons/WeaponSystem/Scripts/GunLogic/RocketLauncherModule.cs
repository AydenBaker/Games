using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LongRangedWeapon))]
public class RocketLauncherModule : MonoBehaviour
{
    
    //variables
    [Header("Rocket Launcher Settings")]
    
    //rocket prefab
    [SerializeField] private GameObject rocketPrefab;
    
    //fake rocket object
    [Tooltip("assign the rocketObject to the rocket launcher's rocket in the scene")]
    [SerializeField] private GameObject rocketObject;

    [SerializeField] private float deactivatedTime = .5f;

    [SerializeField] private CharacterController controller;
    
    //gets input and LongRangedWeaponClass
    private LongRangedWeapon weapon;

    //keeps track of the how much ammo you have
    private bool noAmmo = false;

    //methods
    // Start is called before the first frame update
    void Start()
    {
        //gets class refrences
        weapon = this.GetComponent<LongRangedWeapon>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
        WeaponAmmoManager ammoManager = weapon.GetWeaponAmmoManager();


        //checks if the player has shot
        if (weapon.GetIsShooting())
        {
            if(ammoManager.GetTotalAmmo() >= 0 && ammoManager.GetCurrentAmmo() >= 1)
            {
                StartCoroutine(rocketRegeneration());
            }
                
            LaunchRocket();
        }


        //checks if we can reload the rocket and checks if we are out of ammo
        if (noAmmo && ammoManager.GetTotalAmmo() > 0)
        {
            ammoManager.Reload();
            StartCoroutine(rocketRegeneration());
            weapon.SetTimeSenseLastShot(0);
            noAmmo = false;
        }
        else if(ammoManager.GetCurrentAmmo() <= 0 && ammoManager.GetTotalAmmo() <= 0)
        {
            StopCoroutine(rocketRegeneration());
            rocketObject.SetActive(false);
            noAmmo = true;
        }
        

        //sets our ammoManager settings to what a RPG would be like in real life
        ammoManager.SetCanReload(false);
    }
    
    //launch rocket
    private void LaunchRocket()
    {
        //gets point the rocket should look at
        Vector3 hitPos = GetRayHitPoint().point;
        GameObject activeRocket = Instantiate(rocketPrefab, weapon.GetBulletOrgin().transform.position, weapon.GetBulletOrgin().rotation);
        activeRocket.transform.LookAt(hitPos);
    }
    
    //getting point rocket needs to look at
    private RaycastHit GetRayHitPoint()
    {
        Transform camera = weapon.GetCam();
        
        //create a raycast
        RaycastHit hit;
        if(Physics.Raycast(camera.position, camera.TransformDirection(Vector3.forward), out hit, weapon.GetMaxRayDistance(), weapon.GetLayerToHit()))
        {
            Debug.DrawLine(camera.position, camera.TransformDirection(Vector3.forward * weapon.GetMaxRayDistance()), Color.blue);
        }
        else
        {
            hit.point = camera.position + camera.TransformDirection(Vector3.forward * weapon.GetMaxRayDistance());
        }

        return hit;
    }
    
    //rocket reload IEnemerator
    private IEnumerator rocketRegeneration()
    {
        rocketObject.SetActive(false);

        yield return new WaitForSeconds(deactivatedTime);
 

        rocketObject.SetActive(true);
    }
    
}
