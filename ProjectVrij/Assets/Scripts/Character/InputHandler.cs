using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
public class InputHandler : MonoBehaviour
{
    private Character character;

    [SerializeField]
    private int controllerID = 0;


    //buttons
    private KeyCode baseAttackCode;
    private KeyCode jumpCode;
    private KeyCode specialAttackCode;

    private void Start()
    {
        //finds the character script.
        if (GetComponent<MeleeCharacter>())
        {
            character = GetComponent<MeleeCharacter>();
        }

        if (GetComponent<CloseRangedCharacter>())
        {
            character = GetComponent<CloseRangedCharacter>();
        }
        ConfigureControlButtons();
        
    }

    /// <summary>
    /// Here the button mapping is decided on what the controller ID is
    /// </summary>
    public void ConfigureControlButtons()
    {
        //controller identification for the buttons
        switch (controllerID)
        {
            case 1:
                baseAttackCode = KeyCode.Joystick1Button3;
                jumpCode = KeyCode.Joystick1Button0;
                specialAttackCode = KeyCode.Joystick1Button2;
                break;
            case 2:
                baseAttackCode = KeyCode.Joystick2Button3;
                jumpCode = KeyCode.Joystick2Button0;
                specialAttackCode = KeyCode.Joystick2Button2;

                break;
            case 3:
                baseAttackCode = KeyCode.Joystick3Button3;
                jumpCode = KeyCode.Joystick3Button0;
                specialAttackCode = KeyCode.Joystick3Button2;

                break;
            case 4:
                baseAttackCode = KeyCode.Joystick4Button3;
                jumpCode = KeyCode.Joystick4Button0;
                specialAttackCode = KeyCode.Joystick4Button2;
                break;
            default:
                baseAttackCode = KeyCode.P;
                jumpCode = KeyCode.L;
                specialAttackCode = KeyCode.M;
                break;
        }
    }

    void Update()
    {
        //Checks whether a button is pressed down.
        if (Input.GetKeyDown(baseAttackCode))
        {
            character.BasicAttack(true);
        } else
        {
            character.BasicAttack(false);
        }
        if (Input.GetKeyDown(jumpCode))
        {
            character.Jump();
        }
        if (Input.GetKeyDown(specialAttackCode))
        {
            character.SpecialAttack();
        }

        //the axises -----------------------------------------

        //player movement
        character.Walking(CrossPlatformInputManager.GetAxis("Vertical" + controllerID), -CrossPlatformInputManager.GetAxis("Horizontal" + controllerID));
        //player view change
        character.Rotate(
            CrossPlatformInputManager.GetAxis("RotateHorizontal" + controllerID),
            CrossPlatformInputManager.GetAxis("RotateVertical" + controllerID)
            );
    }
}
