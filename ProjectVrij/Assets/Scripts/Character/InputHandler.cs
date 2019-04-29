using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (Input.GetButtonDown("Jump" + controllerID))
        {
            pm.Jump();
        }
        pm.Walking(Input.GetAxis("Horizontal" + controllerID), Input.GetAxis("Vertical" + controllerID));
        pm.Rotate(Input.GetAxis("RotateHorizontal" + controllerID));

        cm.Rotate(Input.GetAxis("RotateVertical" + controllerID));
    }
}
