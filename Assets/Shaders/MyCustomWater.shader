Shader "RFX/MyCustomWater" 
{
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
	_NormalTex("Albedo (RGB)", 2D) = "white" {}
	_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
	}
	SubShader
	{
		Tags{
		"IgnoreProjector" = "True"
		"Queue" = "Transparent"
		"RenderType" = "Transparent"
		}
			Blend SrcAlpha OneMinusSrcAlpha
			//ZWrite Off
			LOD 300
			CGPROGRAM
			#include "UnityCG.cginc"
			// Physically based Standard lighting model, and enable shadows on all light types
			#pragma surface surf Standard fullforwardshadows vertex:vert

			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0

			sampler2D _MainTex;
			sampler2D _NormalTex;
			uniform float4 _FinalColor;
			uniform sampler2D _CameraDepthTexture;

			struct Input {
				float2 uv_MainTex;
				float2 uv_NormalTex;
				float3 worldPos;
				float3 normal;
				float4 pos : SV_POSITION;
				float4 projPos : TEXCOORD0;
				float4 screenPos;
			};

			half _Glossiness;
			half _Metallic;
			fixed4 _Color;

			UNITY_INSTANCING_BUFFER_START(Props)
			UNITY_INSTANCING_BUFFER_END(Props)

			void vert(inout appdata_full v, out Input result) {
				UNITY_INITIALIZE_OUTPUT(Input, result);
				float4 p = float4(v.vertex.x, v.vertex.y, v.vertex.z, 1);
				float4 pntInWorld = mul(unity_ObjectToWorld, p);
				float4 delta = float4(pntInWorld.x / 100 + _Time.x / 10, pntInWorld.z / 100,0,0);
				float3 offset = tex2Dlod(_MainTex, delta);
				v.normal = normalize(v.normal);

				float backgroundDepth = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dlod(_CameraDepthTexture, p)));
				backgroundDepth *= backgroundDepth;
				backgroundDepth -= ((v.vertex.x*v.vertex.x) + (v.vertex.y*v.vertex.y) + (v.vertex.z*v.vertex.z));

				float4 pos = UnityObjectToClipPos(v.vertex);
				float4 projPos = ComputeScreenPos(pos);
				COMPUTE_EYEDEPTH(projPos.z);
				float4 projCoord = UNITY_PROJ_COORD(projPos);
				float sceneZ = max(0, LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dlod(_CameraDepthTexture, projCoord))) - _ProjectionParams.g);
				float partZ = max(0, projPos.z - _ProjectionParams.g);

				result.pos = UnityObjectToClipPos(v.vertex);
				result.projPos = ComputeScreenPos(result.pos);
				COMPUTE_EYEDEPTH(result.projPos.z);


				//v.vertex.z += offset.z;
			}
			void surf(Input IN, inout SurfaceOutputStandard o) {
				//float sceneZ = max(0, LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, IN.screenPos))) - _ProjectionParams.g);

			float sceneZ = max(0, LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, UNITY_PROJ_COORD(IN.projPos)))) - _ProjectionParams.g);
				float partZ = max(0, IN.projPos.z - _ProjectionParams.g);
				fixed4 output = fixed4(_Color.r,_Color.g,_Color.b, saturate((sceneZ - partZ)));

				float4 delta = float4(IN.worldPos.x / 100 + _Time.x / 10, IN.worldPos.z / 100, 0, 0);
				float4 delta2 = float4(IN.worldPos.z / 5 , IN.worldPos.x / 10 + _Time.x / 10, 0, 0);

				fixed4 n = tex2D(_NormalTex, delta.xy);
				n*= tex2D(_NormalTex, delta2.xy);
				o.Albedo = output.rgb;// c.rgb;
				o.Normal = normalize(n.xyz);
				o.Metallic = _Metallic;
				o.Smoothness = _Glossiness;
				o.Alpha = output.a;
			}
			ENDCG
		}
	
	FallBack "Diffuse"
}

