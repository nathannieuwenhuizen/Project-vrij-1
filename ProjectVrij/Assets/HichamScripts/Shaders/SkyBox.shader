Shader "Skybox/Gradient Skybox"
{
	Properties
	{
		_Color2("High Color", Color) = (1, 1, 1, 0)
		_Color1("Lower Color", Color) = (1, 1, 1, 0)
		[HideInInspector] _UpVector("Up Vector", Vector) = (0, 1, 0, 0)
		_Intensity("Intensity", Float) = 1.0
		_Exponent("Exponent", Range(-0,1)) = 0.5

		_Clouds("Cloud Texture",2D) = "black"{}
		_CLCol("Cloud Color", Color) = (1,1,1,0)
		_CloudIntensity("Cloud Intensity",Range(0,5)) = 2
		_CloudSpeed("Cloud Speed",Float) = 0.15

		_SunColor("Sun Color", Color) = (1, 0.99, 0.87, 1)
		_SunIntensity("Sun Intensity",Range(-20,20)) = 20
		_SunAlpha("Sun Alpha", Range(0.2,500)) = 250
		_SunBeta("Sun Beta", Range(0.1,500)) = 130
		_SunVector("Sun Vector", Vector) = (0.02, 0.65, 0.76, 0)

	}

		CGINCLUDE

#include "UnityCG.cginc"

			sampler2D _Clouds;

			struct appdata
		{
			float4 position : POSITION;
			float3 texcoord : TEXCOORD0;
			float2 uv : TEXCOORD1;
		};

		struct v2f
		{
			float2 uv : TEXCOORD1;
			float4 position : SV_POSITION;
			float3 texcoord : TEXCOORD0;
		};

		float _CloudSpeed;
		float3 _CLCol;
		half3 _Color1, _Color2, _UpVector;
		half _Exponent, _Intensity, _CloudIntensity;
		half3 _SunColor, _SunVector;
		half _SunAlpha, _SunIntensity, _SunBeta;

	

		v2f vert(appdata v)
		{
			v2f o;
			o.position = UnityObjectToClipPos(v.position);
			o.texcoord = v.texcoord;
			o.uv = v.uv;
			return o;
		}

		fixed4 frag(v2f i) : COLOR
		{
		float3 v = normalize(i.texcoord);

		half d = dot(v, _UpVector) * 0.5f + 0.5f;

		i.uv.x += (_Time * _CloudSpeed);

		half cloud = (tex2D(_Clouds, i.uv).r * _Exponent) * _CloudIntensity;

		half3 Sun = _SunColor * min(pow(max(0, dot(v, _SunVector)), (_SunAlpha * 100)) * _SunBeta, 1);

		half3 Skybox = lerp((lerp(_Color1, _Color2, pow(d, _Exponent)) * _Intensity),_CLCol,cloud);

		return half4(Skybox * 1 + Sun * _SunIntensity, 0);
	//	return fixed4(cloud, 1);
		}

			ENDCG

			SubShader
		{
			Tags{ "RenderType" = "Background" "Queue" = "Background" }
				Pass
			{
				ZWrite Off
				Cull Off
				Fog { Mode Off }
				CGPROGRAM
				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma vertex vert
				#pragma fragment frag
				ENDCG
			}
		}
}