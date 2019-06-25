Shader "Hidden/FogShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Color01("Start Color", Color) = (1,1,1,1)
		_Color02("Mid Color", Color) = (1,1,1,1)
		_Color03("End Color", Color) = (1,1,1,1)
		_MidStart("Mid Color Start", Color) = (1,1,1,1)
		_DepthStart("Depth Start",float) = 1
		_DepthDistance("Depth Distance",float) = 1
		_FogIntensity("Intensity",float) = 1
		_FogOpacityMax("Fog Opacity Max",Range(0,1)) = 1
	}
		SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _CameraDepthTexture;
			fixed4 _FogColor;
			fixed4 _Color01,_Color02,_Color03;
			float _DepthStart, _DepthDistance, _MidStart, _FogIntensity, _FogOpacityMax;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 scrPos :TEXCOORD1;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.scrPos = ComputeScreenPos(o.vertex);
				o.uv = v.uv;
				return o;
			}

			
			sampler2D _MainTex;

			fixed4 frag(v2f i) : COLOR
			{ 
				float depthValue = Linear01Depth(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(i.scrPos)).r) * _ProjectionParams.z;
				depthValue = saturate((depthValue - _DepthStart) / _DepthDistance);
				fixed4 fogColorEnd = _Color01 * depthValue;
				fixed4 fogColorMid = _Color01 * depthValue;
				fixed4 fogColorStr = _Color01 * depthValue;

				fixed4 col = tex2Dproj(_MainTex, i.scrPos);

				fixed4 val1 = lerp(_Color01, _Color02, clamp(depthValue, 0, _MidStart));
				fixed4 val2 = lerp(_Color02, _Color03, clamp(depthValue, _MidStart, 1));

				fixed4 newCol = lerp(val1, val2, depthValue);
		
				return lerp(col, newCol, depthValue *_FogIntensity);
				//return fixed4(depthValue, depthValue, depthValue, 1);
			}
			ENDCG
		}
	}
}