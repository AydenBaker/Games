using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwayPosition : MonoBehaviour
{
    private Vector3 initialPos;
    [SerializeField] float smoothing = 8;
    [SerializeField] float speedMultiplier;

    private PlayerInput input;


    // Start is called before the first frame update
    void Start()
    {
        input = PlayerInput.instance;
        initialPos = this.transform.localPosition;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float mouseX = input.mouseInputs.x * speedMultiplier;
        float mouseY = input.mouseInputs.y * speedMultiplier;

        
        
        Vector3 finalPos = new Vector3(mouseX, mouseY, 0);
        this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, finalPos + initialPos, smoothing * Time.deltaTime);
    }
}
