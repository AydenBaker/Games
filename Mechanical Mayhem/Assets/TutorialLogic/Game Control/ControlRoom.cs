using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ControlRoom : MonoBehaviour
{
    public UnityEvent doorControl;
    public UnityEvent buttonControl;
    
    public GameObject[] buttons;

    private bool allPressed;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].GetComponent<ButtonScript>().pressed)
            {
                allPressed = true;
            }
            else
            {
                allPressed = false;
                break;
            }
        }

        if (allPressed)
        {
            buttonControl.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        doorControl.Invoke();
    }
}
