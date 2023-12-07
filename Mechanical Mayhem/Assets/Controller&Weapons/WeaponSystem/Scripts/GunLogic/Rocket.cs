using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class Rocket : MonoBehaviour
{
    [Range(10, 70)]
    [SerializeField] private float speed = 50;
    [SerializeField] private float rocketDamage = 200f;

    [SerializeField] private float timeAlive = 6f;
    private float timer;

    [Range(0.1f, 1)]
    [SerializeField] private float rocketDetectionSize = .4f;

    [Range(.1f, 1f)]
    [SerializeField] private float SphereCastLength = .7f;
    [SerializeField] private float explosionSize = 5f;
    [SerializeField] private LayerMask layer;

    [SerializeField] private GameObject explosionEffect;

    private bool hitSomething = false;
    private RaycastHit hit;
    private Rigidbody rb;

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //checks if the rocket has collided with an object
        if (hitSomething)
        {
            Explode();
        }
        
        //checks if the rocket has been alive for to long 
        timer += Time.deltaTime;
        if (timer >= timeAlive)
        {
            Explode();
        }

    }

    private void FixedUpdate()
    {
        //moves the rocket in a forward direction
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
    }


    //exploding logic
    private void Explode()
    {
        
        //create a sphercastall check for enemys
        RaycastHit[] hits = Physics.SphereCastAll(this.transform.position, explosionSize, Vector3.up, 10, layer);


        //do damage to current enemys
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.CompareTag("Enemy"))
            {
                GameObject activeEnemy = hits[i].collider.gameObject;
                activeEnemy.GetComponent<Health>().ChangeHealthValue(-rocketDamage);
            }
            else if (hits[i].collider.CompareTag("Target"))
            {
                DummyMain dummy = hits[i].collider.gameObject.GetComponentInParent<DummyMain>();
                dummy.Hit();
            }
        }

        //instatiate effect
        RocketEffects();

        //despawn rocket
        Destroy(this.gameObject);

    }

    //effects
    private void RocketEffects()
    {

        //check if hit is not equal to zero
        if(hit.point == Vector3.zero)
        {
            Instantiate(explosionEffect, transform.position, transform.rotation);
        }
        else
        {
            Instantiate(explosionEffect, hit.point, Quaternion.LookRotation(hit.normal));
        }

    }


    private void OnCollisionEnter(Collision collision)
    {
        //checks if the rocket has hit the correct layer
        if ((layer & (1 << collision.transform.gameObject.layer)) != 0)
        {
            hitSomething = true;
        }
        
       
    }

    public void SetRocketVelocity(Vector3 velocity)
    {
        rb.velocity = velocity;
    }
    

    //debuger
    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, explosionSize);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0,0, SphereCastLength), rocketDetectionSize);
            
    }

}
