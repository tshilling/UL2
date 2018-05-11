// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:3,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:True,hqlp:False,rprd:True,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:2865,x:32719,y:32712,varname:node_2865,prsc:2|diff-8565-OUT,spec-358-OUT,gloss-1813-OUT,normal-3743-OUT,alpha-5839-OUT,voffset-9471-OUT;n:type:ShaderForge.SFN_Color,id:6665,x:31466,y:32404,ptovrint:False,ptlb:Color,ptin:_Color,varname:_Color,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5019608,c2:0.5019608,c3:0.5019608,c4:1;n:type:ShaderForge.SFN_Slider,id:358,x:32135,y:32763,ptovrint:False,ptlb:Metallic,ptin:_Metallic,varname:node_358,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Slider,id:1813,x:32135,y:32860,ptovrint:False,ptlb:Gloss,ptin:_Gloss,varname:_Metallic_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.8,max:1;n:type:ShaderForge.SFN_DepthBlend,id:5839,x:32461,y:32970,varname:node_5839,prsc:2;n:type:ShaderForge.SFN_Append,id:9471,x:31921,y:33155,varname:node_9471,prsc:2|A-4769-OUT,B-507-OUT,C-6313-OUT;n:type:ShaderForge.SFN_Set,id:6587,x:31393,y:33222,varname:XValue,prsc:2|IN-1252-OUT;n:type:ShaderForge.SFN_Get,id:4769,x:31735,y:33155,varname:node_4769,prsc:2|IN-6587-OUT;n:type:ShaderForge.SFN_Set,id:4836,x:30910,y:33860,varname:YValue,prsc:2|IN-4729-OUT;n:type:ShaderForge.SFN_Get,id:507,x:31735,y:33202,varname:node_507,prsc:2|IN-4836-OUT;n:type:ShaderForge.SFN_Set,id:2176,x:31393,y:33275,varname:ZValue,prsc:2|IN-3733-OUT;n:type:ShaderForge.SFN_Get,id:6313,x:31735,y:33256,varname:node_6313,prsc:2|IN-2176-OUT;n:type:ShaderForge.SFN_Vector1,id:1252,x:31243,y:33222,varname:node_1252,prsc:2,v1:0;n:type:ShaderForge.SFN_Vector1,id:3733,x:31243,y:33275,varname:node_3733,prsc:2,v1:0;n:type:ShaderForge.SFN_Vector1,id:3998,x:29189,y:33463,varname:node_3998,prsc:2,v1:0.05;n:type:ShaderForge.SFN_Set,id:3755,x:29343,y:33463,varname:Step,prsc:2|IN-3998-OUT;n:type:ShaderForge.SFN_Append,id:9064,x:31456,y:33772,varname:node_9064,prsc:2|A-9646-OUT,B-3553-OUT,C-9503-OUT;n:type:ShaderForge.SFN_Append,id:7557,x:31446,y:34059,varname:node_7557,prsc:2|A-9503-OUT,B-3430-OUT,C-9646-OUT;n:type:ShaderForge.SFN_Get,id:9646,x:31238,y:34234,varname:node_9646,prsc:2|IN-3755-OUT;n:type:ShaderForge.SFN_Vector1,id:9503,x:31177,y:33914,varname:node_9503,prsc:2,v1:0;n:type:ShaderForge.SFN_Cross,id:110,x:32317,y:33936,varname:node_110,prsc:2|A-7557-OUT,B-9064-OUT;n:type:ShaderForge.SFN_Set,id:2709,x:32791,y:33947,varname:Normal,prsc:2|IN-8632-OUT;n:type:ShaderForge.SFN_Get,id:3743,x:32461,y:32913,varname:node_3743,prsc:2|IN-2709-OUT;n:type:ShaderForge.SFN_Multiply,id:6197,x:30234,y:33682,varname:node_6197,prsc:2|A-6957-OUT,B-4720-OUT;n:type:ShaderForge.SFN_ValueProperty,id:4720,x:30069,y:33874,ptovrint:False,ptlb:Wave Height,ptin:_WaveHeight,varname:node_4720,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.1;n:type:ShaderForge.SFN_Subtract,id:3553,x:30931,y:33628,varname:node_3553,prsc:2|A-8566-OUT,B-4729-OUT;n:type:ShaderForge.SFN_Subtract,id:3430,x:30931,y:34049,varname:node_3430,prsc:2|A-857-OUT,B-4729-OUT;n:type:ShaderForge.SFN_Time,id:4562,x:29214,y:33547,varname:node_4562,prsc:2;n:type:ShaderForge.SFN_FragmentPosition,id:4406,x:29214,y:33682,varname:node_4406,prsc:2;n:type:ShaderForge.SFN_Add,id:3048,x:29540,y:33674,varname:node_3048,prsc:2|A-4562-T,B-4406-X;n:type:ShaderForge.SFN_Add,id:7743,x:29543,y:33983,varname:node_7743,prsc:2|A-4562-T,B-4406-Z;n:type:ShaderForge.SFN_Add,id:4897,x:29712,y:34190,varname:node_4897,prsc:2|A-7743-OUT,B-5347-OUT;n:type:ShaderForge.SFN_Get,id:5347,x:29196,y:34001,varname:node_5347,prsc:2|IN-3755-OUT;n:type:ShaderForge.SFN_Append,id:8451,x:29881,y:33682,varname:node_8451,prsc:2|A-3048-OUT,B-1039-OUT;n:type:ShaderForge.SFN_Sin,id:6957,x:30064,y:33682,varname:node_6957,prsc:2|IN-8451-OUT;n:type:ShaderForge.SFN_Cos,id:6191,x:30069,y:33995,varname:node_6191,prsc:2|IN-3121-OUT;n:type:ShaderForge.SFN_Append,id:3121,x:29885,y:33995,varname:node_3121,prsc:2|A-7743-OUT,B-4897-OUT;n:type:ShaderForge.SFN_Add,id:1039,x:29717,y:33830,varname:node_1039,prsc:2|A-3048-OUT,B-5347-OUT;n:type:ShaderForge.SFN_Dot,id:7274,x:30421,y:33779,varname:node_7274,prsc:2,dt:0|A-6197-OUT,B-1308-OUT;n:type:ShaderForge.SFN_Multiply,id:8399,x:30240,y:33995,varname:node_8399,prsc:2|A-6191-OUT,B-4720-OUT;n:type:ShaderForge.SFN_Vector2,id:1308,x:30240,y:33843,varname:node_1308,prsc:2,v1:1,v2:0;n:type:ShaderForge.SFN_Dot,id:7148,x:30421,y:33924,varname:node_7148,prsc:2,dt:0|A-8399-OUT,B-1308-OUT;n:type:ShaderForge.SFN_Add,id:4729,x:30619,y:33859,varname:node_4729,prsc:2|A-7274-OUT,B-7148-OUT;n:type:ShaderForge.SFN_Dot,id:5625,x:30421,y:33625,varname:node_5625,prsc:2,dt:0|A-6197-OUT,B-8111-OUT;n:type:ShaderForge.SFN_Vector2,id:8111,x:30217,y:33538,varname:node_8111,prsc:2,v1:0,v2:1;n:type:ShaderForge.SFN_Dot,id:3839,x:30421,y:34088,varname:node_3839,prsc:2,dt:0|A-8399-OUT,B-9360-OUT;n:type:ShaderForge.SFN_Vector2,id:9360,x:30240,y:34116,varname:node_9360,prsc:2,v1:0,v2:1;n:type:ShaderForge.SFN_Add,id:8566,x:30619,y:33625,varname:node_8566,prsc:2|A-5625-OUT,B-7148-OUT;n:type:ShaderForge.SFN_Add,id:857,x:30619,y:34050,varname:node_857,prsc:2|A-7274-OUT,B-3839-OUT;n:type:ShaderForge.SFN_Add,id:8565,x:32023,y:32300,varname:node_8565,prsc:2|A-3076-OUT,B-6665-RGB;n:type:ShaderForge.SFN_Fresnel,id:3076,x:31476,y:32102,varname:node_3076,prsc:2|NRM-2151-OUT,EXP-4708-OUT;n:type:ShaderForge.SFN_Get,id:2151,x:31217,y:32102,varname:node_2151,prsc:2|IN-2709-OUT;n:type:ShaderForge.SFN_Normalize,id:8632,x:32565,y:33788,varname:node_8632,prsc:2|IN-110-OUT;n:type:ShaderForge.SFN_ValueProperty,id:4708,x:31256,y:32183,ptovrint:False,ptlb:node_4708,ptin:_node_4708,varname:node_4708,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:10;proporder:6665-358-1813-4720-4708;pass:END;sub:END;*/

