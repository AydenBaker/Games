using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassBehavior : MonoBehaviour
{
    public string glassType;
    public GameObject player;

    private ParticleSystem glassShatter;
    private GameObject glass;
    private CCPlayerMovement dash;
    
    // Start is called before the first frame update
    void Start()
    {
        glassShatter = GetComponentInChildren<ParticleSystem>();
        glass = this.gameObject;
        dash = player.GetComponent<CCPlayerMovement>();
    }

    // Update is called once per frame

    public void GlassHit()
    {
        if (glassType == "regular")
        {
            Debug.Log("Break");
            Shatter();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (dash.isDashing && glassType != "noBreak")
        {
            Shatter();
        }
    }

    void Shatter()
    {
        glassShatter.Play();
        Destroy(glass.GetComponent<MeshRenderer>());
        Destroy(glass.GetComponent<BoxCollider>());
        
        Invoke("Remove", 2);
    }

    void Remove()
    {
        Destroy(glass);
    }
}
