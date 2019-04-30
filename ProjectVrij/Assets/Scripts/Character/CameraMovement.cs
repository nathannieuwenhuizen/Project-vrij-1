using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Camera camera;
    private Vector2 cameraRotationSpeed = new Vector2();
    private Vector2 cameraRotation = new Vector2();

    [Header("for designer")]
    [SerializeField]
    private Vector2 rotationSpeed;

    [SerializeField]
    [Range(0f, 89f)]
    private float maxDownAngle = 45f;

    [SerializeField]
    [Range(0f, 89f)]
    private float maxUpAngle = 45f;

    // Start is called before the first frame update
    void Start()
    {
        camera = transform.GetComponentInChildren<Camera>();
    }

    public void Rotate(float y_input)
    {
        transform.Rotate(new Vector3(y_input, 0, 0));

        Vector3 currentRotation = transform.localRotation.eulerAngles;
        if (currentRotation.x > 180)
        {
            currentRotation.x = -360 + currentRotation.x; 
        }


        currentRotation.x = Mathf.Clamp(currentRotation.x, -maxUpAngle, maxDownAngle);
        transform.localRotation = Quaternion.Euler(currentRotation);
    }

}
