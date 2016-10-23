Shader "Cartoon FX/Alpha Blended Tint"
{
	Properties
	{
		_MainTex ("Particle Texture", 2D) = "white" {}
	}

	Category
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		AlphaTest Greater .01
		ColorMask RGB
		Cull Off Lighting Off ZWrite Off
		BindChannels
		{
			Bind "Color", color
			Bind "Vertex", vertex
			Bind "TexCoord", texcoord
		}
	
		// ---- Fragment program cards
		SubShader
		{
			Pass
			{
		
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma multi_compile_particles
			
				#include "UnityCG.cginc"

				sampler2D _MainTex;
			
				struct appdata_t
				{
					float4 vertex : POSITION;
					float3 normal : NORMAL;
					fixed4 color : COLOR;
					float2 texcoord : TEXCOORD0;
				};
			
				struct v2f
				{
					float4 vertex : POSITION;
					float3 normal : TEXCOORD3;
					fixed4 color : COLOR;
					float2 texcoord : TEXCOORD0;
				};
			
				float4 _MainTex_ST;
			
				v2f vert (appdata_t v)
				{
					v2f o;
					o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
					o.color = v.color;
					o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
					o.normal = v.normal;
				
					return o;
				}
			
				sampler2D _CameraDepthTexture;
			
				fixed4 frag (v2f i) : COLOR
				{
					return 2.0f * i.color * tex2D(_MainTex, i.texcoord).a;
				}
				ENDCG 
			}
		} 	
	
		// ---- Single texture cards (does not do color tint)
		SubShader
		{
			Pass
			{
				SetTexture [_MainTex]
				{
					combine texture * primary
				}
			}
		}
	}
}