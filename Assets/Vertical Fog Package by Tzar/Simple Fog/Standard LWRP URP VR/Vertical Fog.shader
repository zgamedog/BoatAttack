Shader "Tzar/VerticalFog"
{
    Properties
    {
       _Color("Main Color", Color) = (1, 1, 1, .5)
       _Intensity("Intensity", float) = 1
         _ColorMul("_ColorMul", float) = 1
         		_MainTex("Main Texture", 2D) = "white" {}
    }
        SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent"  }

        Pass
        {
           Blend SrcAlpha OneMinusSrcAlpha
           ZWrite Off
           CGPROGRAM
           #pragma vertex vert
           #pragma fragment frag
           #pragma multi_compile_fog
           #include "UnityCG.cginc"

           struct appdata
           {
               float4 vertex : POSITION;
                float2 uv : TEXCOORD1;
               UNITY_VERTEX_INPUT_INSTANCE_ID
           };

           struct v2f
           {
               float4 scrPos : TEXCOORD0;
               		float2 uv : TEXCOORD1;
               UNITY_FOG_COORDS(1)
               float4 vertex : SV_POSITION;
               UNITY_VERTEX_OUTPUT_STEREO
           };



           UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
           float4 _Color;
           float _Intensity;
            float _ColorMul;
  	        sampler2D _MainTex;
float4 _MainTex_ST;
           v2f vert(appdata v)
           {
               v2f o;

               UNITY_SETUP_INSTANCE_ID(v);
               UNITY_INITIALIZE_OUTPUT(v2f, o);
               UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
               o.vertex = UnityObjectToClipPos(v.vertex);
               o.scrPos = ComputeScreenPos(o.vertex);
               UNITY_TRANSFER_FOG(o,o.vertex);
               return o;
           }


            half4 frag(v2f i) : SV_TARGET
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

               float depth = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.scrPos)));
               float diff = saturate(_Intensity * (depth - i.scrPos.w));

               fixed4 col = lerp(fixed4(_Color.rgb, 0.0), _Color, diff * diff * diff * (diff * (6 * diff - 15) + 10));
                col *= _ColorMul;

                fixed4 mcol = tex2D(_MainTex, i.uv);

                col.a *= mcol.a;

               UNITY_APPLY_FOG(i.fogCoord, col);
               return col;
            }

            ENDCG
        }
    }
}