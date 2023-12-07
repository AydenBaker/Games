using UnityEngine;

[RequireComponent(typeof(CCPlayerMovement))]
public class SprintingModule : MonoBehaviour
{

    //sprinting speed
    [Tooltip("runningSpeed + normal speed = actual speed")]
    [SerializeField] private float runningSpeed = 7f;
    private float normalSpeed;

    [SerializeField] private bool canRunInAir = false;

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

        //gets both the new run speed and the normal speed
        normalSpeed = playerMovement.getNormalSpeed();
        runningSpeed = GetnewSpeed(playerMovement.getNormalSpeed());
    }

    private void Update()
    {

        float _speed = playerMovement.getNormalSpeed();

        //check if player wants to jump
        if (input.Sprint && playerMovement.IsGrounded() || input.Sprint && canRunInAir)
        {
            _speed = runningSpeed;
            playerMovement.setSpeed(_speed);
        }
        else
        {
            _speed = normalSpeed;
            playerMovement.setSpeed(_speed);
        }

    }

    private float GetnewSpeed(float normalSpeed)
    {
        return runningSpeed + normalSpeed;
    }

    //need to finish
    private void ActivateRunningEffects()
    {
        //add any noises, camera fov increase, or post processing
    }
    
}
