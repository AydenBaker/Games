using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LongRangedWeapon))]
public class ShotGunModule : MonoBehaviour
{

    [Space]
    [Header("Shot Settings")]
    //variables
    [SerializeField] float rayAdjacentAngle = 15f;
    [SerializeField] float rayAngleDivider = .25f;
    private int amountOfRays = 9;

    [Tooltip("Recomended to turn this one")]
    [SerializeField] bool autoCalculateRayPos = true;
    
    [Header("--------------------------------------------------")]
    [Tooltip("only needs to be set if you have autoCalculateRayPos disabled")]
    [SerializeField] Vector3[] rayPoses = new Vector3[9];


    private LongRangedWeapon weapon;


    //methods
    private void Awake()
    {

        weapon = this.GetComponent<LongRangedWeapon>();

        if (autoCalculateRayPos)
        {
            SetRayRotations();
        }

    }

    public void Update()
    {
        //checks if the player has shot
        if (weapon.GetIsShooting())
        {
            shotGunDamage();
        }
    }


    //sets each rays rotation
    private void SetRayRotations()
    {
        //hard code, change later if you figure out a way
        rayPoses[0] = new Vector3(0, 0, 90);
        rayPoses[1] = new Vector3(rayAdjacentAngle, 0, 90);
        rayPoses[2] = new Vector3(-rayAdjacentAngle, 0, 90);
        rayPoses[3] = new Vector3(0, rayAdjacentAngle, 90);
        rayPoses[4] = new Vector3(0, -rayAdjacentAngle, 90);
        rayPoses[5] = new Vector3(-rayAdjacentAngle * rayAngleDivider, rayAdjacentAngle * rayAngleDivider, 90);
        rayPoses[6] = new Vector3(rayAdjacentAngle * rayAngleDivider, rayAdjacentAngle * rayAngleDivider, 90);
        rayPoses[7] = new Vector3(-rayAdjacentAngle * rayAngleDivider, -rayAdjacentAngle * rayAngleDivider, 90);
        rayPoses[8] = new Vector3(rayAdjacentAngle * rayAngleDivider, -rayAdjacentAngle * rayAngleDivider, 90);
    }

    //deals damage to any anemy with in the range
    private void shotGunDamage()
    {

        //holds all the hit points
        RaycastHit[] shotGunHits = GetShotGunRays(weapon.GetCam(), weapon.GetMaxRayDistance(), weapon.GetLayerToHit());

        //does damage to enemy
        foreach (RaycastHit hits in shotGunHits)
        {
            if (hits.collider != null && hits.collider.CompareTag("Enemy"))
            {
                GameObject enemy = hits.collider.gameObject;
                enemy.GetComponent<Health>().ChangeHealthValue(-weapon.GetRayCastBulletDamage());

            }

            //spawns in bullet impact effect
            weapon.InstantiateBulletImpactAtPoint(hits);
        }

    }

    //gets all the hit information from the raycasts
    public RaycastHit[] GetShotGunRays(Transform startPos, float rayLength,LayerMask layers)
    {
        RaycastHit[] hits = new RaycastHit[9];

        for (int i = 0; i < amountOfRays; i++)
        {

            if (Physics.Raycast(startPos.position, startPos.TransformDirection(rayPoses[i]), out hits[i], rayLength, layers))
            {
                Debug.DrawLine(startPos.position, startPos.TransformDirection(rayPoses[i]), Color.yellow);
            }
            
        }

        return hits;
    }

}
