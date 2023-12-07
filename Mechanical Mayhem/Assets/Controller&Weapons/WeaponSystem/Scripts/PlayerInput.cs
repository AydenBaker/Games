using System.Collections;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    #region Variables
    [Space]
    [Header("Input Settings")]

    [SerializeField] private bool createInputs = false;

    //class Singleton
    public static PlayerInput instance { get; private set; }

    //controller movement
    public Vector2 HVInput { get; private set; }
    public bool Sprint { get; private set; }
    public float jumpInput { get; private set; }
    public bool oldJumpInput { get; private set; }
    public Vector2 mouseInputs { get; private set; }
    public bool dashing { get; private set; }

    //weapon inputs
    public bool shoot { get; private set; }
    public bool shootSemi { get; private set; }
    public bool reload { get; private set; }
    public bool throwGernade { get; private set; }
    public bool switchGernade { get; private set; }
    public float switchingGuns { get; private set; }

    
    
    
    //enviroment interactions
    public bool DoorInteraction { get; private set; }

    public bool PickUpGun { get; private set; }

    #endregion

    #region Methods

    private void Awake()
    {
        CheckIfMultipleScripts();
    }

    private void Update()
    {
        if (createInputs)
        {
            CreateInputs();
        }
        else
        {
            GetOldInputs();
        }
    }

    //using Input.GetButton 
    private void GetOldInputs()
    {

        //camera inputs
        mouseInputs = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));


        //movement inputs
        HVInput = new Vector2 (Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Sprint = Input.GetButton("Sprint");
        dashing = Input.GetButton("Dash");
        oldJumpInput = Input.GetButtonDown("Jump");


        //weapon inputs
        shoot = Input.GetButton("Shoot");
        shootSemi = Input.GetButtonDown("Shoot");
        reload = Input.GetButton("Reload");

        throwGernade = Input.GetButtonDown("GernadeThrow");
        switchGernade = Input.GetButtonDown("SwitchGernade");

        switchingGuns = Input.GetAxis("Mouse ScrollWheel");


        //enviroment interaction inputs
        DoorInteraction = Input.GetButtonDown("Door");

    }

    //using Input.GetKey
    private void CreateInputs()
    {

        //camera inputs
        mouseInputs = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));


        //movement inputs
        HVInput = new Vector2 (Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Sprint = Input.GetKey(KeyCode.LeftShift);
        dashing = Input.GetKey(KeyCode.LeftShift);
        oldJumpInput = Input.GetKeyDown(KeyCode.Space);

        
        //weapon inputs
        shoot = Input.GetMouseButton(0);
        shootSemi = Input.GetMouseButtonDown(0);
        reload = Input.GetKeyDown(KeyCode.R);

        throwGernade = Input.GetKeyDown(KeyCode.E);
        switchGernade = Input.GetKeyDown(KeyCode.T);

        switchingGuns = Input.mouseScrollDelta.y;


        //enviroment interaction inputs
        DoorInteraction = Input.GetKeyDown(KeyCode.F);

        PickUpGun = Input.GetKeyDown(KeyCode.F);

    }

    //makes sure only one instanse of this class excists
    private void CheckIfMultipleScripts()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    #endregion

}
