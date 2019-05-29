Shader "Custom/ PBR Shader"  {
	Properties {

	_Color("Main Color",Color) = (0.5,0.5,0.5,1)  //main color mix with maintexture
	_MainTex("Base (RGB)",2D )= "white" {}
	_NormalTex("Normal Map",2D) = "normal" {}
	_ToonRamp("Ramp", 2D) = "white" {}
	_Metallic("Metallic/Smoothness",2D) = "black" {}
	}

		SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		Cull Off
		CGPROGRAM

		#pragma surface surf Standard fullforwardshadows vertex:vert addshadow
		#pragma target 4.0

		 sampler2D _ToonRamp;

	// custom lighting function that uses a texture ramp based
	// on angle between light direction and normal
	#pragma lighting  ToonRamp exclude_path:prepass	
	inline half4 LightingToonRamp(SurfaceOutput s, half3 lightDir, half atten)
	{
	#ifndef USING_DIRECTIONAL_LIGHT
		lightDir = normalize(lightDir);
	#endif

		half d = dot(s.Normal, lightDir) *0.5 + 0.5;
		half3 ramp = tex2D(_ToonRamp, float2(d, d)).rgb;

		half4 c;
		c.rgb = s.Albedo * _LightColor0.rgb * ramp *(atten * 2);
		c.a = 0;
		return c;
	}

	sampler2D _Metallic, _Curva;
	sampler2D _NormalTex, _SandNormal;
	sampler2D _MainTex;
	float4 _Color, _SandColor, _TPColor, _SandAngle, _RimColor;
	float _SandSize, _Height, _RimPower;
	float _high, _low;
	half _SandSmoothness;
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

	
	}

	void surf(Input IN, inout SurfaceOutputStandard  o)
	{
		float3 localPos = (IN.worldPos - mul(unity_ObjectToWorld, float4(0, 0, 0, 1)).xyz); //  position sand blend
		half d = dot(o.Normal, IN.lightDir) *0.5 + 0.5;
		half4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		half3 rampS = tex2D(_ToonRamp, float2(d,d)).rgb;

		o.Albedo = c.rgb * _Color;
		//o.Albedo = IN.worldNormal;
		o.Normal = UnpackNormal(tex2D(_NormalTex, IN.uv_MainTex));
		o.Alpha = c.a;
		
		o.Metallic =  tex2D(_Metallic, IN.uv_MainTex).rgb;
		o.Smoothness = tex2D(_Metallic, IN.uv_MainTex).a;
		} 
		ENDCG
	} 
	FallBack "Diffuse"
}
