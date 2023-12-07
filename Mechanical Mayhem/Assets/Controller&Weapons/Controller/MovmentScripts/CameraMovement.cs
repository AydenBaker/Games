using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraMovement : MonoBehaviour
{

    #region Variables
    private PlayerInput input;

    [Space]
    [Header("Camera movement settings")]

    [Tooltip("Player Movement object must be the parent of this gameobject.")]
    public GameObject player;
    [Space]
    
    [Tooltip("(recomended 70)")]
    [Range(10f, 100f)]
    [SerializeField] float sensetivity = 70;
    [Space]

    [Tooltip("100 = no smoothing, 10 = high smoothing. (recomended 65 - 90)")]
    [Range(10f, 150f)]
    [SerializeField] float CamSmoothing = 65f;
    [Space]

    [Tooltip("X = max, Y = min. (recomended 90, -90)")]
    [SerializeField] Vector2 clampValues = new Vector2(90f, -90f);
    
    //rotational data
    private float Yrot;
    private float Xrot;

    private Quaternion yrotation;
    private Quaternion xrotation;

    #endregion

    #region Methods

    private void Start()
    {
        input = PlayerInput.instance;
        Cursor.lockState = CursorLockMode.Locked;
    }

    //gets input and does calculaitons
    private void Update()
    {
        //gets multiplied inputs
        SetRotations();
       
    }

    private void LateUpdate()
    {
        //does camera rotations
        MoveCameraSmoothly(xrotation, yrotation, CamSmoothing * .2f * Time.deltaTime);
    }

    //sets yrot, xrot, yrotation, and xrotation
    private void SetRotations()
    {

        float mouseY = input.mouseInputs.y * (sensetivity * .02f);
        float mouseX = input.mouseInputs.x * (sensetivity * .02f);

        Yrot += mouseX;
        Xrot -= mouseY;

        Xrot = Mathf.Clamp(Xrot, clampValues.x, clampValues.y);

        yrotation = Quaternion.Euler(0f, Yrot, 0f);
        xrotation = Quaternion.Euler(Xrot, 0f, 0f);

    }

    //moves the camera according to quaternion values
    private void MoveCameraSmoothly(Quaternion _xrot, Quaternion _yrot, float _camSmoothing) 
    {

        //moves the x-Axis(up and down)
        transform.localRotation = Quaternion.Lerp(transform.localRotation, _xrot, _camSmoothing);

        //rotates a character controller
        player.GetComponent<CharacterController>().transform.localRotation = Quaternion.Lerp(player.transform.localRotation, _yrot, _camSmoothing);

    }
    #endregion

}
