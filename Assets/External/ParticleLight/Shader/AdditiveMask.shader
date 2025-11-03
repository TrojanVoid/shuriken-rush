Shader "Custom/AdditiveMask_URP_Emissive"
{
    Properties
    {
        _BaseMap("Base (RGB)", 2D) = "white" {}
        _Mask("Culling Mask", 2D) = "white" {}
        _Color("Main Color", Color) = (1,1,1,1)
        _EmissionColor("Emission Color (HDR)", Color) = (5,5,5,1) // high default intensity
        _EmissionIntensity("Emission Intensity", Range(0,10)) = 2
        _Cutoff("Alpha Cutoff", Range(0.01,1)) = 0.1
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            Name "ForwardLit"
            Tags{"LightMode"="UniversalForward"}

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _BaseMap;
            sampler2D _Mask;
            float4 _BaseMap_ST;
            float4 _Mask_ST;
            float4 _Color;
            float4 _EmissionColor;
            float _EmissionIntensity;
            float _Cutoff;

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                half4 baseCol = tex2D(_BaseMap, TRANSFORM_TEX(IN.uv, _BaseMap));
                half4 maskCol = tex2D(_Mask, TRANSFORM_TEX(IN.uv, _Mask));

                half alpha = saturate(baseCol.a * maskCol.a * _Color.a);
                clip(alpha - _Cutoff);

                half3 color = baseCol.rgb * maskCol.rgb * _Color.rgb;

                // explicit HDR emission
                half3 emissive = color * _EmissionColor.rgb * _EmissionIntensity;

                // final output color >1.0 triggers URP bloom
                return half4(emissive, alpha);
            }
            ENDHLSL
        }

    }

    FallBack Off
}
