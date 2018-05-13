// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:3,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:True,hqlp:False,rprd:True,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:2865,x:32719,y:32712,varname:node_2865,prsc:2|diff-283-OUT,spec-358-OUT,gloss-1813-OUT,normal-5365-OUT,alpha-1586-OUT,voffset-363-OUT;n:type:ShaderForge.SFN_Color,id:6665,x:31375,y:32617,ptovrint:False,ptlb:Color,ptin:_Color,varname:_Color,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5019608,c2:0.5019608,c3:0.5019608,c4:1;n:type:ShaderForge.SFN_Slider,id:358,x:32250,y:32780,ptovrint:False,ptlb:Metallic,ptin:_Metallic,varname:node_358,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.9,max:1;n:type:ShaderForge.SFN_Slider,id:1813,x:32250,y:32882,ptovrint:False,ptlb:Gloss,ptin:_Gloss,varname:_Metallic_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.2,max:1;n:type:ShaderForge.SFN_Time,id:2550,x:33150,y:32397,varname:node_2550,prsc:2;n:type:ShaderForge.SFN_FragmentPosition,id:3837,x:33760,y:32393,varname:node_3837,prsc:2;n:type:ShaderForge.SFN_Code,id:3853,x:33808,y:32613,varname:node_3853,prsc:2,code:ZgBsAG8AYQB0ADMAIAByAGUAcwB1AGwAdAAgAD0AIABmAGwAbwBhAHQAMwAoADAALAAwACwAMAApADsACgBmAGwAbwBhAHQAIABUAGkAbQBlACAAPQAgAFAAYQByAG0AMQAuAHgAOwAKAGYAbABvAGEAdAAgAGEAbgBnAGwAZQAgAD0AIABQAGEAcgBtADEALgB5ACoAMwAuADEANAAxADUAOQAvADEAOAAwAC4AMAA7AAoAZgBsAG8AYQB0ADIAIAB2AGUAYwAgAD0AIABmAGwAbwBhAHQAMgAoAGMAbwBzACgAYQBuAGcAbABlACkALABzAGkAbgAoAGEAbgBnAGwAZQApACkAOwAKAGYAbABvAGEAdAAyACAAdgBlAGMAMgAgAD0AIABmAGwAbwBhAHQAMgAoAGMAbwBzACgAYQBuAGcAbABlACsAMwAuADEANAAxADUAOQAvADIALgAwACkALABzAGkAbgAoAGEAbgBnAGwAZQArADMALgAxADQAMQA1ADkALwAyAC4AMAApACkAOwAKAAoAcgBlAHMAdQBsAHQALgB5ACAAPQAgADAALgAyADUAKgBzAGkAbgAoACgAVABpAG0AZQAgACsAIAB2AGUAYwAuAHgAKgBQAG8AaQBuAHQALgB4ACsAdgBlAGMALgB5ACoAUABvAGkAbgB0AC4AegApACoALgAyADUAKQA7AAoAcgBlAHMAdQBsAHQALgB5ACAAKwA9ACAAMAAuADEAMgA1ACoAcwBpAG4AKAAoAFQAaQBtAGUAIAArACAAdgBlAGMAMgAuAHgAKgBQAG8AaQBuAHQALgB4ACsAdgBlAGMAMgAuAHkAKgBQAG8AaQBuAHQALgB6ACkAKgAxACkAOwAKAHIAZQBzAHUAbAB0AC4AeQAgACsAPQAuADAANgAyADUAIAAqACAAcwBpAG4AKAAoAFQAaQBtAGUAIAArACAAdgBlAGMALgB4ACoAUABvAGkAbgB0AC4AeAArAHYAZQBjAC4AeQAqAFAAbwBpAG4AdAAuAHoAKQAqADIAKQA7AAoACgAKAAoAcgBlAHMAdQBsAHQALgB5ACAAKwA9ACAAUABvAGkAbgB0AC4AeQA7AAoAcgBlAHMAdQBsAHQALgB5ACAALwA9ACAAMwAuADAAOwAKAHIAZQBzAHUAbAB0AC4AeQAgACsAPQAgAC0ALgA1ADsACgByAGUAdAB1AHIAbgAgAHIAZQBzAHUAbAB0ADsA,output:2,fname:MyFunction,width:602,height:293,input:2,input:3,input_1_label:Point,input_2_label:Parm1|A-7327-OUT,B-5443-OUT;n:type:ShaderForge.SFN_Set,id:1850,x:34744,y:32684,varname:Offset,prsc:2|IN-3853-OUT;n:type:ShaderForge.SFN_Get,id:363,x:32125,y:33069,varname:node_363,prsc:2|IN-1850-OUT;n:type:ShaderForge.SFN_Code,id:4686,x:33765,y:33073,varname:node_4686,prsc:2,code:cgBlAHQAdQByAG4AIABNAHkARgB1AG4AYwB0AGkAbwBuACgAUABvAGkAbgB0ACwAIABUAGkAbQBlACkAOwA=,output:2,fname:XAxis,width:604,height:237,input:2,input:3,input_1_label:Point,input_2_label:Time|A-2782-OUT,B-5443-OUT;n:type:ShaderForge.SFN_Set,id:5992,x:34959,y:32554,varname:InputPoint,prsc:2|IN-7543-OUT;n:type:ShaderForge.SFN_Get,id:7327,x:33548,y:32673,varname:node_7327,prsc:2|IN-5992-OUT;n:type:ShaderForge.SFN_Set,id:2524,x:33326,y:32415,varname:InputTime,prsc:2|IN-2550-T;n:type:ShaderForge.SFN_Get,id:4791,x:33348,y:32747,varname:node_4791,prsc:2|IN-2524-OUT;n:type:ShaderForge.SFN_Add,id:5806,x:34478,y:33118,varname:node_5806,prsc:2|A-4686-OUT,B-6217-OUT;n:type:ShaderForge.SFN_Subtract,id:7183,x:34724,y:33118,varname:node_7183,prsc:2|A-5806-OUT,B-3853-OUT;n:type:ShaderForge.SFN_Add,id:3780,x:34484,y:33398,varname:node_3780,prsc:2|A-8511-OUT,B-440-OUT;n:type:ShaderForge.SFN_Subtract,id:478,x:34730,y:33398,varname:node_478,prsc:2|A-3780-OUT,B-3853-OUT;n:type:ShaderForge.SFN_Code,id:8511,x:33767,y:33355,varname:node_8511,prsc:2,code:cgBlAHQAdQByAG4AIABNAHkARgB1AG4AYwB0AGkAbwBuACgAUABvAGkAbgB0ACwAVABpAG0AZQApADsA,output:2,fname:YAxis,width:627,height:237,input:2,input:3,input_1_label:Point,input_2_label:Time|A-2163-OUT,B-5443-OUT;n:type:ShaderForge.SFN_Cross,id:5566,x:34989,y:33278,varname:node_5566,prsc:2|A-478-OUT,B-7183-OUT;n:type:ShaderForge.SFN_Set,id:4239,x:35414,y:33278,varname:Normal,prsc:2|IN-5923-OUT;n:type:ShaderForge.SFN_Get,id:5365,x:31581,y:32920,varname:node_5365,prsc:2|IN-4239-OUT;n:type:ShaderForge.SFN_Append,id:5443,x:33569,y:32747,varname:node_5443,prsc:2|A-4791-OUT,B-9424-OUT,C-2343-OUT,D-5910-OUT;n:type:ShaderForge.SFN_Slider,id:9424,x:33225,y:32819,ptovrint:False,ptlb:Angle,ptin:_Angle,varname:node_9424,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:61.9351,max:360;n:type:ShaderForge.SFN_Vector1,id:2343,x:33377,y:32907,varname:node_2343,prsc:2,v1:0;n:type:ShaderForge.SFN_Vector1,id:5910,x:33420,y:32959,varname:node_5910,prsc:2,v1:0;n:type:ShaderForge.SFN_Add,id:283,x:32440,y:32591,varname:node_283,prsc:2|A-9497-OUT,B-6665-RGB,C-5604-OUT;n:type:ShaderForge.SFN_Fresnel,id:9497,x:31945,y:32720,varname:node_9497,prsc:2|NRM-5365-OUT,EXP-4915-OUT;n:type:ShaderForge.SFN_ValueProperty,id:4915,x:31714,y:32740,ptovrint:False,ptlb:Fresnel Power,ptin:_FresnelPower,varname:node_4915,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:20;n:type:ShaderForge.SFN_Append,id:2321,x:34638,y:32423,varname:node_2321,prsc:2|A-3837-X,B-3837-Z;n:type:ShaderForge.SFN_Set,id:8231,x:34959,y:32429,varname:Coord,prsc:2|IN-2321-OUT;n:type:ShaderForge.SFN_DepthBlend,id:9903,x:32407,y:32971,varname:node_9903,prsc:2|DIST-1356-OUT;n:type:ShaderForge.SFN_Tex2d,id:3406,x:31375,y:32098,ptovrint:False,ptlb:FoamTexture,ptin:_FoamTexture,varname:node_8008,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:bfb644102ac36764787aab972f0b8793,ntxv:1,isnm:False|UVIN-9618-OUT;n:type:ShaderForge.SFN_Color,id:7791,x:31375,y:32275,ptovrint:False,ptlb:Foam Color,ptin:_FoamColor,varname:_Color_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0,c3:0.1,c4:1;n:type:ShaderForge.SFN_Add,id:4719,x:31588,y:32206,varname:node_4719,prsc:2|A-3406-RGB,B-7791-RGB;n:type:ShaderForge.SFN_Lerp,id:5604,x:31902,y:32229,varname:node_5604,prsc:2|A-4719-OUT,B-530-OUT,T-4096-OUT;n:type:ShaderForge.SFN_ValueProperty,id:129,x:31375,y:32450,ptovrint:False,ptlb:Foam Distance,ptin:_FoamDistance,varname:node_7559,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:5;n:type:ShaderForge.SFN_DepthBlend,id:4096,x:31588,y:32450,varname:node_4096,prsc:2|DIST-129-OUT;n:type:ShaderForge.SFN_Vector1,id:530,x:31696,y:32253,varname:node_530,prsc:2,v1:0;n:type:ShaderForge.SFN_ValueProperty,id:1356,x:32141,y:32975,ptovrint:False,ptlb:Edge Distance,ptin:_EdgeDistance,varname:node_1356,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:5;n:type:ShaderForge.SFN_Tex2d,id:2026,x:34401,y:32215,ptovrint:False,ptlb:node_2026,ptin:_node_2026,varname:node_2026,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:fc597129e2152974c9542c0caaed9413,ntxv:0,isnm:False|UVIN-2482-OUT;n:type:ShaderForge.SFN_Add,id:8649,x:34043,y:32215,varname:node_8649,prsc:2|A-6171-OUT,B-6516-OUT;n:type:ShaderForge.SFN_Get,id:6516,x:33841,y:32265,varname:node_6516,prsc:2|IN-8231-OUT;n:type:ShaderForge.SFN_Multiply,id:2482,x:34228,y:32215,varname:node_2482,prsc:2|A-8649-OUT,B-9167-OUT;n:type:ShaderForge.SFN_Vector1,id:9167,x:34043,y:32338,varname:node_9167,prsc:2,v1:0.05;n:type:ShaderForge.SFN_Normalize,id:5923,x:35222,y:33278,varname:node_5923,prsc:2|IN-5566-OUT;n:type:ShaderForge.SFN_Append,id:7543,x:34638,y:32548,varname:node_7543,prsc:2|A-3837-X,B-2026-G,C-3837-Z;n:type:ShaderForge.SFN_Set,id:6995,x:34970,y:31781,varname:InputZ,prsc:2|IN-3925-OUT;n:type:ShaderForge.SFN_Set,id:4967,x:34959,y:32007,varname:InputX,prsc:2|IN-9487-OUT;n:type:ShaderForge.SFN_Multiply,id:3605,x:34228,y:31960,varname:node_3605,prsc:2|A-1733-OUT,B-3679-OUT;n:type:ShaderForge.SFN_Tex2d,id:4567,x:34401,y:31990,ptovrint:False,ptlb:node_2026_copy,ptin:_node_2026_copy,varname:_node_2026_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:fc597129e2152974c9542c0caaed9413,ntxv:0,isnm:False|UVIN-3605-OUT;n:type:ShaderForge.SFN_Vector1,id:3679,x:34043,y:32113,varname:node_3679,prsc:2,v1:0.05;n:type:ShaderForge.SFN_Multiply,id:3944,x:34228,y:31766,varname:node_3944,prsc:2|A-1959-OUT,B-5858-OUT;n:type:ShaderForge.SFN_Tex2d,id:7629,x:34401,y:31766,ptovrint:False,ptlb:node_2026_copy_copy,ptin:_node_2026_copy_copy,varname:_node_2026_copy_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:fc597129e2152974c9542c0caaed9413,ntxv:0,isnm:False|UVIN-3944-OUT;n:type:ShaderForge.SFN_Vector1,id:5858,x:34043,y:31889,varname:node_5858,prsc:2,v1:0.05;n:type:ShaderForge.SFN_Get,id:3208,x:33534,y:31688,varname:node_3208,prsc:2|IN-2524-OUT;n:type:ShaderForge.SFN_Vector1,id:5808,x:33334,y:31970,varname:node_5808,prsc:2,v1:0.05;n:type:ShaderForge.SFN_Append,id:9487,x:34757,y:32007,varname:node_9487,prsc:2|A-6981-OUT,B-4567-G,C-6162-OUT;n:type:ShaderForge.SFN_Add,id:6981,x:33510,y:31970,varname:node_6981,prsc:2|A-5808-OUT,B-185-OUT;n:type:ShaderForge.SFN_Append,id:7915,x:33728,y:32056,varname:node_7915,prsc:2|A-6981-OUT,B-4480-OUT;n:type:ShaderForge.SFN_Set,id:8439,x:33916,y:32378,varname:XCoord,prsc:2|IN-3837-X;n:type:ShaderForge.SFN_Set,id:2487,x:33916,y:32488,varname:ZCoord,prsc:2|IN-3837-Z;n:type:ShaderForge.SFN_Get,id:4480,x:33489,y:32092,varname:node_4480,prsc:2|IN-2487-OUT;n:type:ShaderForge.SFN_Get,id:185,x:33313,y:32025,varname:node_185,prsc:2|IN-8439-OUT;n:type:ShaderForge.SFN_Get,id:6162,x:34574,y:32079,varname:node_6162,prsc:2|IN-2487-OUT;n:type:ShaderForge.SFN_Vector1,id:886,x:33334,y:31824,varname:node_886,prsc:2,v1:0.05;n:type:ShaderForge.SFN_Get,id:9881,x:33313,y:31879,varname:node_9881,prsc:2|IN-2487-OUT;n:type:ShaderForge.SFN_Add,id:1896,x:33510,y:31824,varname:node_1896,prsc:2|A-886-OUT,B-9881-OUT;n:type:ShaderForge.SFN_Get,id:2754,x:33489,y:31762,varname:node_2754,prsc:2|IN-8439-OUT;n:type:ShaderForge.SFN_Append,id:3910,x:33697,y:31806,varname:node_3910,prsc:2|A-2754-OUT,B-1896-OUT;n:type:ShaderForge.SFN_Append,id:3925,x:34757,y:31781,varname:node_3925,prsc:2|A-8508-OUT,B-7629-G,C-1896-OUT;n:type:ShaderForge.SFN_Get,id:8508,x:34567,y:31766,varname:node_8508,prsc:2|IN-8439-OUT;n:type:ShaderForge.SFN_Get,id:2782,x:33303,y:33209,varname:node_2782,prsc:2|IN-4967-OUT;n:type:ShaderForge.SFN_Get,id:2163,x:33303,y:33360,varname:node_2163,prsc:2|IN-6995-OUT;n:type:ShaderForge.SFN_Vector3,id:6217,x:34701,y:32921,varname:node_6217,prsc:2,v1:0.1,v2:0,v3:0;n:type:ShaderForge.SFN_Vector3,id:440,x:34588,y:33598,varname:node_440,prsc:2,v1:0,v2:0,v3:0.1;n:type:ShaderForge.SFN_Add,id:1959,x:34028,y:31749,varname:node_1959,prsc:2|A-6171-OUT,B-3910-OUT;n:type:ShaderForge.SFN_Add,id:1733,x:34057,y:32006,varname:node_1733,prsc:2|A-6171-OUT,B-7915-OUT;n:type:ShaderForge.SFN_Multiply,id:6171,x:33733,y:31709,varname:node_6171,prsc:2|A-3208-OUT,B-3724-OUT;n:type:ShaderForge.SFN_Vector1,id:3724,x:33534,y:31647,varname:node_3724,prsc:2,v1:0.1;n:type:ShaderForge.SFN_Get,id:3351,x:30041,y:31804,varname:node_3351,prsc:2|IN-2524-OUT;n:type:ShaderForge.SFN_Append,id:9618,x:30862,y:32023,varname:node_9618,prsc:2|A-5386-OUT,B-761-OUT;n:type:ShaderForge.SFN_Get,id:2494,x:30269,y:32022,varname:node_2494,prsc:2|IN-8439-OUT;n:type:ShaderForge.SFN_Get,id:2806,x:30325,y:32108,varname:node_2806,prsc:2|IN-2487-OUT;n:type:ShaderForge.SFN_Add,id:5386,x:30606,y:31861,varname:node_5386,prsc:2|A-950-OUT,B-2494-OUT;n:type:ShaderForge.SFN_Cos,id:950,x:30269,y:31871,varname:node_950,prsc:2|IN-1413-OUT;n:type:ShaderForge.SFN_Multiply,id:1413,x:30058,y:31871,varname:node_1413,prsc:2|A-3351-OUT,B-6074-OUT;n:type:ShaderForge.SFN_Vector1,id:6074,x:29837,y:31974,varname:node_6074,prsc:2,v1:0.2;n:type:ShaderForge.SFN_Sin,id:3240,x:30275,y:32225,varname:node_3240,prsc:2|IN-1413-OUT;n:type:ShaderForge.SFN_Add,id:761,x:30516,y:32209,varname:node_761,prsc:2|A-3240-OUT,B-2806-OUT;n:type:ShaderForge.SFN_RemapRange,id:1586,x:32461,y:33170,varname:node_1586,prsc:2,frmn:0,frmx:1,tomn:0.5,tomx:1|IN-9903-OUT;proporder:6665-358-1813-9424-4915-3406-7791-129-1356-2026-4567-7629;pass:END;sub:END;*/

