using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Camera camera;
    private Vector2 cameraRotationSpeed = new Vector2();
    private Vector2 cameraRotation = new Vector2();

    private bool snapToFront = false;
    [Header("for designer")]
    [SerializeField]
    private Vector2 rotationSpeed;



    // Start is called before the first frame update
    void Start()
    {
        camera = transform.GetComponentInChildren<Camera>();
    }

    void Update()
    {
        CameraSpeed = new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
        UpdateCamera();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SnapToFront();
        }
    }
    void SnapToFront()
    {
        if (snapToFront == true)
        {
            return;
        }
        snapToFront = true;
        StartCoroutine(SnappingToFront());
    }
    IEnumerator SnappingToFront()
    {
        Vector3 targetDir = Vector3.zero;
        float start = Time.time;
        float step = 0f;

        while (step < .9f)
        {
            step = Time.time - start;

            // The step size is equal to speed times frame time.
            CameraRotation = Vector3.Lerp(CameraRotation, targetDir, step);

            yield return new WaitForFixedUpdate();
        }
        snapToFront = false;
    }


    void UpdateCamera()
    {
        if (cameraRotationSpeed.x != 0 || cameraRotationSpeed.y != 0)
        {
            StopCoroutine(SnappingToFront());
            CameraRotation += cameraRotationSpeed;
        }
    }
    public Vector2 CameraSpeed
    {
        get { return cameraRotationSpeed; }
        set
        {
            value.x *= rotationSpeed.x;
            value.y *= rotationSpeed.y;

            cameraRotationSpeed = value;
        }
    }
    public Vector2 CameraRotation
    {
        get { return cameraRotation; }
        set {
            cameraRotation = value;
            transform.rotation = Quaternion.Euler(cameraRotation);
        }
    }
}
