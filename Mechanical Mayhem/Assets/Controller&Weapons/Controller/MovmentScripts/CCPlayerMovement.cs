using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public class CCPlayerMovement : MonoBehaviour
{

    #region Variables

        #region public
    //variables visible in inspector
    [Space]
    [Header("Controller Speed Movement Values")]

    [SerializeField] private float speed = 18f;
    [SerializeField] private float speedMultiplier = 1f;

    [Tooltip("values should be from 0 - 1, 0 being no smoothing and 1 being max smoothing")]
    [SerializeField] private float speedSmoothing = .2f;
    private Vector2 currentInputVector;
    private Vector2 vectorVelocity;

    [Tooltip("if you dont want to divide the value set it to 1, else set it from 1.0 - 10.0 for best results")]
    [SerializeField] private float inAirNoInputDivider = 1.5f;


    [Space]
    [Header("Ground Detection")]

    [SerializeField] private float sphereCastRadius = .35f;
    private float maxSphereCastLength = 0f;

    [Tooltip("the size of an object your able to step over, for example stairs. (recomended .5f)")]
    [SerializeField] private float stepOffsetValue = .5f;

    [SerializeField] private LayerMask castLayers;


    [Space]
    [Header("Jumping Mechanics")]

    [SerializeField] private bool canMoveInAir = true;

    [SerializeField] private float gravity = 14.5f;
    private float anyAdditionalGravity = 0f;

    [Tooltip("Values should be between 1 and 5")]
    [SerializeField] private float constantGravity = 4f; 

    private float jumpSpeed = -2f;
    [SerializeField] private float jumpHeight = 3f;

    //used for simulated gravity
    private Vector3 yVelocity;

    #endregion

        #region private

    //class refrences
    [HideInInspector]
    private PlayerInput input;
    private CharacterController controller;


    //raycast variables for ground detection
    private RaycastHit hit; // will be used to get ground tag and add approprate sound effects
    private Vector3 sphereOrgin = Vector3.zero;


    //vector2 input vectors
    private Vector2 lastInput;
    private Vector2 hVInputs;


    //amount of times the player has jumped before landing on the ground
    private int amountOfJumps = 0;

    #endregion

        #region Modifications
    //used for dashing refrence
    [HideInInspector] public bool isDashing = false;

         #endregion

    #endregion

    #region Methods

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Start()
    {
        input = PlayerInput.instance;
    }

    void Update()
    {
        
        //sets HVInputs from the Input class times speed and applies delta time to the value
        Vector2 playerInput = input.HVInput.normalized;
        playerInput *= (speed * speedMultiplier) * Time.deltaTime;
        
        //defined so we can use this variable in other classes
        hVInputs = playerInput;

        if (IsGrounded())
        {
            lastInput = GetLastInput(playerInput);
            
            Jump();
        }
        else if (!IsGrounded())
        {
            //gets how the player should move if there is no input in the air
            playerInput = MovingInAirWithNoInput(playerInput);
        }
        
       
        
        else
        {
            //resets lastInput
            //lastInput = Vector2.zero;

            //Jumping logic
            //calls jump method to check if the player wants to jump 
            
        }

        //checks if the  stepOffset need to be 0 or a higher value
        if (input.oldJumpInput || !IsGrounded())
        {
            StartCoroutine(CancelStepOffset());
        }
        


        //Movement vector math & smoothing logic
        //smooths the players input vector, this makes the game less jarring to play.
        currentInputVector = Vector2.SmoothDamp(currentInputVector, playerInput, ref vectorVelocity, speedSmoothing);

        //The amount the player needs to move, based on the input multliplied by speed
        Vector3 newPlayerPos = new Vector3(currentInputVector.x, 0, currentInputVector.y);

        //moves the character down. this movement simulates gravity

        float _gravity = gravity + anyAdditionalGravity;

        //stops the controller from snaping to slopes
        if (IsGrounded())
        {
            //checks if the velocity needs to be reset
            if (yVelocity.y < 0f && anyAdditionalGravity > 0)
            {
                //if your on a slope
                yVelocity.y += -(constantGravity - anyAdditionalGravity) * Time.deltaTime;
            }
            else if(yVelocity.y < 0f)
            {
                //if your on normal ground
                yVelocity.y = -constantGravity;
            }
        }

        //if your in the air
        yVelocity.y += -(_gravity * Time.deltaTime);
        controller.Move(yVelocity * Time.deltaTime);

        //calls the movement method and uses newPlayerPos as a Vector3 parameter
        MoveCharacter(newPlayerPos);

    }


    //this method checks if an axis in a Vector2 is greater than 0 >= 1
    private bool ActiveInput(Vector2 _input)
    {

        bool activeInputs = false;

        //checks if theres the player's input is less than 1, to do this we get the absolute value of each Axis
        if (Mathf.Abs(_input.x) <= 1 && _input.x != 0 || Mathf.Abs(_input.y) <= 1 && _input.y != 0)
        {
            activeInputs = true;
        }

        return activeInputs;

    }

    //checks if the Controller is touching the ground
    public bool IsGrounded()
    {

        //gets the bottom of the player controller
        maxSphereCastLength = controller.height / 2;

        //gets gizmo calculation so you can see it in the scene
        sphereOrgin = new Vector3(controller.transform.position.x, controller.transform.position.y - maxSphereCastLength, controller.transform.position.z);

        //checks if a sphereCast detects the ground and returns the boolean
        return Physics.SphereCast(controller.transform.position, sphereCastRadius, Vector3.down, out hit, maxSphereCastLength, castLayers, QueryTriggerInteraction.UseGlobal);

    }

    //method returns the last vector value while the ActiveInput of the parameter is true
    private Vector2 GetLastInput(Vector2 _input)
    {
        Vector2 lastInput = _input;

        //checks if the user has entered any input, if the player has it will set the lastInput to _input
       
        if (!canMoveInAir && ActiveInput(_input))
        {
            lastInput = _input;
        }
        else  if (canMoveInAir && ActiveInput(_input))
        {
            lastInput = _input;
        }

        return lastInput;
    }

    //divides the parameter vector two by the inAirNoInputDivider value, if there is no input, then returns that Vector2
    private Vector2 MovingInAirWithNoInput(Vector2 _input)
    {
        if (!canMoveInAir)
        {
            _input = lastInput / inAirNoInputDivider;
        }
        else if (!ActiveInput(_input))
        {
            _input = lastInput / inAirNoInputDivider;
        }

        return _input;

    }

    //checks for jump input form player, if true, the method will move the controller upwards
    public void Jump()
    {
        amountOfJumps = 0; //resets the amount of jumps because when the player is on the ground they arn't jumping

        //checks if the player wants to jump, if the player does then it will move the controller
        if (input.oldJumpInput) //does the player want to jump
        {
            yVelocity.y = Mathf.Sqrt(jumpHeight * jumpSpeed * -gravity);

            amountOfJumps++;
        }

    }

    //moves the character by the Vector3 parameter.
    public void MoveCharacter(Vector3 _input)
    {
        //gets how the player should move depending on its direction
        Vector3 movementVector = _input;

        if (IsGrounded())
        {
            movementVector = controller.transform.TransformDirection(movementVector);
        }
        else if (!canMoveInAir)
        {
            movementVector = controller.transform.TransformDirection(movementVector);
        }
        else
        {
            movementVector = controller.transform.TransformDirection(movementVector);
        }
        

        //moves the character controller with movementVector
        controller.Move(movementVector);
    }
    
    

    //this fixes the Character Controller broken step effset value
    private IEnumerator CancelStepOffset()
    {
        //when off the ground sets stepOffset to 0, this gets ride of any gitter from the controller
        controller.stepOffset = 0f;

        //checks if the player is actually off the ground
        yield return new WaitUntil(() => !IsGrounded());

        //checks if the controller is back on the ground, if so then it gives it its original stepOffset 
        if (IsGrounded())
        {
            controller.stepOffset = stepOffsetValue;
        }

        yield break;
    }



    //getters
    public CharacterController getController()
    {
        return controller;
    }

    public LayerMask getCastLayer()
    {
        return castLayers;
    }

    public bool getIsDashing()
    {
        return isDashing;
    }

    public int getAmountOfJumps() 
    { 
        return amountOfJumps; 
    }

    public float getNormalSpeed()
    {
        return speed;
    }

    public Vector2 getHVInputs()
    {
        return hVInputs;
    }

    public bool getCanMoveInAir()
    {
        return this.canMoveInAir; 
    }

    //setters
    public void setIsDashing(bool isDashing)
    {
        this.isDashing = isDashing;
    }

    public void setAmountOfJumps(int amountOfJumps)
    {
        this.amountOfJumps = amountOfJumps;
    }

    public void setSpeed(float speed)
    {
        this.speed = speed;
    }

    public void AddGravity(float downForce)
    {
        anyAdditionalGravity = downForce;
    }



    //Debugging
    private void OnDrawGizmosSelected()
    {
        //draws the gizmo
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(sphereOrgin, sphereCastRadius);
    }

    #endregion 

}