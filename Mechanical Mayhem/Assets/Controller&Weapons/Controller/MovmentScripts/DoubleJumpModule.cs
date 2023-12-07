using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CCPlayerMovement))]
public class DoubleJumpModule : MonoBehaviour
{

    [Tooltip("Amount of jumps in the air, One jump would be equal to a total of two jumps")]
    [SerializeField]private int amountOfJumps = 2;

    //class refrences
    private CCPlayerMovement playerMovement;
    private PlayerInput input;

    //methods
    private void Awake()
    {
        playerMovement = this.GetComponent<CCPlayerMovement>();

    }

    private void Start()
    {
        input = PlayerInput.instance;
    }

    private void Update()
    {
        //see if the player can and wants to jump
        //starts IEnumerator
        if (!playerMovement.IsGrounded())
        {
            StartCoroutine(DoubleJumpDelay()); // starts the double jump coroutine 
        }
    }

    IEnumerator DoubleJumpDelay()
    {

        yield return null; //yeilds on frame so that input is not confused

        //checks if the player wants to double jump
        if (input.oldJumpInput && CanJumpAgain(playerMovement.getAmountOfJumps())) //checks if the player wants to jump and if they have anymore jumps left
        {
            playerMovement.Jump();

            //add to the amount of times the player has jumped
            if (CanJumpAgain(playerMovement.getAmountOfJumps()))
            {
                playerMovement.setAmountOfJumps(playerMovement.getAmountOfJumps() + 1); 
            }
        }

    }

    private bool CanJumpAgain(int jumps)
    {
        
        return jumps < amountOfJumps;
    }
    
    private void JumpEffects()
    {
        //effects to add when you double jump
    }

}
