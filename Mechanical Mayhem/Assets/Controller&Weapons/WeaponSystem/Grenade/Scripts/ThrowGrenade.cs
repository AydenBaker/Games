using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowGrenade : MonoBehaviour
{

    //variables
    [SerializeField] GameObject Gernade;
    [SerializeField] Transform GernadeSpawn;

    [SerializeField] int maxAmountOfGernades = 4;
    [SerializeField] int amountOfGernades = 4; 

    private PlayerInput input;


    //methods
    private void Start()
    {
        input = PlayerInput.instance;
    }

    void Update()
    {
        bool throwGernade = input.throwGernade;

        if (throwGernade && amountOfGernades > 0)
        {
            SpawnGernade();
        }

    }

    private void SpawnGernade() 
    {
        //spawns gernade
        Instantiate(Gernade, GernadeSpawn.position, GernadeSpawn.rotation);
        amountOfGernades--;
    }

}
