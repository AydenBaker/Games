using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    //get the game object & rigidbody
    private Rigidbody rb;

    //timer before exploding
    [SerializeField] float timeAlive = 4f;
    private float timer;

    [SerializeField] LayerMask Includedlayer;

    //Gernade Information
    [SerializeField] float speedThrown = 5f;

    [SerializeField] float damage = 100f;
    [SerializeField] float forceAppliedToEnemys = 30f; //will be used in weaponSystemV2

    [SerializeField] float blastSize = 5;

    [SerializeField] bool explodeOnCollision = false;

    //used to call explosion effect
    [SerializeField] GameObject explosiveEffect;
    


    //gets reguired refrences
    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    private void Start()
    {
        rb.AddForce(transform.TransformDirection(Vector3.forward * speedThrown * Time.fixedDeltaTime), ForceMode.Impulse);
    }

    private void Update()
    {

        StartTimer();

        //check if timer has ended
        if (timer >= timeAlive)
        {

            //activates explosion & does damage
            ApplyDamageToTargets(ExplodeSphereCast());
            GernadeEffects();
            DestroyGernade();
        }

    }

    //counter
    private void StartTimer()
    {
        timer += Time.deltaTime;
    }

    //sphere cast
    private RaycastHit[] ExplodeSphereCast()
    {
        RaycastHit[] hits = Physics.SphereCastAll(this.transform.position, blastSize, Vector3.one, blastSize);
        return hits;
    }

    //apply damage
    private void ApplyDamageToTargets(RaycastHit[] hits)
    {
        //checks if it has hit any enemys 
        for(int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.CompareTag("Enemy"))
            {
                GameObject enemy = hits[i].collider.gameObject;
                enemy.GetComponent<Health>().ChangeHealthValue(-damage);
            }
            else if (hits[i].collider.CompareTag("Target"))
            {
                DummyMain dummy = hits[i].collider.gameObject.GetComponentInParent<DummyMain>();
                dummy.Hit();
            }
        }

    }

    
    private void OnCollisionEnter(Collision collision)
    {

        if (((1 << collision.gameObject.layer) & Includedlayer) != 0)
        {
            if (explodeOnCollision)
            {
                timer = timeAlive;
            }
            else
            {
                timer = timeAlive * .8f;
            }
        }
    }

    private void DestroyGernade()
    {
        Destroy(this.gameObject);
    }

    //effects
    private void GernadeEffects()
    {

        //call effects
        Instantiate(explosiveEffect, this.transform.position, Quaternion.LookRotation(Vector3.up));

    }

    //debuging
    public void OnDrawGizmosSelected()
    {
        //creates a visible representation of our blast
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, blastSize);
    }


}
