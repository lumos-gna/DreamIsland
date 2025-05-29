Shader "Custom/HeatDistortion"
{
      Properties
    {
        _MainTex ("Distortion Map", 2D) = "white" {}
        _DistortionStrength ("Distortion Strength", Range(0, 0.1)) = 0.02
        _ScrollSpeed ("Scroll Speed", Float) = 0.2
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        GrabPass { }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _GrabTexture;
            float _DistortionStrength;
            float _ScrollSpeed;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
                fixed4 color : COLOR;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.screenPos = o.pos;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 scrollUV = i.uv + float2(_Time.y * _ScrollSpeed, _Time.y * _ScrollSpeed * 0.5);
                float2 noise = tex2D(_MainTex, scrollUV).rg;

                float2 distortion = (noise - 0.5) * _DistortionStrength;

                float2 uv = i.screenPos.xy / i.screenPos.w;
                uv = uv * 0.5 + 0.5;
                uv += distortion;

                fixed4 col = tex2D(_GrabTexture, uv);
                col.a *= i.color.a;

                return col;
            }
            ENDCG
        }
    }
}
