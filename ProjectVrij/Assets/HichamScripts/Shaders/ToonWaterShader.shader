
Shader "Custom/ToonWater"
{
	Properties
	{
		[Toggle(_DO_STUFF_ON)]
		_SmoothOn("Smooth Coloring", Float) = 0

		[Header(Color Settings)]
		_SurfaceColor("Surface Water Color", Color) = (0.3,0.8,0.9,0.7)
		_DeepColor("Depth Water Color",Color) = (0.086, 0.407, 1, 0.749)
		_FoamColor("Foam Color", Color) = (1,1,1,1)
		_WaveCol("Wave Color", Color) = (1,1,1,1)

		[Header(Water Settings)]
		_DepthMaxDistance("Depth Maximum Distance", Float) = 1
		_SurfaceNoise("Surface Noise", 2D) = "white" {}
		_SurfaceNoiseCutoff("Surface Noise Cutoff", Range(0, 1)) = 0.777
		_FoamMaxDistance("Foam Maximum Distance", Float) = 0.4
		_FoamMinDistance("Foam Minimum Distance", Float) = 0.04
		[Space(20)]
		[Header(Distortion Settings)]
		_SurfaceDistortionAmount("Surface Distortion Amount", Range(0, 1)) = 0.27
		_SurfaceDistortion("Surface Distortion", 2D) = "white" {}
		_SurfaceNoiseScroll("Surface Noise Scroll Amount", Vector) = (0.03, 0.03, 0, 0)

	
		[Space(20)]
		[Header(Water Settings)]
		_Speed("Wave Speed", Range(0,1)) = 0.5
		_Amount("Wave Amount", Range(0,50)) = 0.5
		_Height("Wave Height", Range(0,1)) = 0.5
		_topWave ("Wave Color Cutoff", Range(0,1)) = 0.777
		
		_Min("Brightnes (Smooth only)",Float) = 0.777
			
	}
		SubShader
	{
		Tags
		{	"Queue" = "Transparent"
			"LightMode" = "ForwardBase"
			"PassFlags" = "OnlyDirectional"
			}
		Pass 
		{
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off

			CGPROGRAM
			#pragma shader_feature _DO_STUFF_OFF _DO_STUFF_ON
			#define SMOOTHSTEP_AA 0.01
		
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			float4 _SurfaceColor,_DeepColor,_FoamColor,_WaveCol;
			float2 _SurfaceNoiseScroll;
			float _DepthMaxDistance, _SurfaceNoiseCutoff;
			float _FoamMaxDistance, _FoamMinDistance;
			float _Speed, _Amount, _Height, _topWave, _Min;


			sampler2D _CameraDepthTexture;
			sampler2D _CameraNormalsTexture;

			float4 alphaBlend(float4 top, float4 bottom)
			{
				float3 color = (top.rgb * top.a) + (bottom.rgb * (1 - top.a));
				float alpha = top.a + bottom.a * (1 - top.a);
				return float4(color, alpha);
			}


			struct appdata
			{
				float4 vertex : POSITION;
				float4 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			sampler2D _SurfaceNoise, _SurfaceDistortion, _NoiseTex;
			float4 _SurfaceNoise_ST, _SurfaceDistortion_ST;
			float _SurfaceDistortionAmount;

			struct v2f
			{
				float2 noiseUV : TEXCOORD0;
				float2 distortUV : TEXCOORD1;
				float4 screenPosition : TEXCOORD2;
				float3 viewNormal : NORMAL;
				float4 vertex : SV_POSITION;
				float3 localVPos: TEXCOORD3;
			};


			v2f vert(appdata v)
			{
				v2f o;
				float4 tex = tex2Dlod(_NoiseTex, float4(v.uv.xy, 0, 0));//extra noise tex
				v.vertex.y += sin(_Time.z * _Speed + (v.vertex.x * v.vertex.z * _Amount * tex)) * _Height;//movement
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.screenPosition = ComputeScreenPos(o.vertex);
				o.viewNormal = COMPUTE_VIEW_NORMAL;
				o.noiseUV = TRANSFORM_TEX(v.uv, _SurfaceNoise);
				o.distortUV = TRANSFORM_TEX(v.uv, _SurfaceDistortion);
				float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
				//o.worldpos = worldPos;
				o.localVPos = v.vertex;

				return o;
			}


			float4 frag(v2f i) : SV_Target
			{
			float eDepth = tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(i.screenPosition)).r;
			float eDepthL = LinearEyeDepth(eDepth);
			float depthDiff = eDepthL - i.screenPosition.w;

			float waterDepthDiff1 = saturate(depthDiff / _DepthMaxDistance);
			float4 waterColor = lerp(_SurfaceColor, _DeepColor, waterDepthDiff1);
	
float2 distortSample = (tex2D(_SurfaceDistortion, i.distortUV).xy * 2 - 1) * _SurfaceDistortionAmount;
			float2 noiseUV = float2((i.noiseUV.x + _Time.y * _SurfaceNoiseScroll.x) + distortSample.x, (i.noiseUV.y + _Time.y * _SurfaceNoiseScroll.y) + distortSample.y);
			float surfaceNoiseSample = tex2D(_SurfaceNoise, noiseUV).r;

			float3 existingNormal = tex2Dproj(_CameraNormalsTexture, UNITY_PROJ_COORD(i.screenPosition));
			float3 normalDot = saturate(dot(existingNormal, i.viewNormal));

			float foamDistance = lerp(_FoamMaxDistance / 20, _FoamMinDistance / 20, normalDot);
			float foamDepthDifference01 = saturate(depthDiff / foamDistance);
			float foamWave = i.vertex;

			#if defined(_DO_STUFF_ON)
						
						float surfaceNoise = saturate(surfaceNoiseSample - _SurfaceNoiseCutoff);

						float4 topFoamCut = (smoothstep(_topWave, 1, i.localVPos.y))* _Min;
					
					
			#else
						float surfaceNoiseCutoff = foamDepthDifference01 * _SurfaceNoiseCutoff;
						float surfaceNoise = smoothstep(surfaceNoiseCutoff - SMOOTHSTEP_AA, surfaceNoiseCutoff + SMOOTHSTEP_AA, surfaceNoiseSample);
						float topFoamCut = i.localVPos.y > (_topWave * 0.34) ? 1 : 0;
			#endif
	
			float4 surfaceNoiseColor = _FoamColor;
			surfaceNoiseColor.a *= surfaceNoise;
			float4 WaterEnd = lerp(surfaceNoiseColor, _WaveCol, topFoamCut);
			
			return alphaBlend(WaterEnd, waterColor);
			//return float4(i.worldNormal.x, i.worldNormal.y, i.worldNormal.z,1);
			}
			ENDCG
		}
	}
}