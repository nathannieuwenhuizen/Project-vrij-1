using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
public class InputHandler : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement pm;
    [SerializeField]
    private CameraMovement cm;
    [SerializeField]
    private string controllerID = "";

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(controllerID == "1" ? KeyCode.Joystick1Button3 : KeyCode.Joystick2Button3))
        {
            pm.BasicAttack(true);
        } else
        {
            pm.BasicAttack(false);
        }
        if (Input.GetKey(controllerID == "1" ? KeyCode.Joystick1Button1 : KeyCode.Joystick2Button1))
        {
            pm.Jump();
        }

        //if (Input.GetButtonDown("BasicAttack" + controllerID))
        //{

        //    pm.BasicAttack(true);
        //}
        //else
        //{
        //}

        //if (Input.GetButtonDown("Jump" + controllerID))
        //{
        //    pm.Jump();
        //}
        pm.Walking(CrossPlatformInputManager.GetAxis("Vertical" + controllerID), -CrossPlatformInputManager.GetAxis("Horizontal" + controllerID));
        pm.Rotate(CrossPlatformInputManager.GetAxis("RotateHorizontal" + controllerID));

        cm.Rotate(CrossPlatformInputManager.GetAxis("RotateVertical" + controllerID));
    }
}
