using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwitchCanvas : MonoBehaviour
{
    public GameObject offCanvas;
    public GameObject onCanvas;
    public GameObject FirstObject;

    public void Switch()
    {
        offCanvas.SetActive(true);
        onCanvas.SetActive(false);
        //onCanvas[1].SetActive(false);
        GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(FirstObject, null);
    }
}
