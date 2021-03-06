﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
public class InputHandler : MonoBehaviour
{
    private Character character;

    [SerializeField]
    private int controllerID = 0;

    private bool canOnlyMoveCamera = false;

    //buttons
    private KeyCode baseAttackCode;
    private KeyCode jumpCode;
    private KeyCode specialAttackCode;
    private KeyCode specialAttack2Code;
    private KeyCode startButton;
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
                baseAttackCode = KeyCode.Joystick1Button1;
                jumpCode = KeyCode.Joystick1Button0;
                specialAttackCode = KeyCode.Joystick1Button4;
                specialAttack2Code = KeyCode.Joystick1Button5;
                startButton = KeyCode.Joystick1Button7;
                break;
            case 2:
                baseAttackCode = KeyCode.Joystick2Button1;
                jumpCode = KeyCode.Joystick2Button0;
                specialAttackCode = KeyCode.Joystick2Button4;
                specialAttack2Code = KeyCode.Joystick2Button5;
                startButton = KeyCode.Joystick2Button7;
                break;
            case 3:
                baseAttackCode = KeyCode.Joystick3Button1;
                jumpCode = KeyCode.Joystick3Button0;
                specialAttackCode = KeyCode.Joystick3Button4;
                specialAttack2Code = KeyCode.Joystick3Button5;
                startButton = KeyCode.Joystick3Button7;
                break;
            case 4:
                baseAttackCode = KeyCode.Joystick4Button1;
                jumpCode = KeyCode.Joystick4Button0;
                specialAttackCode = KeyCode.Joystick4Button4;
                specialAttack2Code = KeyCode.Joystick4Button5;
                startButton = KeyCode.Joystick4Button7;
                break;
            default:
                baseAttackCode = KeyCode.P;
                jumpCode = KeyCode.Space;
                specialAttackCode = KeyCode.M;
                specialAttack2Code = KeyCode.K;
                startButton = KeyCode.Escape;
                break;
        }
    }

    public int ControllerID
    {
        get { return controllerID; }
        set
        {
            controllerID = value;
            ConfigureControlButtons();
        }
    }
    public bool CanOnlyMoveCamera
    {
        get { return canOnlyMoveCamera; }
        set
        {
            canOnlyMoveCamera = value;
        }
    }

    void Update()
    {
        ConfigureControlButtons();
        CheckInput();
    }
    void CheckInput()
    {
        if (Input.GetKeyDown(startButton))
        {
            GameManager.instance.Pause(Time.timeScale == 1);
        }

        if (Time.timeScale == 0) { return; }
        //player view change
        character.Rotate(
            CrossPlatformInputManager.GetAxis("RotateHorizontal" + controllerID),
            CrossPlatformInputManager.GetAxis("RotateVertical" + controllerID)
            );

        if (canOnlyMoveCamera) { return; }

        //Checks whether a button is pressed down.
        if (Input.GetKeyDown(baseAttackCode))
        {
            character.BasicAttack(true);
        }
        else
        {
            character.BasicAttack(false);
        }
        if (Input.GetKeyDown(jumpCode))
        {
            character.Jump();
        }

        if (Input.GetKeyDown(specialAttack2Code))
        {
            character.SecondSpecialAttack();
        }
        if (Input.GetKeyDown(specialAttackCode))
        {
            character.SpecialAttack();
        }
        if (Input.GetKeyUp(specialAttackCode))
        {
            character.SpecialAttackRelease();
        }
        //the axises -----------------------------------------

        //player movement
        character.Walking(CrossPlatformInputManager.GetAxis("Vertical" + controllerID), -CrossPlatformInputManager.GetAxis("Horizontal" + controllerID));
        Debug.Log("Rotating and walking");
    }
}
