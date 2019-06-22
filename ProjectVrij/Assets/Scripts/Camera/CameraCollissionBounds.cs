using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollissionBounds : MonoBehaviour
{

    public GameObject cObject;
    public float cDistance;


    private float sphereRadius = .4f;
    private float maxDistance;
    private float minDistance = 1f;
    [SerializeField]
    private LayerMask layerMask;

    private Vector3 origin;
    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        maxDistance = Vector3.Distance(transform.position, transform.parent.position);
    }

    // Update is called once per frame
    void Update()
    {
        origin = transform.parent.position;
        direction = -transform.forward;// transform.position - transform.parent.transform.position;
        RaycastHit hit;
        if (Physics.SphereCast(origin, sphereRadius, direction, out hit, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal))
        {
            
            if (!hit.collider.isTrigger)
            {
                cObject = hit.transform.gameObject;
                cDistance = Mathf.Max(minDistance, hit.distance);
            } else
            {
                cObject = null;
                cDistance = maxDistance;
            }
        } else
        {
            cObject = null;
            cDistance = maxDistance;
        }
        //transform.position = Vector3.Lerp(transform.position, origin + direction * cDistance, Time.deltaTime * 10f);
        transform.position = origin + direction * cDistance;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Debug.DrawLine(origin, origin + direction * cDistance);
        Gizmos.DrawWireSphere(origin + direction * cDistance, sphereRadius);
    }
}
