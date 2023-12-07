using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSwitching : MonoBehaviour
{

    //stores all of our guns
    public List<GameObject> guns;

    [SerializeField] private float timeBetweenSwitching = .5f;
    private float time;
    
    //input 
    private float switchGunInput = 0;

    //gun index
    private int gunIndex = 0;
    private bool isSwitching = true;

    //active gun
    private GameObject currentGun;

    //cam Refrence - used for guns scripts to get cam refrence
    private GameObject cam;

    //refrences
    PlayerInput input;


    private void Awake()
    {
        cam = this.GetComponentInParent<Camera>().gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        input = PlayerInput.instance;

        //sets the active gun to gun 0 if active gun == false
        if (currentGun == null)
        {
            currentGun = guns[0];
        }

        ActivateActiveGun();
    }

    // Update is called once per frame
    void Update()
    {
        switchGunInput = input.switchingGuns; //Debug.Log(switchGunInput);// -.1 to .1


        SwitchGun();
    }

    //gets current gun
    public GameObject GetCurrentGun()
    {
        return currentGun;
    }

    //switch gun
    private void SwitchGun()
    {
        //gun index range goes from 0 - guns.Count

        //checks if we need to check which gun is active
        if (isSwitching)
        {
            time += Time.deltaTime;
            //checks if the player wants the next gun or the last
            if (time > timeBetweenSwitching)
            {
                //gets active gun
                for (int i = 0; i < guns.Count; i++)
                {
                    if (i == gunIndex)
                    {
                        //set active gun
                        currentGun = guns[i];


                        ActivateActiveGun();

                        isSwitching = false;
                        time = 0;
                    }
                }
            }
        }

        
            if (switchGunInput > 0 && gunIndex < guns.Count - 1 && !isSwitching)
            {
                //next gun
                gunIndex++;
                isSwitching = true;
            }
            else if (switchGunInput < 0 && gunIndex > 0 && !isSwitching)
            {
                //last gun
                gunIndex--;
                isSwitching = true;
            }


        


    }

    public void ActivateActiveGun()
    {
        //activates current guns. then deactivates all other guns
        for(int i = 0; i < guns.Count; i++)
        {
            if (guns[i] != currentGun)
            {
                guns[i].SetActive(false);
            }
            else
            {
                LongRangedWeapon weapon = guns[i].GetComponent<LongRangedWeapon>();
                weapon.SetCam(cam);
                weapon.SetEquipt(true);
                weapon.CancelAnimation();
                guns[i].SetActive(true);
            }
        }
    }//make this timed so the player cant switch really quickly between weapons


    public void RemoveGunFromList(GameObject gun)
    {
        //iterates through each gun the player is holding and removes the parameter gun from the list
        for (int i = 0; i < guns.Count; i++)
        {
            if (guns[i] == gun)
            {
                LongRangedWeapon weapon = guns[i].GetComponent<LongRangedWeapon>();

                //disable the gun
                weapon.SetEquipt(false);
                guns.Remove(guns[i]);
                
            }
        }
    }


    public void AddGunToList(GameObject weapon)
    {
        guns.Add(weapon);
        gunIndex = guns.Count - 1;
        currentGun = weapon;
        ActivateActiveGun();
    }

    public void AddGunAtPointInList(GameObject weapon, int index)
    {
        
        for (int i = 0; i < guns.Count; i++)
        {
            if (i == index)
            {
                guns.Insert(i ,weapon);
            }
        }
        
        currentGun = weapon;
        ActivateActiveGun();
    }
    
}
