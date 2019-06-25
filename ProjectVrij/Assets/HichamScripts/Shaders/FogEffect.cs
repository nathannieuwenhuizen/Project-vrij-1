using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FogEffect : MonoBehaviour
{
    public Material _mat;
    public Color _FogStartColor;
    public Color _FogMidColor;
    public Color _FogEndColor;
    [Range(0,1)]public float _MidStart;
    [Range(0,2.5f)] public float _FogIntensity;

    public float _depthStart;
    public float _depthDistance;

    // Use this for initialization
    void Start()
    {
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
    }

    void Update()
    {
        _mat.SetColor("_Color01", _FogStartColor);
        _mat.SetColor("_Color02", _FogMidColor);
        _mat.SetColor("_Color03", _FogEndColor);
        _mat.SetFloat("_MidStart", _MidStart);
        _mat.SetFloat("_DepthStart", _depthStart);
        _mat.SetFloat("_DepthDistance", _depthDistance);
        _mat.SetFloat("_FogIntensity", _FogIntensity);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, _mat);
    }
}
