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



    // Start is called before the first frame update
    void Start()
    {
        camera = transform.GetComponentInChildren<Camera>();
    }

    public void Rotate(float y_input)
    {
        transform.Rotate(new Vector3(y_input, 0, 0));
    }

}
