using UnityEngine;

public class AmmoPack : MonoBehaviour
{
    //added ammo Amount
    [SerializeField] private bool autoCalculateAmmo = false;
    
    [SerializeField] private int addedAmmo = 15;

    [Tooltip("make sure your value is a decimal")]
    [SerializeField] private float ammoDivider = .65f;
    
    private WeaponAmmoManager currentAmmoManager;

    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //gets our active gun reference & adds ammo amount
            GunSwitching guns = other.GetComponentInChildren<GunSwitching>();
            currentAmmoManager = guns.GetCurrentGun().GetComponent<WeaponAmmoManager>();
            
            if (autoCalculateAmmo)
            {
                addedAmmo = GetAmmoToAdd(currentAmmoManager.GetMaxTotalAmmo());
            }
            
            //adds total ammo
            currentAmmoManager.ChangeTotalAmmo(addedAmmo);

            DestroyObject();
        }
    }

    public void DestroyObject()
    {
        //effects
        
        //destroying game object
        Destroy(this.gameObject);
    }
    
    private int GetAmmoToAdd(int maxTotalAmmo)
    {
        return Mathf.RoundToInt((float)maxTotalAmmo * ammoDivider);
    }
    
    
}
