using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CCPlayerMovement))] //reguires the game object to have a CCplayerMovement class
public class DashingModule : MonoBehaviour
{

    [Space]
    [Header("Dash Values")]
    [Space]

    [Tooltip("Recomended 1.25f")]
    [SerializeField] private float dashSpeed = 1.25f;

    [Tooltip("Recomended .25f")]
    [SerializeField] private float dashDuration = .25f;

    [Tooltip("delay between each dash")]
    [SerializeField] private float dashDelay = .5f;

    //class refrences
    private CCPlayerMovement playerMovement;
    private PlayerInput input;

    //required boolean
    private bool canDash = true;

    //methods
    private void Awake()
    {
        //gets player controller class 
        playerMovement = this.GetComponent<CCPlayerMovement>();
    }

    private void Start()
    {
        //gets input
        input = PlayerInput.instance;
    }

    public void Update()
    {
        //checks if player can dash, then checks if the player wants to dash.
        //starts corutine for dash IEnumerator
        if (input.dashing && canDash)
        {
            StartCoroutine(Dashing(dashDelay, dashDuration, 
                new Vector3(playerMovement.getHVInputs().x, 0, playerMovement.getHVInputs().y)));
        }
    }

    //dashing logic
    IEnumerator Dashing(float dashDelay, float dashDuration, Vector3 position)
    {
        playerMovement.setIsDashing(true);
        //dash activated
        float time = Time.time;

        canDash = false;

        while (Time.time < time + dashDuration)
        {
            //dash for an amount of time
            playerMovement.MoveCharacter(ActivateDash(position));
            yield return null;
        }

        playerMovement.setIsDashing(false);

        yield return new WaitForSeconds(dashDelay);//dash cool down
        canDash = true;

    }

    //returns a new vector3 for movement
    private Vector3 ActivateDash(Vector3 orginalVector)
    {
       
        return new Vector3(orginalVector.x * dashSpeed, orginalVector.y, orginalVector.z * dashSpeed);
        
    }

}