Shader "Shader Forge/Water7" {
    Properties {
        _Color ("Color", Color) = (0.5019608,0.5019608,0.5019608,1)
        _Metallic ("Metallic", Range(0, 1)) = 0
        _Gloss ("Gloss", Range(0, 1)) = 0.8
        _WaveHeight ("Wave Height", Float ) = 0.1
        _node_4708 ("node_4708", Float ) = 10
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _CameraDepthTexture;
            uniform float4 _Color;
            uniform float _Metallic;
            uniform float _Gloss;
            uniform float _WaveHeight;
            uniform float _node_4708;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv1 : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float4 posWorld : TEXCOORD2;
                float3 normalDir : TEXCOORD3;
                float3 tangentDir : TEXCOORD4;
                float3 bitangentDir : TEXCOORD5;
                float4 projPos : TEXCOORD6;
                UNITY_FOG_COORDS(7)
                #if defined(LIGHTMAP_ON) || defined(UNITY_SHOULD_SAMPLE_SH)
                    float4 ambientOrLightmapUV : TEXCOORD8;
                #endif
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                #ifdef LIGHTMAP_ON
                    o.ambientOrLightmapUV.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
                    o.ambientOrLightmapUV.zw = 0;
                #elif UNITY_SHOULD_SAMPLE_SH
                #endif
                #ifdef DYNAMICLIGHTMAP_ON
                    o.ambientOrLightmapUV.zw = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
                #endif
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                float XValue = 0.0;
                float4 node_4562 = _Time;
                float node_3048 = (node_4562.g+mul(unity_ObjectToWorld, v.vertex).r);
                float node_3998 = 0.05;
                float Step = node_3998;
                float node_5347 = Step;
                float2 node_6957 = sin(float2(node_3048,(node_3048+node_5347)));
                float2 node_6197 = (node_6957*_WaveHeight);
                float2 node_1308 = float2(1,0);
                float node_7274 = dot(node_6197,node_1308);
                float node_7743 = (node_4562.g+mul(unity_ObjectToWorld, v.vertex).b);
                float2 node_6191 = cos(float2(node_7743,(node_7743+node_5347)));
                float2 node_8399 = (node_6191*_WaveHeight);
                float node_7148 = dot(node_8399,node_1308);
                float node_4729 = (node_7274+node_7148);
                float YValue = node_4729;
                float ZValue = 0.0;
                v.vertex.xyz += float3(XValue,YValue,ZValue);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float node_9503 = 0.0;
                float4 node_4562 = _Time;
                float node_3048 = (node_4562.g+i.posWorld.r);
                float node_3998 = 0.05;
                float Step = node_3998;
                float node_5347 = Step;
                float2 node_6957 = sin(float2(node_3048,(node_3048+node_5347)));
                float2 node_6197 = (node_6957*_WaveHeight);
                float2 node_1308 = float2(1,0);
                float node_7274 = dot(node_6197,node_1308);
                float node_7743 = (node_4562.g+i.posWorld.b);
                float2 node_6191 = cos(float2(node_7743,(node_7743+node_5347)));
                float2 node_8399 = (node_6191*_WaveHeight);
                float node_7148 = dot(node_8399,node_1308);
                float node_4729 = (node_7274+node_7148);
                float node_9646 = Step;
                float3 node_9064 = float3(node_9646,((dot(node_6197,float2(0,1))+node_7148)-node_4729),node_9503);
                float3 Normal = normalize(cross(float3(node_9503,((node_7274+dot(node_8399,float2(0,1)))-node_4729),node_9646),node_9064));
                float3 normalLocal = Normal;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float sceneZ = max(0,LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)))) - _ProjectionParams.g);
                float partZ = max(0,i.projPos.z - _ProjectionParams.g);
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = 1;
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float gloss = _Gloss;
                float perceptualRoughness = 1.0 - _Gloss;
                float roughness = perceptualRoughness * perceptualRoughness;
                float specPow = exp2( gloss * 10.0 + 1.0 );
