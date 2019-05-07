using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
public class InputHandler : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement pm;
    [SerializeField]
    private Weapon weapon;
    [SerializeField]
    private string controllerID = "";


    //buttons
    private KeyCode baseAttackCode;
    private KeyCode jumpCode;
    private KeyCode shootCode;

    private void Start()
    {

        //controller identification for the buttons
        switch (controllerID)
        {
            case "1":
                baseAttackCode = KeyCode.Joystick1Button3;
                jumpCode =       KeyCode.Joystick1Button0;
                shootCode =      KeyCode.Joystick1Button2;
                break;
            case "2":
                baseAttackCode = KeyCode.Joystick2Button3;
                jumpCode =       KeyCode.Joystick2Button0;
                shootCode =      KeyCode.Joystick2Button2;

                break;
            case "3":
                baseAttackCode = KeyCode.Joystick3Button3;
                jumpCode =       KeyCode.Joystick3Button0;
                shootCode =      KeyCode.Joystick3Button2;

                break;
            case "4":
                baseAttackCode = KeyCode.Joystick4Button3;
                jumpCode =       KeyCode.Joystick4Button0;
                shootCode =      KeyCode.Joystick4Button2;
                break;
            default:
                baseAttackCode = KeyCode.P;
                jumpCode =       KeyCode.L;
                shootCode =      KeyCode.M;
                break;
        }
    }
    void Update()
    {
        if (Input.GetKey(baseAttackCode))
        {
            pm.BasicAttack(true);
        } else
        {
            pm.BasicAttack(false);
        }
        if (Input.GetKey(jumpCode))
        {
            pm.Jump();
        }
        if (Input.GetKey(shootCode))
        {
            weapon.Shoot();
        }

        //player movement
        pm.Walking(CrossPlatformInputManager.GetAxis("Vertical" + controllerID), -CrossPlatformInputManager.GetAxis("Horizontal" + controllerID));
        //player view change
        pm.Rotate(
            CrossPlatformInputManager.GetAxis("RotateHorizontal" + controllerID),
            CrossPlatformInputManager.GetAxis("RotateVertical" + controllerID)
            );

        //just for testing on keyboard...
        //pm.Rotate(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }
}
