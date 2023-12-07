using UnityEngine;

public class HealthPack : MonoBehaviour
{

    //health gain amount
    [SerializeField] private int healthAmount = 100;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //gets players health component
            Health health = other.GetComponent<Health>();
            health.ChangeHealthValue(healthAmount);

            //destroys the health pack gameObject
            DestroyHealthPack();
        }
    }

    private void DestroyHealthPack()
    {
        //effects & sounds
        
        //destroys gameObject
        Destroy(this.gameObject);
    }
    
}
