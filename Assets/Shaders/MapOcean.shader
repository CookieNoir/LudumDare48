Shader "Unlit/MapOcean"
{
	Properties
	{
		_Color("Color", color) = (1,1,1,1)
		_MainTex("Texture", 2D) = "white" {}
		_BackgroundTex("Background Texture", 2D) = "white" {}
		_LightTex("Mask", 2D) = "black" {}
		_MaskTex("Light Texture", 2D) = "white" {}

	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
				float2 uv2 : TEXCOORD2;
				float2 uv3 : TEXCOORD3;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _BackgroundTex;
			float4 _BackgroundTex_ST;
			sampler2D _LightTex;
			float4 _LightTex_ST;
			sampler2D _MaskTex;
			float4 _MaskTex_ST;
			fixed4 _Color;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv1 = TRANSFORM_TEX(v.uv, _BackgroundTex);
				o.uv2 = TRANSFORM_TEX(v.uv, _LightTex);
				o.uv3 = TRANSFORM_TEX(v.uv, _MaskTex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed m = tex2D(_MainTex, i.uv).r;
				fixed4 b = tex2D(_BackgroundTex, i.uv1) + tex2D(_LightTex, i.uv2);
				fixed a = tex2D(_MaskTex, i.uv3).r * (1.0 - m);
				fixed4 col = _Color * a + b * (1.0 - a);
				return col;
			}
			ENDCG
		}
	}
}