/////// GI Data:
                UnityLight light;
                #ifdef LIGHTMAP_OFF
                    light.color = lightColor;
                    light.dir = lightDirection;
                    light.ndotl = LambertTerm (normalDirection, light.dir);
                #else
                    light.color = half3(0.f, 0.f, 0.f);
                    light.ndotl = 0.0f;
                    light.dir = half3(0.f, 0.f, 0.f);
                #endif
                UnityGIInput d;
                d.light = light;
                d.worldPos = i.posWorld.xyz;
                d.worldViewDir = viewDirection;
                d.atten = attenuation;
                #if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
                    d.ambient = 0;
                    d.lightmapUV = i.ambientOrLightmapUV;
                #else
                    d.ambient = i.ambientOrLightmapUV;
                #endif
                #if UNITY_SPECCUBE_BLENDING || UNITY_SPECCUBE_BOX_PROJECTION
                    d.boxMin[0] = unity_SpecCube0_BoxMin;
                    d.boxMin[1] = unity_SpecCube1_BoxMin;
                #endif
                #if UNITY_SPECCUBE_BOX_PROJECTION
                    d.boxMax[0] = unity_SpecCube0_BoxMax;
                    d.boxMax[1] = unity_SpecCube1_BoxMax;
                    d.probePosition[0] = unity_SpecCube0_ProbePosition;
                    d.probePosition[1] = unity_SpecCube1_ProbePosition;
                #endif
                d.probeHDR[0] = unity_SpecCube0_HDR;
                d.probeHDR[1] = unity_SpecCube1_HDR;
                Unity_GlossyEnvironmentData ugls_en_data;
                ugls_en_data.roughness = 1.0 - gloss;
                ugls_en_data.reflUVW = viewReflectDirection;
                UnityGI gi = UnityGlobalIllumination(d, 1, normalDirection, ugls_en_data );
                lightDirection = gi.light.dir;
                lightColor = gi.light.color;
