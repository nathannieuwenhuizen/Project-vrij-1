using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    [SerializeField]
    private Transform menu;

    private Transform target;

    [SerializeField]
    private Vector3 velocity = Vector3.zero;
    [SerializeField]
    private float smoothTime;
    // Start is called before the first frame update
    void Start()
    {
        moveCameraToPos(menu);
    }
    public void moveCameraToPos(Transform pos)
    {
        if (target != null)
        {
            target.GetComponentInChildren<Canvas>().worldCamera = null;
        }

        target = pos;
        target.GetComponentInChildren<Canvas>().worldCamera = GetComponent<Camera>();
        target.GetComponentInChildren<Canvas>().GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
        StopAllCoroutines();
        StartCoroutine(MovingCameraToPos());
        //target.GetComponentInChildren<Canvas>().pixelRect
    }
    IEnumerator MovingCameraToPos()
    {
        while (Vector3.Distance(target.position, transform.position) > 0.1f)
        {
            // Define a target position above and behind the target transform
            Vector3 targetPosition = target.TransformPoint(new Vector3(0, 5, -10));

            // Smoothly move the camera towards that target position
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);


            // Smoothly move the camera towards that target position
            transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, Time.deltaTime / smoothTime);

            yield return new WaitForFixedUpdate();
        }
        transform.position = target.position;
        transform.rotation = target.rotation;
    }
}
