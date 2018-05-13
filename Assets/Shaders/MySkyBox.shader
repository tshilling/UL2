// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'glstate.matrix.mvp' with 'UNITY_MATRIX_MVP'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "RFX/Skybox3"
{
	Properties{
		_Color("Bottom Color", Color) = (1,1,1,1)
		_Color2("Top Color", Color) = (1,1,1,1)
		_Scale("Scale", Float) = 1
	}

		SubShader{
		Tags{ "Queue" = "Background" }

		Pass{
		ZWrite Off
		Cull Off

		CGPROGRAM
#include "UnityCG.cginc"
#pragma vertex vert
#pragma fragment frag

		// User-specified uniforms
		samplerCUBE _Cube;

	struct vertexInput {
		float4 vertex : POSITION;
		fixed4 col : COLOR;
	};

	struct vertexOutput {
		float4 vertex : SV_POSITION;
		float3 texcoord : TEXCOORD0;
	};

	vertexOutput vert(vertexInput input)
	{
		vertexOutput output;
		output.vertex = UnityObjectToClipPos(input.vertex);
		return output;
	}

	fixed4 frag(vertexOutput input) : COLOR
	{
		float3 Up = mul((float3x3)UNITY_MATRIX_V,float3(0,1,0));
		fixed4 C = fixed4(0,0,Up.y,0);
		return C;
	}
		ENDCG
	}
	}
}
/*
Camera right direction = UNITY_MATRIX_V[0].xyz = mul((float3x3)UNITY_MATRIX_V,float3(1,0,0));
Camera up direction = UNITY_MATRIX_V[1].xyz = mul((float3x3)UNITY_MATRIX_V,float3(0,1,0));
Camera forward direction = UNITY_MATRIX_V[2].xyz = mul((float3x3)UNITY_MATRIX_V,float3(0,0,1));
Camera position = _WorldSpaceCameraPos = mul(UNITY_MATRIX_V,float4(0,0,0,1)).xyz; 
*/