using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public bool pressed;
    public bool lockButtonAfter;

    public Material[] materials;

    public GameObject buttonObject;
    public Light light;

    private bool buttonLock = false;

    // Start is called before the first frame update
    void Start()
    {
        
        if (!pressed)
        {
            buttonObject.GetComponent<Renderer>().material = materials[0];
        }
        else
        {
            buttonObject.GetComponent<Renderer>().material = materials[1];
        }
    }

    // Update is called once per frame
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E) && !buttonLock)
        {
            if (!pressed)
            {
                pressed = true;
                buttonObject.GetComponent<Renderer>().material = materials[1];
                light.enabled = false;
            }
            else
            {
                pressed = false;
                buttonObject.GetComponent<Renderer>().material = materials[0];
            }

            if (lockButtonAfter)
            {
                buttonLock = true;
            }
        }
    }
}
