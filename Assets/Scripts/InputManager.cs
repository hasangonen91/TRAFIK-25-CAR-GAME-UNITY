//91
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class InputManager : MonoBehaviour
{
    public float vertical;
    public float horizontal;
    public bool Brake;

    private void FixedUpdate()
    {
        // vertical = 1f;
        // Brake = (Input.GetAxis("Jump")!=0) ? true : false;
        horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        vertical = CrossPlatformInputManager.GetAxis("Vertical");

        if (PlayerPrefs.GetInt("tilt") != 1)
        {
            if (vertical < 0)
                vertical = -1f;
            else
                vertical = 1f;

        }
    }
}
