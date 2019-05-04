using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Camera camera;
    private Vector2 cameraRotationSpeed = new Vector2();
    private Vector2 cameraRotation = new Vector2();

    [Header("Rotation Sensetivity")]
    public float rotationSpeedY;
    public float rotationSpeedX;

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
        
    //hallo sukkels heheheheheh xx bbq

    public void Rotate(float y_input)
    {
        transform.Rotate(new Vector3(y_input * rotationSpeedX, 0, 0));

        Vector3 currentRotation = transform.localRotation.eulerAngles;

        
        if (currentRotation.x > 180)
        {
            currentRotation.x = -360 + currentRotation.x; 
        }
        

        currentRotation.x = Mathf.Clamp(currentRotation.x, -maxUpAngle, maxDownAngle);
        transform.localRotation = Quaternion.Euler(currentRotation);
    }

}
//HAAAAAAAAAAAAHHHHHHHHHH hallo bbq back at the epic pranks again he ik wil kaasDENNIS WILTME VERMOORDEN HELP PLSSSSS
//ALLO EEN KINKER HIER BO IS GEK EN GSTOORD *DAB* CTRL Z DIE SHIT EENS EFFEN HEEL SEL JOU MOTHER FUCKING REPTIEL
//EEN KIKKER IS EEN AMFIBIE JIJ DOMME EMO KECH
//UM hallo ik ben de weeb uit de vrienden groep mireille is de eoe check je prioritijten jij cunt
//first of all how dare you. Secondly Joey is de weeb van onze vriendengroep jij sunkel
//kech!
// wajow dennis eens ff heels sne oppassrn anders steek ik je maar dit keer met een mes inplaats van me vinger
//maakt me niks uit kech, ik gebruik de vinger, kijk maar uit.
//dit vindt ik vunzig en ongepast jongenman
//sappig
//oke kijk maar uit dan, zodra je dit leest krijg je een vinger in je zij.
//(.Y.)
//8====D
//Maar toch kwam het allemaal weer goed!
//hallo