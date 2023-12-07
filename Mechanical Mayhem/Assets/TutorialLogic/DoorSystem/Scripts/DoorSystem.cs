using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DoorSystem : MonoBehaviour
{
    
    public Rigidbody rb;
    public Animator animator;
    public Transform player, cam;
    public CCPlayerMovement forDash;
    public ParticleSystem[] breakParticles;
    
    public int maxDistance;
    public int forwardForce;
    
    private bool isBroken = false;
    private bool isOpen = false;
    private bool pleaseWait = false;

    // Update is called once per frame
    private void Update()
    {
        // Gets distance from player
        Vector3 distanceToPlayer = player.position - transform.position;
        
        // Checks if in range, animation is not playing, isn't broken, and if 'F' is being pressed
        if (distanceToPlayer.magnitude <= maxDistance && Input.GetKeyDown(KeyCode.F) && !pleaseWait && !isBroken)
        {
            OpenCLose();
        }
    }

    private void OpenCLose()
    {
        // Opens or Closes the Door
        if (!isOpen)
        {
            StartCoroutine(Open());
        }
        else
        {
            StartCoroutine(Close());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Checks if player is Dashing and isn't broken
        if (forDash.isDashing && !isBroken && other.CompareTag("Player"))
        {
            Break();
        }
    }

    IEnumerator Open()
    {
        pleaseWait = true;
        
        // Plays Animation
        animator.Play("DoorOpen", 0, 0.0f);
        yield return new WaitForSeconds(1f);
        
        isOpen = true;
        pleaseWait = false;
    }

    IEnumerator Close()
    {
        pleaseWait = true;
        
        // Plays Animation
        animator.Play("DoorClose", 0, 0.0f);
        yield return new WaitForSeconds(1f);
        
        isOpen = false;
        pleaseWait = false;
    }

    private void Break()
    {
        //Prevents Door from being interacted with
        isBroken = true;

        animator.enabled = false;
        for (int i = 0; i < breakParticles.Length; i++)
        {
            breakParticles[0].Play();
            breakParticles[1].Play();
            breakParticles[2].Play();
        }

        //Throw Door Back
        transform.SetParent(null);
        rb.isKinematic = false;
        rb.AddForce(cam.forward * forwardForce, ForceMode.Impulse);
    }
}
