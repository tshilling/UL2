// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:True,hqlp:False,rprd:True,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:2865,x:32719,y:32712,varname:node_2865,prsc:2|diff-8225-RGB,normal-4930-OUT,emission-6404-OUT,alpha-452-OUT,voffset-9184-OUT;n:type:ShaderForge.SFN_Color,id:8225,x:31561,y:32824,ptovrint:False,ptlb:node_8225,ptin:_node_8225,varname:node_8225,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.2063012,c2:0.6501388,c3:0.7169812,c4:1;n:type:ShaderForge.SFN_Vector3,id:111,x:32413,y:33626,varname:node_111,prsc:2,v1:0,v2:1,v3:0;n:type:ShaderForge.SFN_Set,id:2619,x:32706,y:33485,varname:Normal,prsc:2|IN-9554-OUT;n:type:ShaderForge.SFN_Get,id:4930,x:32353,y:32790,varname:node_4930,prsc:2|IN-2619-OUT;n:type:ShaderForge.SFN_Get,id:6404,x:32353,y:32843,varname:node_6404,prsc:2|IN-3491-OUT;n:type:ShaderForge.SFN_Set,id:3491,x:32129,y:32824,varname:Emission,prsc:2|IN-9872-OUT;n:type:ShaderForge.SFN_Get,id:452,x:32463,y:32987,varname:node_452,prsc:2|IN-7333-OUT;n:type:ShaderForge.SFN_Set,id:7333,x:32798,y:33767,varname:Opacity,prsc:2|IN-9916-OUT;n:type:ShaderForge.SFN_DepthBlend,id:1160,x:32132,y:33767,varname:node_1160,prsc:2|DIST-3572-OUT;n:type:ShaderForge.SFN_Tex2d,id:4547,x:31374,y:32817,ptovrint:False,ptlb:node_4547,ptin:_node_4547,varname:node_4547,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:bfb644102ac36764787aab972f0b8793,ntxv:0,isnm:False|UVIN-1238-OUT;n:type:ShaderForge.SFN_Add,id:2983,x:31787,y:32824,varname:node_2983,prsc:2|A-8225-RGB,B-4098-OUT;n:type:ShaderForge.SFN_Lerp,id:4098,x:31561,y:32975,varname:node_4098,prsc:2|A-4547-RGB,B-9950-OUT,T-3588-OUT;n:type:ShaderForge.SFN_Vector1,id:9950,x:31374,y:32994,varname:node_9950,prsc:2,v1:0;n:type:ShaderForge.SFN_DepthBlend,id:3588,x:31374,y:33058,varname:node_3588,prsc:2|DIST-524-OUT;n:type:ShaderForge.SFN_ValueProperty,id:524,x:31191,y:33058,ptovrint:False,ptlb:Foam Strength,ptin:_FoamStrength,varname:node_524,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:3;n:type:ShaderForge.SFN_ViewVector,id:5334,x:30319,y:34094,varname:node_5334,prsc:2;n:type:ShaderForge.SFN_LightVector,id:4122,x:30186,y:33842,varname:node_4122,prsc:2;n:type:ShaderForge.SFN_Dot,id:9539,x:30485,y:33972,varname:node_9539,prsc:2,dt:0|A-1145-OUT,B-5334-OUT;n:type:ShaderForge.SFN_Multiply,id:9872,x:31973,y:32824,varname:node_9872,prsc:2|A-2983-OUT,B-953-OUT;n:type:ShaderForge.SFN_OneMinus,id:2394,x:30644,y:33972,varname:node_2394,prsc:2|IN-953-OUT;n:type:ShaderForge.SFN_TexCoord,id:6864,x:31000,y:32694,varname:node_6864,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Time,id:2140,x:31000,y:32836,varname:node_2140,prsc:2;n:type:ShaderForge.SFN_Add,id:1238,x:31214,y:32817,varname:node_1238,prsc:2|A-6864-UVOUT,B-2140-TSL;n:type:ShaderForge.SFN_RemapRangeAdvanced,id:9916,x:32561,y:33767,varname:node_9916,prsc:2|IN-1160-OUT,IMIN-379-OUT,IMAX-2439-OUT,OMIN-3427-OUT,OMAX-9503-OUT;n:type:ShaderForge.SFN_Slider,id:3427,x:31802,y:33858,ptovrint:False,ptlb:Trans Min,ptin:_TransMin,varname:node_3427,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:10;n:type:ShaderForge.SFN_Slider,id:9503,x:31802,y:33945,ptovrint:False,ptlb:Trans Max,ptin:_TransMax,varname:_node_3427_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Vector1,id:379,x:32244,y:33812,varname:node_379,prsc:2,v1:0;n:type:ShaderForge.SFN_Vector1,id:2439,x:32244,y:33865,varname:node_2439,prsc:2,v1:1;n:type:ShaderForge.SFN_Slider,id:3572,x:31802,y:33767,ptovrint:False,ptlb:Trans Depth,ptin:_TransDepth,varname:node_3572,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:4.025305,max:10;n:type:ShaderForge.SFN_Time,id:7765,x:29150,y:32843,varname:node_7765,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:6289,x:29170,y:32985,ptovrint:False,ptlb:WaveSpeed,ptin:_WaveSpeed,varname:node_6289,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:3571,x:29408,y:32764,varname:node_3571,prsc:2|A-7765-TSL,B-6289-OUT;n:type:ShaderForge.SFN_FragmentPosition,id:5428,x:29216,y:33048,varname:node_5428,prsc:2;n:type:ShaderForge.SFN_Add,id:2676,x:29766,y:32937,varname:node_2676,prsc:2|A-3571-OUT,B-5428-X,C-5428-Z;n:type:ShaderForge.SFN_ValueProperty,id:2195,x:29766,y:32874,ptovrint:False,ptlb:WaveCount,ptin:_WaveCount,varname:node_2195,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:10;n:type:ShaderForge.SFN_Multiply,id:7473,x:30351,y:32863,varname:node_7473,prsc:2|A-8984-OUT,B-3848-OUT;n:type:ShaderForge.SFN_ValueProperty,id:3848,x:30169,y:33017,ptovrint:False,ptlb:WaveSpread,ptin:_WaveSpread,varname:node_3848,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.3;n:type:ShaderForge.SFN_Add,id:4977,x:30682,y:32863,varname:node_4977,prsc:2|A-7473-OUT,B-429-OUT,C-2146-OUT,D-2973-OUT;n:type:ShaderForge.SFN_ValueProperty,id:429,x:30351,y:33017,ptovrint:False,ptlb:WaveWidth,ptin:_WaveWidth,varname:node_429,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:10;n:type:ShaderForge.SFN_Tex2d,id:3854,x:30145,y:33356,ptovrint:False,ptlb:node_3854,ptin:_node_3854,varname:node_3854,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:fc597129e2152974c9542c0caaed9413,ntxv:0,isnm:False|UVIN-2657-OUT;n:type:ShaderForge.SFN_Time,id:7214,x:29200,y:33483,varname:node_7214,prsc:2;n:type:ShaderForge.SFN_Append,id:9966,x:30689,y:33286,varname:node_9966,prsc:2|A-9574-OUT,B-4977-OUT,C-7728-OUT;n:type:ShaderForge.SFN_Vector1,id:9574,x:30616,y:33169,varname:node_9574,prsc:2,v1:0;n:type:ShaderForge.SFN_Vector1,id:7728,x:30571,y:33499,varname:node_7728,prsc:2,v1:0;n:type:ShaderForge.SFN_Set,id:6444,x:31136,y:33299,varname:Offset,prsc:2|IN-4889-OUT;n:type:ShaderForge.SFN_Get,id:9184,x:32468,y:33084,varname:node_9184,prsc:2|IN-6444-OUT;n:type:ShaderForge.SFN_RemapRange,id:2146,x:30317,y:33377,varname:node_2146,prsc:2,frmn:0,frmx:1,tomn:0,tomx:1|IN-3854-R;n:type:ShaderForge.SFN_DDXY,id:9554,x:32477,y:33485,varname:node_9554,prsc:2|IN-1224-OUT;n:type:ShaderForge.SFN_Get,id:1224,x:32237,y:33485,varname:node_1224,prsc:2|IN-6444-OUT;n:type:ShaderForge.SFN_Add,id:1114,x:29766,y:32716,varname:node_1114,prsc:2|A-5428-X,B-5428-Z,C-3571-OUT;n:type:ShaderForge.SFN_Multiply,id:9279,x:29974,y:32716,varname:node_9279,prsc:2|A-1114-OUT,B-2195-OUT;n:type:ShaderForge.SFN_Multiply,id:5317,x:30335,y:32716,varname:node_5317,prsc:2|A-6424-OUT,B-3848-OUT;n:type:ShaderForge.SFN_Add,id:2973,x:30519,y:32716,varname:node_2973,prsc:2|A-5317-OUT,B-429-OUT;n:type:ShaderForge.SFN_Sin,id:8984,x:30169,y:32863,varname:node_8984,prsc:2|IN-3744-OUT;n:type:ShaderForge.SFN_Multiply,id:3744,x:29974,y:32874,varname:node_3744,prsc:2|A-2676-OUT,B-2195-OUT;n:type:ShaderForge.SFN_Reflect,id:1145,x:30201,y:33997,varname:node_1145,prsc:2|A-4122-OUT,B-8431-OUT;n:type:ShaderForge.SFN_Get,id:6227,x:29828,y:34019,varname:node_6227,prsc:2|IN-6444-OUT;n:type:ShaderForge.SFN_RemapRange,id:1683,x:31004,y:33722,varname:node_1683,prsc:2,frmn:0,frmx:1,tomn:0,tomx:1|IN-953-OUT;n:type:ShaderForge.SFN_Multiply,id:1165,x:29497,y:33156,varname:node_1165,prsc:2|A-5428-X,B-1769-OUT;n:type:ShaderForge.SFN_Vector1,id:1769,x:29216,y:33232,varname:node_1769,prsc:2,v1:0.03;n:type:ShaderForge.SFN_Multiply,id:7978,x:29497,y:33282,varname:node_7978,prsc:2|A-5428-Z,B-1769-OUT;n:type:ShaderForge.SFN_Append,id:2657,x:29853,y:33263,varname:node_2657,prsc:2|A-3211-OUT,B-6446-OUT;n:type:ShaderForge.SFN_Add,id:3211,x:29693,y:33341,varname:node_3211,prsc:2|A-1165-OUT,B-3638-OUT;n:type:ShaderForge.SFN_Add,id:6446,x:29693,y:33472,varname:node_6446,prsc:2|A-7978-OUT,B-3638-OUT;n:type:ShaderForge.SFN_Multiply,id:3638,x:29497,y:33416,varname:node_3638,prsc:2|A-7214-TSL,B-1769-OUT;n:type:ShaderForge.SFN_Sin,id:6424,x:30169,y:32716,varname:node_6424,prsc:2|IN-9279-OUT;n:type:ShaderForge.SFN_RemapRange,id:4889,x:30853,y:33139,varname:node_4889,prsc:2,frmn:-1,frmx:1,tomn:-1,tomx:0|IN-9966-OUT;n:type:ShaderForge.SFN_Normalize,id:8431,x:30008,y:34050,varname:node_8431,prsc:2|IN-6227-OUT;n:type:ShaderForge.SFN_Fresnel,id:953,x:30586,y:33613,varname:node_953,prsc:2|NRM-8431-OUT,EXP-9288-OUT;n:type:ShaderForge.SFN_Vector1,id:9288,x:30385,y:33701,varname:node_9288,prsc:2,v1:1;proporder:8225-4547-524-3427-9503-3572-6289-2195-3848-429-3854;pass:END;sub:END;*/

