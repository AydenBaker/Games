using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{

    //throw force
    [Header("Pick up Information")]
    [SerializeField] private float throwForce = 50f;

    [SerializeField] private Transform gunPos;
    [SerializeField] private GameObject gunHolder;
    [SerializeField] private int weaponLayersIndex;
    [SerializeField] private int weaponPickUpLayer;
    
    private GameObject gunBeingSwitched;
    private GameObject ObjectInCollider;
    
    private bool inRange = false;


    [Header("Weapon SphereCast Settings")] 
    [SerializeField] private float rayLength = 1.3f;
    [SerializeField] private float sphereRadius = .5f;
    
    private RaycastHit weaponHitInfo;
    

    //rigidbody & input class
    private PlayerInput input;
    private Rigidbody gunRB;
    [SerializeField] private GunSwitching gunSwitchingScript;

    // Start is called before the first frame update
    void Start()
    {
        //set class references
        input = PlayerInput.instance;
    }

    private bool AlreadyHasWeapon()
    {
        //get the guns gun type
        LongRangedWeapon weapon = ObjectInCollider.GetComponent<LongRangedWeapon>();
        
        
        //check if the player already has that gun type
        for (int i = 0; i < gunSwitchingScript.guns.Count; i++)
        {
            if (gunSwitchingScript.guns[i].GetComponent<LongRangedWeapon>().GetGunTypeInt() == weapon.GetGunTypeInt())
            {
                return true;
            }
        }
        
        return false;
    }

    private GameObject GetSameGunInGunList(GameObject pickedUpGun)
    {
        
        //get the guns gun type
        LongRangedWeapon weapon = pickedUpGun.GetComponent<LongRangedWeapon>();
        
        
        //check if the player already has that gun type
        for (int i = 0; i < gunSwitchingScript.guns.Count; i++)
        {
            if (gunSwitchingScript.guns[i].GetComponent<LongRangedWeapon>().GetGunTypeInt() == weapon.GetGunTypeInt())
            {
                return gunSwitchingScript.guns[i];
            }
        }
        
        return null;
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            if (input.PickUpGun)
            {
                ObjectInCollider = other.transform.gameObject;

                //if already has gun type then drop the current gun of that type
                if (!AlreadyHasWeapon())
                {
                    EquipWeapon();
                }
                else
                {

                    gunBeingSwitched = GetSameGunInGunList(ObjectInCollider);

                    //equipts the new gun
                    EquipWeapon();

                    //drop the equipt gun and remove it from gun list
                    DropGun(gunBeingSwitched);


                }
            }


        }
    }


    private void EquipWeapon()
    {
        //get guns rigidbody, set its parent to gun holder and add it to the gun array in the gun switching script if thw weapon is not already in the array
        gunRB = ObjectInCollider.GetComponent<Rigidbody>();
        gunRB.isKinematic = true;
        gunRB.useGravity = false;

        ObjectInCollider.transform.position = gunPos.position;
        ObjectInCollider.transform.rotation = gunPos.rotation;

        ObjectInCollider.transform.parent = gunHolder.transform;

        ObjectInCollider.GetComponent<LongRangedWeapon>().GetWeaponEffectProvider().SetGunOrigins();

        gunRB.gameObject.layer = weaponLayersIndex;

        for (int i = 0; i < gunRB.gameObject.transform.childCount; i++)
        {
            gunRB.gameObject.transform.GetChild(i).gameObject.layer = weaponLayersIndex;
        }
        
        if (AlreadyHasWeapon())
        {
            
            gunSwitchingScript.AddGunAtPointInList(gunRB.gameObject, 0);
        }
        else
        {
            gunSwitchingScript.AddGunToList(gunRB.gameObject);
        }
        
        
    }
    
    //drop gun
    private void DropGun(GameObject gun)
    {
        gun.gameObject.SetActive(true);
        
        //adds gravity back to the rigidbody
        gun.GetComponent<Rigidbody>().isKinematic = false;
        gun.GetComponent<Rigidbody>().useGravity = true;

        //sets parent to none and sets layer to pick up
        gun.transform.parent = null;
        gun.layer = weaponPickUpLayer;

        for (int i = 0; i < gun.transform.childCount; i++)
        {
            gun.transform.GetChild(i).gameObject.layer = weaponPickUpLayer;
        }
        
        gunSwitchingScript.RemoveGunFromList(gun);

        //add force
        gun.GetComponent<Rigidbody>().AddForce(transform.TransformDirection(Vector3.forward) * throwForce * Time.fixedDeltaTime, ForceMode.Impulse);

        ObjectInCollider = null;
        gunBeingSwitched = null;
        
    }
    
    //if guns are the same type switch them
    
    //debugging 
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y - rayLength, transform.position.z), sphereRadius);
    }
}
