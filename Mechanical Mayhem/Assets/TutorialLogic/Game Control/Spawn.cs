using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{

    [SerializeField] private GameObject spawnGameObject;
    [SerializeField] private bool despawn = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!despawn)
        {
            spawnGameObject.SetActive(true);
        }
        else
        {
            spawnGameObject.SetActive(false);
        }
        
    }
}
