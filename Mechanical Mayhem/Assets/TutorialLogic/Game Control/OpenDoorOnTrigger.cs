using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorOnTrigger : MonoBehaviour
{

    [SerializeField] private SlideDoorMovement door;
    [SerializeField] private bool doorStartClose = true;
    

    private bool hasEnteredTrigger = false;

    // Start is called before the first frame update
    void Start()
    {
        if(door == null)
        {
            door = this.GetComponent<SlideDoorMovement>();
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //open the door
            if (!hasEnteredTrigger)
            {
                DoorInteractions();
            }

            hasEnteredTrigger = true;
        }

        
    }

    private void DoorInteractions()
    {
        if (doorStartClose)
        {
            door.SetHasAudioPlayed(false);
            door.StartOpen();
        }
        else
        {
            door.SetHasAudioPlayed(false);
            door.StartClose();
        }
    }
}
