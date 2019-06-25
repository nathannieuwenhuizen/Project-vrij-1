﻿Shader "Hidden/ScreenWave"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_NoiseScale("Noise Scale", float) = 1
		_NoiseFreq("Noise Frequency", float) = 1
		_NoiseSpeed("Noise speed", float) = 1
		_pixelOffset("Pixel Offset",float)= 0.005
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
				#include "noiseSimplex.cginc"
				#define M_PI 3.1415926535897932384626433832795

				uniform float _NoiseScale,_NoiseFreq,_NoiseSpeed,_pixelOffset;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
				float4 scrPos : TEXCOORD1;
            };

            v2f vert (appdata v)
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
				float3 spos = float3(i.scrPos.x , i.scrPos.y,0) * _NoiseFreq;
				spos.z += _Time.x * _NoiseSpeed;
				float noise = _NoiseScale * ((snoise(spos) + 1) / 2);
				float4 noiseToDirection = float4(cos(noise*M_PI * 2), sin(noise * M_PI * 2), 0, 0);

				fixed4 col = tex2Dproj(_MainTex, i.scrPos + normalize(noiseToDirection) * _pixelOffset);
                return col;
            }
            ENDCG
        }
    }
}