////// Specular:
                float NdotL = saturate(dot( normalDirection, lightDirection ));
                float LdotH = saturate(dot(lightDirection, halfDirection));
                float3 specularColor = _Metallic;
                float specularMonochrome;
                float3 diffuseColor = (pow(1.0-max(0,dot(Normal, viewDirection)),_node_4708)+_Color.rgb); // Need this for specular when using metallic
                diffuseColor = DiffuseAndSpecularFromMetallic( diffuseColor, specularColor, specularColor, specularMonochrome );
                specularMonochrome = 1.0-specularMonochrome;
                float NdotV = abs(dot( normalDirection, viewDirection ));
                float NdotH = saturate(dot( normalDirection, halfDirection ));
                float VdotH = saturate(dot( viewDirection, halfDirection ));
                float visTerm = SmithJointGGXVisibilityTerm( NdotL, NdotV, roughness );
                float normTerm = GGXTerm(NdotH, roughness);
                float specularPBL = (visTerm*normTerm) * UNITY_PI;
                #ifdef UNITY_COLORSPACE_GAMMA
                    specularPBL = sqrt(max(1e-4h, specularPBL));
                #endif
                specularPBL = max(0, specularPBL * NdotL);
                #if defined(_SPECULARHIGHLIGHTS_OFF)
                    specularPBL = 0.0;
                #endif
                half surfaceReduction;
                #ifdef UNITY_COLORSPACE_GAMMA
                    surfaceReduction = 1.0-0.28*roughness*perceptualRoughness;
                #else
                    surfaceReduction = 1.0/(roughness*roughness + 1.0);
                #endif
                specularPBL *= any(specularColor) ? 1.0 : 0.0;
                float3 directSpecular = attenColor*specularPBL*FresnelTerm(specularColor, LdotH);
                half grazingTerm = saturate( gloss + specularMonochrome );
                float3 indirectSpecular = (gi.indirect.specular);
                indirectSpecular *= FresnelLerp (specularColor, grazingTerm, NdotV);
                indirectSpecular *= surfaceReduction;
                float3 specular = (directSpecular + indirectSpecular);
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
                float nlPow5 = Pow5(1-NdotL);
                float nvPow5 = Pow5(1-NdotV);
                float3 directDiffuse = ((1 +(fd90 - 1)*nlPow5) * (1 + (fd90 - 1)*nvPow5) * NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += gi.indirect.diffuse;
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                fixed4 finalRGBA = fixed4(finalColor,saturate((sceneZ-partZ)));
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdadd
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _CameraDepthTexture;
            uniform float4 _Color;
            uniform float _Metallic;
            uniform float _Gloss;
            uniform float _WaveHeight;
            uniform float _node_4708;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv1 : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float4 posWorld : TEXCOORD2;
                float3 normalDir : TEXCOORD3;
                float3 tangentDir : TEXCOORD4;
                float3 bitangentDir : TEXCOORD5;
                float4 projPos : TEXCOORD6;
                LIGHTING_COORDS(7,8)
                UNITY_FOG_COORDS(9)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                float XValue = 0.0;
                float4 node_4562 = _Time;
                float node_3048 = (node_4562.g+mul(unity_ObjectToWorld, v.vertex).r);
                float node_3998 = 0.05;
                float Step = node_3998;
                float node_5347 = Step;
                float2 node_6957 = sin(float2(node_3048,(node_3048+node_5347)));
                float2 node_6197 = (node_6957*_WaveHeight);
                float2 node_1308 = float2(1,0);
                float node_7274 = dot(node_6197,node_1308);
                float node_7743 = (node_4562.g+mul(unity_ObjectToWorld, v.vertex).b);
                float2 node_6191 = cos(float2(node_7743,(node_7743+node_5347)));
                float2 node_8399 = (node_6191*_WaveHeight);
                float node_7148 = dot(node_8399,node_1308);
                float node_4729 = (node_7274+node_7148);
                float YValue = node_4729;
                float ZValue = 0.0;
                v.vertex.xyz += float3(XValue,YValue,ZValue);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float node_9503 = 0.0;
                float4 node_4562 = _Time;
                float node_3048 = (node_4562.g+i.posWorld.r);
                float node_3998 = 0.05;
                float Step = node_3998;
                float node_5347 = Step;
                float2 node_6957 = sin(float2(node_3048,(node_3048+node_5347)));
                float2 node_6197 = (node_6957*_WaveHeight);
                float2 node_1308 = float2(1,0);
                float node_7274 = dot(node_6197,node_1308);
                float node_7743 = (node_4562.g+i.posWorld.b);
                float2 node_6191 = cos(float2(node_7743,(node_7743+node_5347)));
                float2 node_8399 = (node_6191*_WaveHeight);
                float node_7148 = dot(node_8399,node_1308);
                float node_4729 = (node_7274+node_7148);
                float node_9646 = Step;
                float3 node_9064 = float3(node_9646,((dot(node_6197,float2(0,1))+node_7148)-node_4729),node_9503);
                float3 Normal = normalize(cross(float3(node_9503,((node_7274+dot(node_8399,float2(0,1)))-node_4729),node_9646),node_9064));
                float3 normalLocal = Normal;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float sceneZ = max(0,LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)))) - _ProjectionParams.g);
                float partZ = max(0,i.projPos.z - _ProjectionParams.g);
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                UNITY_LIGHT_ATTENUATION(attenuation,i, i.posWorld.xyz);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float gloss = _Gloss;
                float perceptualRoughness = 1.0 - _Gloss;
                float roughness = perceptualRoughness * perceptualRoughness;
                float specPow = exp2( gloss * 10.0 + 1.0 );
