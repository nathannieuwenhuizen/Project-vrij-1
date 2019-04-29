Shader "Lika/Healthbar" {
    Properties {
        _DeathColor ("Death Color", Color) = (1,0,0,1)
        _HealthColor ("Health Color", Color) = (0.1172414,1,0,1)
        _TimeAmmount ("Health Ammount", Range(1, 0)) = 1
        _Decall ("Decall", 2D) = "white" {}
        _DecalColor ("DecalColor", Color) = (0.9034483,1,0,1)
        _Speed ("Speed", Range(-1, 1)) = 0
        _CircleSize ("Circle Size", Float ) = 0.2
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
 
    }
    SubShader {
        Tags {
            "Queue"="AlphaTest"
            "RenderType"="TransparentCutout"
            "CanUseSpriteAtlas"="True"
            "PreviewType"="Plane"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One OneMinusSrcAlpha
            Cull Off
            
            
            Stencil {
                Ref [_Stencil]
                ReadMask [_StencilReadMask]
                WriteMask [_StencilWriteMask]
                Comp [_StencilComp]
                Pass [_StencilOp]
                Fail [_StencilOpFail]
                ZFail [_StencilOpZFail]
            }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #pragma multi_compile _ PIXELSNAP_ON
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _DeathColor;
            uniform float4 _HealthColor;
            uniform float _TimeAmmount;
            uniform sampler2D _Decall; uniform float4 _Decall_ST;
            uniform float4 _DecalColor;
            uniform float _Speed;
            uniform float _CircleSize;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos( v.vertex );
                #ifdef PIXELSNAP_ON
                    o.pos = UnityPixelSnap(o.pos);
                #endif
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float node_7587_ang = (-3.15);
                float node_7587_spd = 1.0;
                float node_7587_cos = cos(node_7587_spd*node_7587_ang);
                float node_7587_sin = sin(node_7587_spd*node_7587_ang);
                float2 node_7587_piv = float2(0.5,0.5);
                float2 node_7587 = (mul(i.uv0-node_7587_piv,float2x2( node_7587_cos, -node_7587_sin, node_7587_sin, node_7587_cos))+node_7587_piv);
                float2 node_8019 = (node_7587*2.0+-1.0);
                float2 node_584 = node_8019.rg;
                float node_3077 = length(node_8019);
                float node_3702 = ((1.0 - ceil((((atan2(node_584.r,node_584.g)/6.28318530718)+0.5)-_TimeAmmount)))*floor((_CircleSize+node_3077))*(1.0 - floor(node_3077)));
                clip(node_3702 - 0.5);
////// Lighting:
////// Emissive:
                float4 node_2322 = _Time;
                float2 node_1436 = (i.uv0+(node_2322.g*(_Speed*9.0+1.0))*float2(0.1,0.1));
                float4 _Decall_var = tex2D(_Decall,TRANSFORM_TEX(node_1436, _Decall));
                float3 emissive = lerp(_DecalColor.rgb,(lerp(_DeathColor.rgb,_HealthColor.rgb,_TimeAmmount)*node_3702),(1.0 - (node_3702-(1.0 - _Decall_var.rgb))));
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #pragma multi_compile _ PIXELSNAP_ON
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float _TimeAmmount;
            uniform float _CircleSize;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos( v.vertex );
                #ifdef PIXELSNAP_ON
                    o.pos = UnityPixelSnap(o.pos);
                #endif
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float node_7587_ang = (-3.15);
                float node_7587_spd = 1.0;
                float node_7587_cos = cos(node_7587_spd*node_7587_ang);
                float node_7587_sin = sin(node_7587_spd*node_7587_ang);
                float2 node_7587_piv = float2(0.5,0.5);
                float2 node_7587 = (mul(i.uv0-node_7587_piv,float2x2( node_7587_cos, -node_7587_sin, node_7587_sin, node_7587_cos))+node_7587_piv);
                float2 node_8019 = (node_7587*2.0+-1.0);
                float2 node_584 = node_8019.rg;
                float node_3077 = length(node_8019);
                float node_3702 = ((1.0 - ceil((((atan2(node_584.r,node_584.g)/6.28318530718)+0.5)-_TimeAmmount)))*floor((_CircleSize+node_3077))*(1.0 - floor(node_3077)));
                clip(node_3702 - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
