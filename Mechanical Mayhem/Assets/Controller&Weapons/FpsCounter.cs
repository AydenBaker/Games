using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FpsCounter : MonoBehaviour
{
    [SerializeField] private Text fpsText;
    [SerializeField] private float timeToUpdateCounter = .2f;
    private float timer;

    // Update is called once per frame
    void Update()
    {
        if (timer > timeToUpdateCounter)
        {
            //show fps
            float fps = 1f / Time.unscaledDeltaTime;
            fpsText.text = "FPS: " + (int)fps;
            
            //resets timer
            timer = 0;
        }

        timer += Time.deltaTime;
    }
}
