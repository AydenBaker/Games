using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotate : MonoBehaviour
{

    public float rotateSpeed = 5f;
    

    void Update()
    {

        this.transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
        
    }
}
