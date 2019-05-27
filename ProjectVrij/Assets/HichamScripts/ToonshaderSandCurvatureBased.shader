Shader "Custom/ToonShaderSand + Ambient Occlusion"  {
	Properties{
	[Header(Input Control)]
	_Curva("Ambient Occlusion",2D) = "white" {}
	_low("Brightness",Range(0,2)) = 0
	_high("Custom strenght",Range(1,0)) = 1

	[Header(PBR Properties)]
	_Color("Main Color",Color) = (0.5,0.5,0.5,1)  //main color mix with maintexture
	_MainTex("Base (RGB)",2D) = "white" {}
	_NormalTex("Normal Map",2D) = "normal" {}
	_Metallic("Metallic/Smoothness",2D) = "black" {}
	[HDR] _Emissive("Emissive",2D) = "black" {}
	_EmisCol("Emissive Color",Color) = (0,0,0,0)
	[Space(20)]
	[Header(Sand Properties)]
	_SandColor("Sand Base Color",Color) = (0.5,0.4,0.1,1)
	_TPColor("Sand Top Color", Color) = (0.5,0.5,0.5,1)
	_RimColor("Sand Rim Color", Color) = (0.5,0.5,0.5,1)
	_SandRamp("Sand Ramp (RGB)", 2D) = "gray" {}
	_SandSize("Sand Amount", Range(-2,2)) = 1
	_RimPower("Rim Power", Range(0,4)) = 3
	_SandSmoothness("Sand Smoothness",Range(0,1)) = 0
	_Height("Sand Height", Range(0,4)) = 0.1
	[Space(30)]
	_SandNormal("Sand Normal",2D) = "normal" {}
	_SandAngle("Angle Sand Buildup", Vector) = (0,1,0)
	}
		SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		Cull Off
		CGPROGRAM

		#pragma surface surf Standard fullforwardshadows vertex:vert addshadow
		#pragma target 4.0

	sampler2D _Metallic, _Curva, _Emissive;
	sampler2D _NormalTex, _SandNormal;
	sampler2D _MainTex, _SandRamp;
	float4 _Color, _SandColor, _TPColor, _SandAngle, _RimColor, _EmisCol;
	half _SandSize, _Height, _RimPower;
	half _high, _low, _SandSmoothness;

	struct Input {
		float2 uv_MainTex : TEXCOORD0;
		float3 worldPos;
		float3 viewDir;
		float3 lightDir;
		float3 secondaryNormal : TEXCOORD4;
		float3 worldNormal; INTERNAL_DATA
	};

	struct appdata {
		float4 vertex : POSITION;
		float3 normal : NORMAL;
	};

	void vert(inout appdata_full v, out Input o)
	{
		UNITY_INITIALIZE_OUTPUT(Input, o);
		o.lightDir = WorldSpaceLightDir(v.vertex); //light direction for the Sand
		float4 sandC = mul(_SandAngle, unity_ObjectToWorld);  //sand direction to WorldSpace

		half tex = tex2Dlod(_Curva, float4(v.texcoord.xy, 0, 0)).r;
		half ca = ((tex - _low) >= _high) * -1;

		if (lerp(0, 1, dot(v.normal, sandC.xyz) >= _SandSize - 0.1)) {
			v.vertex.y += v.normal.y * (_Height);
		}
		o.secondaryNormal = mul(unity_ObjectToWorld, v.normal);
	}

	void surf(Input IN, inout SurfaceOutputStandard  o)
	{
		float3 localPos = (IN.worldPos - mul(unity_ObjectToWorld, float4(0, 0, 0, 1)).xyz); //  position sandblend
		half d = dot(o.Normal, IN.lightDir) *0.5 + 0.5;

		half4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		half3 rampS = tex2D(_SandRamp, float2(d,d)).rgb;
		o.Albedo = c.rgb * _Color;

		half Curva = 1 * (tex2D(_Curva, IN.uv_MainTex).r);
		half ca = ((Curva / _low) >= _high) * -1;
		half3 myNormal = normalize(IN.secondaryNormal.xyz);
		half Rim = 1.0 - saturate(dot(normalize(IN.viewDir), myNormal));

		o.Normal = UnpackNormal(tex2D(_NormalTex, IN.uv_MainTex));
		o.Metallic = tex2D(_Metallic, IN.uv_MainTex).rgb;
		o.Smoothness = tex2D(_Metallic, IN.uv_MainTex).a;
		o.Emission = tex2D(_Emissive, IN.uv_MainTex) * _EmisCol;
		if (lerp(0,ca, dot(myNormal, _SandAngle.xyz) >= _SandSize))
		{
			o.Albedo = (lerp(_SandColor * rampS, _TPColor * rampS, saturate(localPos.y)));
			o.Emission = _RimColor * pow(Rim, _RimPower);
			o.Normal = UnpackNormal(tex2D(_SandNormal, IN.uv_MainTex));
			o.Smoothness = _SandSmoothness;
		}
		o.Alpha = c.a;
		}
		ENDCG
	}
		FallBack "Diffuse"
}
