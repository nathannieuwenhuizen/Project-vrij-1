using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

//source code: https://answers.unity.com/questions/293141/how-to-make-the-screen-go-red-like-this-example.html

public class CameraFade : MonoBehaviour
{
    public static Texture2D Fade;
    public bool fadingOut = false;
    public float alphaFadeValue = 0;
    public float fadeSpeed = 1;
    public Color fadingColor = Color.black;
    private Camera camera;
    private float yPos;

    //[SerializeField]
    //private GameObject fadeImage;
    void Start()
    {
        camera = GetComponent<Camera>();


        //GameObject.Instantiate(fadeImage);
        //fadeImage.transform.parent = Transform.FindObjectOfType<Canvas>().transform;
        //fadeImage.GetComponent<RectTransform>().position = camera.rect.position;
        //fadeImage.GetComponent<RectTransform>().sizeDelta = camera.rect.size;

        if (Fade == null)
        {
            Fade = new Texture2D(1, 1);
            Fade.SetPixel(0, 0, new Color(1, 1, 1, 1));

        }
        if (camera.rect.height != 1)
        {
            yPos = (camera.rect.y == 0 ? 0.5f : 0);
        } else
        {
            yPos = camera.rect.y;
        }
    }

    void Update()
    {
        alphaFadeValue = Mathf.Clamp01(alphaFadeValue + ((Time.deltaTime / fadeSpeed) * (fadingOut ? 1 : -1)));
        useGUILayout = alphaFadeValue != 0;
        if (alphaFadeValue != 0)
        {
            fadingColor.a = alphaFadeValue;
            Fade.SetPixel(0, 0, fadingColor);
            Fade.Apply();
        }
    }


    void OnGUI()
    {
        //if (GUI.Button(new Rect(10, 10, 150, 100), "I am a button"))
        //{
        //    print("You clicked the button!");
        //}
        GUI.depth = 1;
        if (camera != null)
        {
            Rect camRect = camera.rect;
            if (alphaFadeValue != 0 && Event.current.type == EventType.Repaint)
                GUI.DrawTexture(new Rect(camRect.x * Screen.width, yPos * Screen.height, camRect.width * Screen.width, camRect.height * Screen.height), Fade);
        }
        else
        {
            if (alphaFadeValue != 0 && Event.current.type == EventType.Repaint)
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Fade);
        }

    }

}