Shader "Shader Forge/Water5" {
    Properties {
        _node_8225 ("node_8225", Color) = (0.2063012,0.6501388,0.7169812,1)
        _node_4547 ("node_4547", 2D) = "white" {}
        _FoamStrength ("Foam Strength", Float ) = 3
        _TransMin ("Trans Min", Range(0, 10)) = 0
        _TransMax ("Trans Max", Range(0, 1)) = 0
        _TransDepth ("Trans Depth", Range(0, 10)) = 4.025305
        _WaveSpeed ("WaveSpeed", Float ) = 1
        _WaveCount ("WaveCount", Float ) = 10
        _WaveSpread ("WaveSpread", Float ) = 0.3
        _WaveWidth ("WaveWidth", Float ) = 10
        _node_3854 ("node_3854", 2D) = "white" {}
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
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _CameraDepthTexture;
            uniform float4 _node_8225;
            uniform sampler2D _node_4547; uniform float4 _node_4547_ST;
            uniform float _FoamStrength;
            uniform float _TransMin;
            uniform float _TransMax;
            uniform float _TransDepth;
            uniform float _WaveSpeed;
            uniform float _WaveCount;
            uniform float _WaveSpread;
            uniform float _WaveWidth;
            uniform sampler2D _node_3854; uniform float4 _node_3854_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                float4 projPos : TEXCOORD5;
                UNITY_FOG_COORDS(6)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                float4 node_7765 = _Time;
                float node_3571 = (node_7765.r*_WaveSpeed);
                float node_1769 = 0.03;
                float4 node_7214 = _Time;
                float node_3638 = (node_7214.r*node_1769);
                float2 node_2657 = float2(((mul(unity_ObjectToWorld, v.vertex).r*node_1769)+node_3638),((mul(unity_ObjectToWorld, v.vertex).b*node_1769)+node_3638));
                float4 _node_3854_var = tex2Dlod(_node_3854,float4(TRANSFORM_TEX(node_2657, _node_3854),0.0,0));
                float node_9279 = ((mul(unity_ObjectToWorld, v.vertex).r+mul(unity_ObjectToWorld, v.vertex).b+node_3571)*_WaveCount);
                float3 node_4889 = (float3(0.0,((sin(((node_3571+mul(unity_ObjectToWorld, v.vertex).r+mul(unity_ObjectToWorld, v.vertex).b)*_WaveCount))*_WaveSpread)+_WaveWidth+(_node_3854_var.r*1.0+0.0)+((sin(node_9279)*_WaveSpread)+_WaveWidth)),0.0)*0.5+-0.5);
                float3 Offset = node_4889;
                v.vertex.xyz += Offset;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
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
                float4 node_7765 = _Time;
                float node_3571 = (node_7765.r*_WaveSpeed);
                float node_1769 = 0.03;
                float4 node_7214 = _Time;
                float node_3638 = (node_7214.r*node_1769);
                float2 node_2657 = float2(((i.posWorld.r*node_1769)+node_3638),((i.posWorld.b*node_1769)+node_3638));
                float4 _node_3854_var = tex2D(_node_3854,TRANSFORM_TEX(node_2657, _node_3854));
                float node_9279 = ((i.posWorld.r+i.posWorld.b+node_3571)*_WaveCount);
                float3 node_4889 = (float3(0.0,((sin(((node_3571+i.posWorld.r+i.posWorld.b)*_WaveCount))*_WaveSpread)+_WaveWidth+(_node_3854_var.r*1.0+0.0)+((sin(node_9279)*_WaveSpread)+_WaveWidth)),0.0)*0.5+-0.5);
                float3 Offset = node_4889;
                float3 Normal = fwidth(Offset);
                float3 normalLocal = Normal;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float sceneZ = max(0,LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)))) - _ProjectionParams.g);
                float partZ = max(0,i.projPos.z - _ProjectionParams.g);
