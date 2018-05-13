// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'glstate.matrix.mvp' with 'UNITY_MATRIX_MVP'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "RFX/Skybox3" 
{
	Properties{
		_Cube("Texture", Cube) = "white" {}
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
			float3 texcoord : TEXCOORD0;
		};

		struct vertexOutput {
			float4 vertex : SV_POSITION;
			float3 texcoord : TEXCOORD0;
		};

		vertexOutput vert(vertexInput input)
		{
			vertexOutput output;
			output.vertex = UnityObjectToClipPos(input.vertex);
			output.texcoord = input.texcoord;
			return output;
		}

		fixed4 frag(vertexOutput input) : COLOR
		{
			float4 v = mul(unity_CameraInvProjection,float4(0,1,0,0));
			float3 v2 = UNITY_MATRIX_IT_MV[2].xyz
			fixed4 C = fixed4(0,0,v2.y,0);
			return C;
		}
		ENDCG
		}
	}
}