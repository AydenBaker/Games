using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public class EnemyNavMesh : MonoBehaviour
{
    // Nav Mesh Variables
    [SerializeField] private Transform movePositionTransform;
    private NavMeshAgent navMeshAgent;
    private GameObject player;
    public int followRange = 10;
    
    // Animation Variables
    private Animator animator;
    private bool isWalking = true;
    private bool isResting = false;
    private bool spawnTaunt = true;
    
    // Collision Variables
    public float yBoundBox = 1.7f;
    public float xzBoundBox = 1.4f;
    public bool autoDie = false;

    public float damage = 15f;
    
    // Rendering Variables
    public float deathDuration = 1.0f;
    private Vector3 temp;

    // Activate enemy lights
    public GameObject replace;
    public float lightUpDistance = 7f;
    
    // Sound Effects
    public AudioSource source;
    public AudioClip scream;
    public AudioClip skitter;
    public static bool soundPlay = false;



    private void Awake()
     {
         // Load the battlefield
         navMeshAgent = GetComponent<NavMeshAgent>();
         // Find the player
         player = GameObject.FindWithTag("Player");
         // Get player's transform object
         movePositionTransform = player.transform;
         // Get enemy's animator
         animator = this.GetComponent<Animator>();
         // Set a random speed for variance
         this.navMeshAgent.speed = Random.Range(1f, 4f);
         // Obtain Source
         source = GetComponent<AudioSource>();


     }
    
    private void Update()
    {
        temp = this.transform.localScale;

        if (!source.isPlaying)
        {
            soundPlay = false;
        }
        
        // Find the distance between the player and the enemy
        float dist = Vector3.Distance(movePositionTransform.position, transform.position);
         
        // If within the follow range, then start following
        if (dist < followRange || followRange <= 0)
        {
            // Set destination to players location
            navMeshAgent.destination = movePositionTransform.position;
            animator.SetBool("isWalking", true);
        }
        else
        {
            navMeshAgent.destination = this.movePositionTransform.position;
            animator.SetBool("isWalking", false);
        }

        if (dist < lightUpDistance && replace.name == "Drone_Walker Dark")
        {
            GameObject replacement = Instantiate(replace, this.transform.position, this.transform.rotation);
            
            replacement.transform.parent = this.transform.parent;
            NavMeshAgent c = replacement.GetComponent<NavMeshAgent>();
            c.speed = this.navMeshAgent.speed + 5.5f;
            
            AudioSource replacementAudio = replacement.GetComponent<AudioSource>();
            if (!soundPlay)
            {
                soundPlay = true;
                
                replacementAudio.PlayOneShot(scream);
            }
            
            Destroy(this.gameObject);
        }
        else if (dist >= lightUpDistance && replace.name == "Drone_Walker Day (Creepy)")
        {
            
            GameObject replacement = Instantiate(replace, this.transform.position, this.transform.rotation);
            replacement.transform.parent = this.transform.parent;
            NavMeshAgent c = replacement.GetComponent<NavMeshAgent>();
            c.speed = this.navMeshAgent.speed - 5.5f;
            Destroy(this.gameObject);
        }

        if (autoDie && Mathf.Abs(this.transform.position.y - player.transform.position.y) < yBoundBox && Mathf.Abs(this.transform.position.x - player.transform.position.x) < xzBoundBox && Mathf.Abs(this.transform.position.z - player.transform.position.z) < xzBoundBox)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("die", true);

            while (temp.x > .001f)
            {
                temp = this.transform.localScale;
                temp.x -= Time.deltaTime;
                temp.y -= Time.deltaTime;
                temp.z -= Time.deltaTime;
                this.transform.localScale = temp;
            }
            
            player.GetComponent<Health>().ChangeHealthValue(-damage);
            
            Destroy(this.gameObject);
            
            
        }
        
    }

    
}

