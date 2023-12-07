using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchGrenade : MonoBehaviour
{

    [SerializeField] ThrowGrenade[] gernades;
    private int gernadeIndex = 0;

    private ThrowGrenade currentGernade;

    private PlayerInput input;
    private bool gernadeSwitching;

    private void Start()
    {
        input = PlayerInput.instance;

        currentGernade = gernades[0];
        ActivateCurrentGernade();
        

    }

    private void Update()
    {
        gernadeSwitching = input.switchGernade;

        //if player wants to switch gernades switch
        if (gernadeSwitching)
        {
            SwitchGernade();
        }
    }

    private void SwitchGernade()
    {

        if(gernadeSwitching)
        {
            gernadeIndex++;
        }
        else if(gernadeSwitching && gernadeIndex > 0)
        {
            gernadeIndex--;
        }

        //resets the index if it is over the amount of gernades
        if(gernadeIndex >= gernades.Length)
        {
            gernadeIndex = 0;
        }

        for(int i = 0; i < gernades.Length; i++)
        {
            if(i == gernadeIndex)
            {
                currentGernade = gernades[i];
                ActivateCurrentGernade();
            }
        }

    }

    private void ActivateCurrentGernade()
    {

        

        for(int i = 0; i < gernades.Length; i++)
        {
            if (gernades[i] != currentGernade)
            {
                //deactivate
                gernades[i].enabled = false;
            }
            else
            {
                //activate
                gernades[i].enabled = true;
            }
        }

    }

}
