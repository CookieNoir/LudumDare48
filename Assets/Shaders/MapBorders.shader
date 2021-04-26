Shader "Unlit/MapBorders"
{
    Properties
    {
		_Color("Color", color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
		_MaskTex ("Mask", 2D) = "black" {}
		_LightTex ("Mask", 2D) = "black" {}
    }
    SubShader
    {
		Tags {"Queue" = "Transparent" "RenderType" = "Transparent"}
		Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
				fixed4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
				fixed4 color : COLOR;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			sampler2D _MaskTex;
			float4 _MaskTex_ST;
			sampler2D _LightTex;
			float4 _LightTex_ST;
			fixed4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv1 = TRANSFORM_TEX(v.uv, _MaskTex);
				o.uv2 = TRANSFORM_TEX(v.uv, _LightTex);
				o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				fixed mask = tex2D(_MaskTex, i.uv1).r;
                fixed4 col = _Color * tex2D(_MainTex, i.uv) + tex2D(_LightTex, i.uv2);
				col.a = lerp(i.color.g * i.color.g, lerp(0.0, 1.0, step(mask * mask, i.color.r + 0.01f)), step(1.0, i.color.g));
                return col;
            }
            ENDCG
        }
    }
}
