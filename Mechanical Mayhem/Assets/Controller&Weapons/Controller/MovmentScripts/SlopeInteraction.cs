using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CCPlayerMovement))]
public class SlopeInteraction : MonoBehaviour
{

    //variables
    [Space]
    [Header("Slope Values")]

    [Tooltip("make sure your values our between 0.1 and 1.0")]
    public float maxAngle = .55f;

    //slope detection
    [Tooltip("make sure your values our between 0.1 and 1.0")]
    [SerializeField] private float slopeDetectionRayLength = .55f;
    [SerializeField] private float slopeDownForce = 80f;

    [Range(1, 10)]
    [SerializeField] private float slopeDownForceMultiplier = 10;

    private float playersLastYPos = 0;

    //class refrences
    private CCPlayerMovement playerMovment;
    private PlayerInput input;

    //methods
    private void Awake()
    {
        playerMovment = this.GetComponent<CCPlayerMovement>();
    }

    private void Start()
    {
        input = PlayerInput.instance;
    }

    private void Update()
    {
       
        CharacterController controller = playerMovment.getController();

        //Getting if player is on a slope
        if (IsOnSlope(CreateRayCast(controller.transform.position, controller.height / 2 + slopeDetectionRayLength)))
        {
            if (!IsCharacterMovingUp(controller.transform.position.y) && !input.oldJumpInput && playerMovment.IsGrounded())
            {
                //adds to addedc gravity value in the CCPlayerClass
                playerMovment.AddGravity(slopeDownForce * slopeDownForceMultiplier);
            }

            //if true then the player should not be on a slope
            if (input.oldJumpInput)
            {
                //resets added gravitys value in the CCPlayerClass
                playerMovment.AddGravity(0);
            }
        }
        else
        {
            //resets added gravitys value in the CCPlayerClass
            playerMovment.AddGravity(0);
        }
    }

    private RaycastHit CreateRayCast(Vector3 startPos, float rayDistance)
    {

        RaycastHit _hit;

        Physics.Raycast(startPos, Vector3.down, out _hit, rayDistance, playerMovment.getCastLayer());

        //for debuging
        Vector3 lineEndPoint = new Vector3(startPos.x, startPos.y - rayDistance, startPos.z);
        Debug.DrawLine(startPos, lineEndPoint, Color.red);

        return _hit;

    }

    private bool IsOnSlope(RaycastHit hit)
    {

        float pointAngle = hit.normal.y;
        if(pointAngle < 1 && pointAngle >= maxAngle)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    private bool IsCharacterMovingUp(float _yAxis)
    {
        if(_yAxis > playersLastYPos)
        {
            //sets the new last pos equal to this new pos
            playersLastYPos = _yAxis;
            return true;
        }
        else
        {
            playersLastYPos = _yAxis;
            return false;
        }
    }

}
