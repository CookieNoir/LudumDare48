Shader "Unlit/BorderLine"
{
    Properties
    {
		_Color("Color", color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
		_Threshold("Threshold", Range(0.0, 1.0)) = 0.1
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
				fixed4 color : COLOR;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			fixed4 _Color;
			float _Threshold;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = lerp(tex2D(_MainTex, i.uv) * _Color, _Color, step(1.0 - 2.0 * abs(i.color.g - 0.5), _Threshold));
                return col;
            }
            ENDCG
        }
    }
}