////// Specular:
                float NdotL = saturate(dot( normalDirection, lightDirection ));
                float LdotH = saturate(dot(lightDirection, halfDirection));
                float3 specularColor = _Metallic;
                float specularMonochrome;
                float3 diffuseColor = (pow(1.0-max(0,dot(Normal, viewDirection)),_node_4708)+_Color.rgb); // Need this for specular when using metallic
                diffuseColor = DiffuseAndSpecularFromMetallic( diffuseColor, specularColor, specularColor, specularMonochrome );
                specularMonochrome = 1.0-specularMonochrome;
                float NdotV = abs(dot( normalDirection, viewDirection ));
                float NdotH = saturate(dot( normalDirection, halfDirection ));
                float VdotH = saturate(dot( viewDirection, halfDirection ));
                float visTerm = SmithJointGGXVisibilityTerm( NdotL, NdotV, roughness );
                float normTerm = GGXTerm(NdotH, roughness);
                float specularPBL = (visTerm*normTerm) * UNITY_PI;
                #ifdef UNITY_COLORSPACE_GAMMA
                    specularPBL = sqrt(max(1e-4h, specularPBL));
                #endif
                specularPBL = max(0, specularPBL * NdotL);
                #if defined(_SPECULARHIGHLIGHTS_OFF)
                    specularPBL = 0.0;
                #endif
                specularPBL *= any(specularColor) ? 1.0 : 0.0;
                float3 directSpecular = attenColor*specularPBL*FresnelTerm(specularColor, LdotH);
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
                float nlPow5 = Pow5(1-NdotL);
                float nvPow5 = Pow5(1-NdotV);
                float3 directDiffuse = ((1 +(fd90 - 1)*nlPow5) * (1 + (fd90 - 1)*nvPow5) * NdotL) * attenColor;
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                fixed4 finalRGBA = fixed4(finalColor * saturate((sceneZ-partZ)),0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Back
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float _WaveHeight;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                float XValue = 0.0;
                float4 node_4562 = _Time;
                float node_3048 = (node_4562.g+mul(unity_ObjectToWorld, v.vertex).r);
                float node_3998 = 0.05;
                float Step = node_3998;
                float node_5347 = Step;
                float2 node_6957 = sin(float2(node_3048,(node_3048+node_5347)));
                float2 node_6197 = (node_6957*_WaveHeight);
                float2 node_1308 = float2(1,0);
                float node_7274 = dot(node_6197,node_1308);
                float node_7743 = (node_4562.g+mul(unity_ObjectToWorld, v.vertex).b);
                float2 node_6191 = cos(float2(node_7743,(node_7743+node_5347)));
                float2 node_8399 = (node_6191*_WaveHeight);
                float node_7148 = dot(node_8399,node_1308);
                float node_4729 = (node_7274+node_7148);
                float YValue = node_4729;
                float ZValue = 0.0;
                v.vertex.xyz += float3(XValue,YValue,ZValue);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
        Pass {
            Name "Meta"
            Tags {
                "LightMode"="Meta"
            }
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #include "UnityMetaPass.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _Color;
            uniform float _Metallic;
            uniform float _Gloss;
            uniform float _WaveHeight;
            uniform float _node_4708;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv1 : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float4 posWorld : TEXCOORD2;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                float XValue = 0.0;
                float4 node_4562 = _Time;
                float node_3048 = (node_4562.g+mul(unity_ObjectToWorld, v.vertex).r);
                float node_3998 = 0.05;
                float Step = node_3998;
                float node_5347 = Step;
                float2 node_6957 = sin(float2(node_3048,(node_3048+node_5347)));
                float2 node_6197 = (node_6957*_WaveHeight);
                float2 node_1308 = float2(1,0);
                float node_7274 = dot(node_6197,node_1308);
                float node_7743 = (node_4562.g+mul(unity_ObjectToWorld, v.vertex).b);
                float2 node_6191 = cos(float2(node_7743,(node_7743+node_5347)));
                float2 node_8399 = (node_6191*_WaveHeight);
                float node_7148 = dot(node_8399,node_1308);
                float node_4729 = (node_7274+node_7148);
                float YValue = node_4729;
                float ZValue = 0.0;
                v.vertex.xyz += float3(XValue,YValue,ZValue);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityMetaVertexPosition(v.vertex, v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST );
                return o;
            }
            float4 frag(VertexOutput i) : SV_Target {
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                UnityMetaInput o;
                UNITY_INITIALIZE_OUTPUT( UnityMetaInput, o );
                
                o.Emission = 0;
                
                float node_9503 = 0.0;
                float4 node_4562 = _Time;
                float node_3048 = (node_4562.g+i.posWorld.r);
                float node_3998 = 0.05;
                float Step = node_3998;
                float node_5347 = Step;
                float2 node_6957 = sin(float2(node_3048,(node_3048+node_5347)));
                float2 node_6197 = (node_6957*_WaveHeight);
                float2 node_1308 = float2(1,0);
                float node_7274 = dot(node_6197,node_1308);
                float node_7743 = (node_4562.g+i.posWorld.b);
                float2 node_6191 = cos(float2(node_7743,(node_7743+node_5347)));
                float2 node_8399 = (node_6191*_WaveHeight);
                float node_7148 = dot(node_8399,node_1308);
                float node_4729 = (node_7274+node_7148);
                float node_9646 = Step;
                float3 node_9064 = float3(node_9646,((dot(node_6197,float2(0,1))+node_7148)-node_4729),node_9503);
                float3 Normal = normalize(cross(float3(node_9503,((node_7274+dot(node_8399,float2(0,1)))-node_4729),node_9646),node_9064));
                float3 diffColor = (pow(1.0-max(0,dot(Normal, viewDirection)),_node_4708)+_Color.rgb);
                float specularMonochrome;
                float3 specColor;
                diffColor = DiffuseAndSpecularFromMetallic( diffColor, _Metallic, specColor, specularMonochrome );
                float roughness = 1.0 - _Gloss;
                o.Albedo = diffColor + specColor * roughness * roughness * 0.5;
                
                return UnityMetaFragment( o );
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