////// Lighting:
////// Emissive:
                float4 node_2140 = _Time;
                float2 node_1238 = (i.uv0+node_2140.r);
                float4 _node_4547_var = tex2D(_node_4547,TRANSFORM_TEX(node_1238, _node_4547));
                float node_9950 = 0.0;
                float3 node_8431 = normalize(Offset);
                float node_953 = pow(1.0-max(0,dot(node_8431, viewDirection)),1.0);
                float3 Emission = ((_node_8225.rgb+lerp(_node_4547_var.rgb,float3(node_9950,node_9950,node_9950),saturate((sceneZ-partZ)/_FoamStrength)))*node_953);
                float3 emissive = Emission;
                float3 finalColor = emissive;
                float node_379 = 0.0;
                float Opacity = (_TransMin + ( (saturate((sceneZ-partZ)/_TransDepth) - node_379) * (_TransMax - _TransMin) ) / (1.0 - node_379));
                fixed4 finalRGBA = fixed4(finalColor,Opacity);
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
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float _WaveSpeed;
            uniform float _WaveCount;
            uniform float _WaveSpread;
            uniform float _WaveWidth;
            uniform sampler2D _node_3854; uniform float4 _node_3854_ST;
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float4 posWorld : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                float4 node_7765 = _Time;
                float node_3571 = (node_7765.r*_WaveSpeed);
                float node_1769 = 0.03;
                float4 node_7214 = _Time;
                float node_3638 = (node_7214.r*node_1769);
                float2 node_2657 = float2(((mul(unity_ObjectToWorld, v.vertex).r*node_1769)+node_3638),((mul(unity_ObjectToWorld, v.vertex).b*node_1769)+node_3638));
                float4 _node_3854_var = tex2Dlod(_node_3854,float4(TRANSFORM_TEX(node_2657, _node_3854),0.0,0));
                float node_9279 = ((mul(unity_ObjectToWorld, v.vertex).r+mul(unity_ObjectToWorld, v.vertex).b+node_3571)*_WaveCount);
                float3 node_4889 = (float3(0.0,((sin(((node_3571+mul(unity_ObjectToWorld, v.vertex).r+mul(unity_ObjectToWorld, v.vertex).b)*_WaveCount))*_WaveSpread)+_WaveWidth+(_node_3854_var.r*1.0+0.0)+((sin(node_9279)*_WaveSpread)+_WaveWidth)),0.0)*0.5+-0.5);
                float3 Offset = node_4889;
                v.vertex.xyz += Offset;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
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
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #include "UnityMetaPass.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _CameraDepthTexture;
            uniform float4 _node_8225;
            uniform sampler2D _node_4547; uniform float4 _node_4547_ST;
            uniform float _FoamStrength;
            uniform float _WaveSpeed;
            uniform float _WaveCount;
            uniform float _WaveSpread;
            uniform float _WaveWidth;
            uniform sampler2D _node_3854; uniform float4 _node_3854_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float4 projPos : TEXCOORD2;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                float4 node_7765 = _Time;
                float node_3571 = (node_7765.r*_WaveSpeed);
                float node_1769 = 0.03;
                float4 node_7214 = _Time;
                float node_3638 = (node_7214.r*node_1769);
                float2 node_2657 = float2(((mul(unity_ObjectToWorld, v.vertex).r*node_1769)+node_3638),((mul(unity_ObjectToWorld, v.vertex).b*node_1769)+node_3638));
                float4 _node_3854_var = tex2Dlod(_node_3854,float4(TRANSFORM_TEX(node_2657, _node_3854),0.0,0));
                float node_9279 = ((mul(unity_ObjectToWorld, v.vertex).r+mul(unity_ObjectToWorld, v.vertex).b+node_3571)*_WaveCount);
                float3 node_4889 = (float3(0.0,((sin(((node_3571+mul(unity_ObjectToWorld, v.vertex).r+mul(unity_ObjectToWorld, v.vertex).b)*_WaveCount))*_WaveSpread)+_WaveWidth+(_node_3854_var.r*1.0+0.0)+((sin(node_9279)*_WaveSpread)+_WaveWidth)),0.0)*0.5+-0.5);
                float3 Offset = node_4889;
                v.vertex.xyz += Offset;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityMetaVertexPosition(v.vertex, v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST );
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            float4 frag(VertexOutput i) : SV_Target {
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float sceneZ = max(0,LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)))) - _ProjectionParams.g);
                float partZ = max(0,i.projPos.z - _ProjectionParams.g);
                UnityMetaInput o;
                UNITY_INITIALIZE_OUTPUT( UnityMetaInput, o );
                
                float4 node_2140 = _Time;
                float2 node_1238 = (i.uv0+node_2140.r);
                float4 _node_4547_var = tex2D(_node_4547,TRANSFORM_TEX(node_1238, _node_4547));
                float node_9950 = 0.0;
                float4 node_7765 = _Time;
                float node_3571 = (node_7765.r*_WaveSpeed);
                float node_1769 = 0.03;
                float4 node_7214 = _Time;
                float node_3638 = (node_7214.r*node_1769);
                float2 node_2657 = float2(((i.posWorld.r*node_1769)+node_3638),((i.posWorld.b*node_1769)+node_3638));
                float4 _node_3854_var = tex2D(_node_3854,TRANSFORM_TEX(node_2657, _node_3854));
                float node_9279 = ((i.posWorld.r+i.posWorld.b+node_3571)*_WaveCount);
                float3 node_4889 = (float3(0.0,((sin(((node_3571+i.posWorld.r+i.posWorld.b)*_WaveCount))*_WaveSpread)+_WaveWidth+(_node_3854_var.r*1.0+0.0)+((sin(node_9279)*_WaveSpread)+_WaveWidth)),0.0)*0.5+-0.5);
                float3 Offset = node_4889;
                float3 node_8431 = normalize(Offset);
                float node_953 = pow(1.0-max(0,dot(node_8431, viewDirection)),1.0);
                float3 Emission = ((_node_8225.rgb+lerp(_node_4547_var.rgb,float3(node_9950,node_9950,node_9950),saturate((sceneZ-partZ)/_FoamStrength)))*node_953);
                o.Emission = Emission;
                
                float3 diffColor = float3(0,0,0);
                o.Albedo = diffColor;
                
                return UnityMetaFragment( o );
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
