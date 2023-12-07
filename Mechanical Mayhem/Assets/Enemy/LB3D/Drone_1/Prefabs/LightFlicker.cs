using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public Light light1;
    public Light light2;

    // Update is called once per frame
    void Update()
    {
        if ( Random.value > 2.9 ) //a random chance
        {
            if ( light1.enabled == true ) //if the light is on...
            {
                light1.enabled = false; //turn it off
                light2.enabled = false;
            }
            else
            {
                light1.enabled = true; //turn it on
                light2.enabled = true;
            }
        }
    }
}
