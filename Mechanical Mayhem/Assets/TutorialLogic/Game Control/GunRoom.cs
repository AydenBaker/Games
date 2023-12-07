using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GunRoom : MonoBehaviour
{
    public UnityEvent roomStart;
    public UnityEvent gunControl;

    public GameObject[] targets;
    public GameObject[] guns;
    public Transform spawner;

    private int round = 0;


    private void startSpawn()
    {
        if(guns.Length > 0)
        Instantiate(guns[0], spawner.position, Quaternion.identity);
    }

    private void Update()
    {
        for(int i = 0; i < targets.Length; i++)
        {
            DummyMain target = targets[i].GetComponent<DummyMain>();
            //if one of the dummys is hit it will open the door
            if (target.GetIsHit())
            {
                gunControl.Invoke();
            }

        }
    }

    private void next()
    {
        if (round == 2)
        {
            round++;
            
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        roomStart.Invoke();
        if (round == 0)
        {
            startSpawn();
        }
    }
}