Shader "Shader Forge/Water8" {
    Properties {
        _Color ("Color", Color) = (0.5019608,0.5019608,0.5019608,1)
        _Metallic ("Metallic", Range(0, 1)) = 0.9
        _Gloss ("Gloss", Range(0, 1)) = 0.2
        _Angle ("Angle", Range(0, 360)) = 61.9351
        _FresnelPower ("Fresnel Power", Float ) = 20
        _FoamTexture ("FoamTexture", 2D) = "gray" {}
        _FoamColor ("Foam Color", Color) = (0,0,0.1,1)
        _FoamDistance ("Foam Distance", Float ) = 5
        _EdgeDistance ("Edge Distance", Float ) = 5
        _node_2026 ("node_2026", 2D) = "white" {}
        _node_2026_copy ("node_2026_copy", 2D) = "white" {}
        _node_2026_copy_copy ("node_2026_copy_copy", 2D) = "white" {}
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
            Cull Off
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
            float3 MyFunction( float3 Point , float4 Parm1 ){
            float3 result = float3(0,0,0);
            float Time = Parm1.x;
            float angle = Parm1.y*3.14159/180.0;
            float2 vec = float2(cos(angle),sin(angle));
            float2 vec2 = float2(cos(angle+3.14159/2.0),sin(angle+3.14159/2.0));
            
            result.y = 0.25*sin((Time + vec.x*Point.x+vec.y*Point.z)*.25);
            result.y += 0.125*sin((Time + vec2.x*Point.x+vec2.y*Point.z)*1);
            result.y +=.0625 * sin((Time + vec.x*Point.x+vec.y*Point.z)*2);
            
            
            
            result.y += Point.y;
            result.y /= 3.0;
            result.y += -.5;
            return result;
            }
            
            float3 XAxis( float3 Point , float4 Time ){
            return MyFunction(Point, Time);
            }
            
            float3 YAxis( float3 Point , float4 Time ){
            return MyFunction(Point,Time);
            }
            
            uniform float _Angle;
            uniform float _FresnelPower;
            uniform sampler2D _FoamTexture; uniform float4 _FoamTexture_ST;
            uniform float4 _FoamColor;
            uniform float _FoamDistance;
            uniform float _EdgeDistance;
            uniform sampler2D _node_2026; uniform float4 _node_2026_ST;
            uniform sampler2D _node_2026_copy; uniform float4 _node_2026_copy_ST;
            uniform sampler2D _node_2026_copy_copy; uniform float4 _node_2026_copy_copy_ST;
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
                float4 node_2550 = _Time;
                float InputTime = node_2550.g;
                float node_6171 = (InputTime*0.1);
                float2 Coord = float2(mul(unity_ObjectToWorld, v.vertex).r,mul(unity_ObjectToWorld, v.vertex).b);
                float2 node_2482 = ((node_6171+Coord)*0.05);
                float4 _node_2026_var = tex2Dlod(_node_2026,float4(TRANSFORM_TEX(node_2482, _node_2026),0.0,0));
                float3 InputPoint = float3(mul(unity_ObjectToWorld, v.vertex).r,_node_2026_var.g,mul(unity_ObjectToWorld, v.vertex).b);
                float4 node_5443 = float4(InputTime,_Angle,0.0,0.0);
                float3 node_3853 = MyFunction( InputPoint , node_5443 );
                float3 Offset = node_3853;
                v.vertex.xyz += Offset;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float XCoord = i.posWorld.r;
                float4 node_2550 = _Time;
                float InputTime = node_2550.g;
                float node_6171 = (InputTime*0.1);
                float ZCoord = i.posWorld.b;
                float node_1896 = (0.05+ZCoord);
                float2 node_3944 = ((node_6171+float2(XCoord,node_1896))*0.05);
                float4 _node_2026_copy_copy_var = tex2D(_node_2026_copy_copy,TRANSFORM_TEX(node_3944, _node_2026_copy_copy));
                float3 InputZ = float3(XCoord,_node_2026_copy_copy_var.g,node_1896);
                float4 node_5443 = float4(InputTime,_Angle,0.0,0.0);
                float2 Coord = float2(i.posWorld.r,i.posWorld.b);
                float2 node_2482 = ((node_6171+Coord)*0.05);
                float4 _node_2026_var = tex2D(_node_2026,TRANSFORM_TEX(node_2482, _node_2026));
                float3 InputPoint = float3(i.posWorld.r,_node_2026_var.g,i.posWorld.b);
                float3 node_3853 = MyFunction( InputPoint , node_5443 );
                float node_6981 = (0.05+XCoord);
                float2 node_3605 = ((node_6171+float2(node_6981,ZCoord))*0.05);
                float4 _node_2026_copy_var = tex2D(_node_2026_copy,TRANSFORM_TEX(node_3605, _node_2026_copy));
                float3 InputX = float3(node_6981,_node_2026_copy_var.g,ZCoord);
                float3 Normal = normalize(cross(((YAxis( InputZ , node_5443 )+float3(0,0,0.1))-node_3853),((XAxis( InputX , node_5443 )+float3(0.1,0,0))-node_3853)));
                float3 node_5365 = Normal;
                float3 normalLocal = node_5365;
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
                float node_1413 = (InputTime*0.2);
                float2 node_9618 = float2((cos(node_1413)+XCoord),(sin(node_1413)+ZCoord));
                float4 _FoamTexture_var = tex2D(_FoamTexture,TRANSFORM_TEX(node_9618, _FoamTexture));
                float node_530 = 0.0;
                float3 diffuseColor = (pow(1.0-max(0,dot(node_5365, viewDirection)),_FresnelPower)+_Color.rgb+lerp((_FoamTexture_var.rgb+_FoamColor.rgb),float3(node_530,node_530,node_530),saturate((sceneZ-partZ)/_FoamDistance))); // Need this for specular when using metallic
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
                fixed4 finalRGBA = fixed4(finalColor,(saturate((sceneZ-partZ)/_EdgeDistance)*0.5+0.5));
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
            Cull Off
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
            float3 MyFunction( float3 Point , float4 Parm1 ){
            float3 result = float3(0,0,0);
            float Time = Parm1.x;
            float angle = Parm1.y*3.14159/180.0;
            float2 vec = float2(cos(angle),sin(angle));
            float2 vec2 = float2(cos(angle+3.14159/2.0),sin(angle+3.14159/2.0));
            
            result.y = 0.25*sin((Time + vec.x*Point.x+vec.y*Point.z)*.25);
            result.y += 0.125*sin((Time + vec2.x*Point.x+vec2.y*Point.z)*1);
            result.y +=.0625 * sin((Time + vec.x*Point.x+vec.y*Point.z)*2);
            
            
            
            result.y += Point.y;
            result.y /= 3.0;
            result.y += -.5;
            return result;
            }
            
            float3 XAxis( float3 Point , float4 Time ){
            return MyFunction(Point, Time);
            }
            
            float3 YAxis( float3 Point , float4 Time ){
            return MyFunction(Point,Time);
            }
            
            uniform float _Angle;
            uniform float _FresnelPower;
            uniform sampler2D _FoamTexture; uniform float4 _FoamTexture_ST;
            uniform float4 _FoamColor;
            uniform float _FoamDistance;
            uniform float _EdgeDistance;
            uniform sampler2D _node_2026; uniform float4 _node_2026_ST;
            uniform sampler2D _node_2026_copy; uniform float4 _node_2026_copy_ST;
            uniform sampler2D _node_2026_copy_copy; uniform float4 _node_2026_copy_copy_ST;
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
                float4 node_2550 = _Time;
                float InputTime = node_2550.g;
                float node_6171 = (InputTime*0.1);
                float2 Coord = float2(mul(unity_ObjectToWorld, v.vertex).r,mul(unity_ObjectToWorld, v.vertex).b);
                float2 node_2482 = ((node_6171+Coord)*0.05);
                float4 _node_2026_var = tex2Dlod(_node_2026,float4(TRANSFORM_TEX(node_2482, _node_2026),0.0,0));
                float3 InputPoint = float3(mul(unity_ObjectToWorld, v.vertex).r,_node_2026_var.g,mul(unity_ObjectToWorld, v.vertex).b);
                float4 node_5443 = float4(InputTime,_Angle,0.0,0.0);
                float3 node_3853 = MyFunction( InputPoint , node_5443 );
                float3 Offset = node_3853;
                v.vertex.xyz += Offset;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float XCoord = i.posWorld.r;
                float4 node_2550 = _Time;
                float InputTime = node_2550.g;
                float node_6171 = (InputTime*0.1);
                float ZCoord = i.posWorld.b;
                float node_1896 = (0.05+ZCoord);
                float2 node_3944 = ((node_6171+float2(XCoord,node_1896))*0.05);
                float4 _node_2026_copy_copy_var = tex2D(_node_2026_copy_copy,TRANSFORM_TEX(node_3944, _node_2026_copy_copy));
                float3 InputZ = float3(XCoord,_node_2026_copy_copy_var.g,node_1896);
                float4 node_5443 = float4(InputTime,_Angle,0.0,0.0);
                float2 Coord = float2(i.posWorld.r,i.posWorld.b);
                float2 node_2482 = ((node_6171+Coord)*0.05);
                float4 _node_2026_var = tex2D(_node_2026,TRANSFORM_TEX(node_2482, _node_2026));
                float3 InputPoint = float3(i.posWorld.r,_node_2026_var.g,i.posWorld.b);
                float3 node_3853 = MyFunction( InputPoint , node_5443 );
                float node_6981 = (0.05+XCoord);
                float2 node_3605 = ((node_6171+float2(node_6981,ZCoord))*0.05);
                float4 _node_2026_copy_var = tex2D(_node_2026_copy,TRANSFORM_TEX(node_3605, _node_2026_copy));
                float3 InputX = float3(node_6981,_node_2026_copy_var.g,ZCoord);
                float3 Normal = normalize(cross(((YAxis( InputZ , node_5443 )+float3(0,0,0.1))-node_3853),((XAxis( InputX , node_5443 )+float3(0.1,0,0))-node_3853)));
                float3 node_5365 = Normal;
                float3 normalLocal = node_5365;
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
                float node_1413 = (InputTime*0.2);
                float2 node_9618 = float2((cos(node_1413)+XCoord),(sin(node_1413)+ZCoord));
                float4 _FoamTexture_var = tex2D(_FoamTexture,TRANSFORM_TEX(node_9618, _FoamTexture));
                float node_530 = 0.0;
                float3 diffuseColor = (pow(1.0-max(0,dot(node_5365, viewDirection)),_FresnelPower)+_Color.rgb+lerp((_FoamTexture_var.rgb+_FoamColor.rgb),float3(node_530,node_530,node_530),saturate((sceneZ-partZ)/_FoamDistance))); // Need this for specular when using metallic
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
                fixed4 finalRGBA = fixed4(finalColor * (saturate((sceneZ-partZ)/_EdgeDistance)*0.5+0.5),0);
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
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            float3 MyFunction( float3 Point , float4 Parm1 ){
            float3 result = float3(0,0,0);
            float Time = Parm1.x;
            float angle = Parm1.y*3.14159/180.0;
            float2 vec = float2(cos(angle),sin(angle));
            float2 vec2 = float2(cos(angle+3.14159/2.0),sin(angle+3.14159/2.0));
            
            result.y = 0.25*sin((Time + vec.x*Point.x+vec.y*Point.z)*.25);
            result.y += 0.125*sin((Time + vec2.x*Point.x+vec2.y*Point.z)*1);
            result.y +=.0625 * sin((Time + vec.x*Point.x+vec.y*Point.z)*2);
            
            
            
            result.y += Point.y;
            result.y /= 3.0;
            result.y += -.5;
            return result;
            }
            
            uniform float _Angle;
            uniform sampler2D _node_2026; uniform float4 _node_2026_ST;
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
                float4 node_2550 = _Time;
                float InputTime = node_2550.g;
                float node_6171 = (InputTime*0.1);
                float2 Coord = float2(mul(unity_ObjectToWorld, v.vertex).r,mul(unity_ObjectToWorld, v.vertex).b);
                float2 node_2482 = ((node_6171+Coord)*0.05);
                float4 _node_2026_var = tex2Dlod(_node_2026,float4(TRANSFORM_TEX(node_2482, _node_2026),0.0,0));
                float3 InputPoint = float3(mul(unity_ObjectToWorld, v.vertex).r,_node_2026_var.g,mul(unity_ObjectToWorld, v.vertex).b);
                float4 node_5443 = float4(InputTime,_Angle,0.0,0.0);
                float3 node_3853 = MyFunction( InputPoint , node_5443 );
                float3 Offset = node_3853;
                v.vertex.xyz += Offset;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
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
            uniform sampler2D _CameraDepthTexture;
            uniform float4 _Color;
            uniform float _Metallic;
            uniform float _Gloss;
            float3 MyFunction( float3 Point , float4 Parm1 ){
            float3 result = float3(0,0,0);
            float Time = Parm1.x;
            float angle = Parm1.y*3.14159/180.0;
            float2 vec = float2(cos(angle),sin(angle));
            float2 vec2 = float2(cos(angle+3.14159/2.0),sin(angle+3.14159/2.0));
            
            result.y = 0.25*sin((Time + vec.x*Point.x+vec.y*Point.z)*.25);
            result.y += 0.125*sin((Time + vec2.x*Point.x+vec2.y*Point.z)*1);
            result.y +=.0625 * sin((Time + vec.x*Point.x+vec.y*Point.z)*2);
            
            
            
            result.y += Point.y;
            result.y /= 3.0;
            result.y += -.5;
            return result;
            }
            
            float3 XAxis( float3 Point , float4 Time ){
            return MyFunction(Point, Time);
            }
            
            float3 YAxis( float3 Point , float4 Time ){
            return MyFunction(Point,Time);
            }
            
            uniform float _Angle;
            uniform float _FresnelPower;
            uniform sampler2D _FoamTexture; uniform float4 _FoamTexture_ST;
            uniform float4 _FoamColor;
            uniform float _FoamDistance;
            uniform sampler2D _node_2026; uniform float4 _node_2026_ST;
            uniform sampler2D _node_2026_copy; uniform float4 _node_2026_copy_ST;
            uniform sampler2D _node_2026_copy_copy; uniform float4 _node_2026_copy_copy_ST;
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
                float4 projPos : TEXCOORD3;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                float4 node_2550 = _Time;
                float InputTime = node_2550.g;
                float node_6171 = (InputTime*0.1);
                float2 Coord = float2(mul(unity_ObjectToWorld, v.vertex).r,mul(unity_ObjectToWorld, v.vertex).b);
                float2 node_2482 = ((node_6171+Coord)*0.05);
                float4 _node_2026_var = tex2Dlod(_node_2026,float4(TRANSFORM_TEX(node_2482, _node_2026),0.0,0));
                float3 InputPoint = float3(mul(unity_ObjectToWorld, v.vertex).r,_node_2026_var.g,mul(unity_ObjectToWorld, v.vertex).b);
                float4 node_5443 = float4(InputTime,_Angle,0.0,0.0);
                float3 node_3853 = MyFunction( InputPoint , node_5443 );
                float3 Offset = node_3853;
                v.vertex.xyz += Offset;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityMetaVertexPosition(v.vertex, v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST );
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : SV_Target {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float sceneZ = max(0,LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)))) - _ProjectionParams.g);
                float partZ = max(0,i.projPos.z - _ProjectionParams.g);
                UnityMetaInput o;
                UNITY_INITIALIZE_OUTPUT( UnityMetaInput, o );
                
                o.Emission = 0;
                
                float XCoord = i.posWorld.r;
                float4 node_2550 = _Time;
                float InputTime = node_2550.g;
                float node_6171 = (InputTime*0.1);
                float ZCoord = i.posWorld.b;
                float node_1896 = (0.05+ZCoord);
                float2 node_3944 = ((node_6171+float2(XCoord,node_1896))*0.05);
                float4 _node_2026_copy_copy_var = tex2D(_node_2026_copy_copy,TRANSFORM_TEX(node_3944, _node_2026_copy_copy));
                float3 InputZ = float3(XCoord,_node_2026_copy_copy_var.g,node_1896);
                float4 node_5443 = float4(InputTime,_Angle,0.0,0.0);
                float2 Coord = float2(i.posWorld.r,i.posWorld.b);
                float2 node_2482 = ((node_6171+Coord)*0.05);
                float4 _node_2026_var = tex2D(_node_2026,TRANSFORM_TEX(node_2482, _node_2026));
                float3 InputPoint = float3(i.posWorld.r,_node_2026_var.g,i.posWorld.b);
                float3 node_3853 = MyFunction( InputPoint , node_5443 );
                float node_6981 = (0.05+XCoord);
                float2 node_3605 = ((node_6171+float2(node_6981,ZCoord))*0.05);
                float4 _node_2026_copy_var = tex2D(_node_2026_copy,TRANSFORM_TEX(node_3605, _node_2026_copy));
                float3 InputX = float3(node_6981,_node_2026_copy_var.g,ZCoord);
                float3 Normal = normalize(cross(((YAxis( InputZ , node_5443 )+float3(0,0,0.1))-node_3853),((XAxis( InputX , node_5443 )+float3(0.1,0,0))-node_3853)));
                float3 node_5365 = Normal;
                float node_1413 = (InputTime*0.2);
                float2 node_9618 = float2((cos(node_1413)+XCoord),(sin(node_1413)+ZCoord));
                float4 _FoamTexture_var = tex2D(_FoamTexture,TRANSFORM_TEX(node_9618, _FoamTexture));
                float node_530 = 0.0;
                float3 diffColor = (pow(1.0-max(0,dot(node_5365, viewDirection)),_FresnelPower)+_Color.rgb+lerp((_FoamTexture_var.rgb+_FoamColor.rgb),float3(node_530,node_530,node_530),saturate((sceneZ-partZ)/_FoamDistance)));
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
