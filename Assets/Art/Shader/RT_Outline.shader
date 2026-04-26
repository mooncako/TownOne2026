Shader "TownOne/Rendering/Scene Outline"
{
    Properties
    {
        _OutlineColor("Outline Color", Color) = (0, 0, 0, 1)
        _OutlineThickness("Outline Thickness (Pixels)", Range(0.5, 8)) = 1.5
        _DepthThreshold("Depth Threshold", Range(0.0001, 5)) = 0.25
        _DepthSensitivity("Depth Sensitivity", Range(0.1, 8)) = 2
        _NormalThreshold("Normal Threshold", Range(0.001, 1)) = 0.12
        _NormalSensitivity("Normal Sensitivity", Range(0.1, 8)) = 2
        _OutlineOpacity("Outline Opacity", Range(0, 1)) = 1
    }

    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" "Queue" = "Overlay" }
        ZWrite Off
        ZTest Always
        Cull Off
        Blend One Zero

        Pass
        {
            Name "SceneOutline"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #pragma target 4.5

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareNormalsTexture.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _OutlineColor;
                float _OutlineThickness;
                float _DepthThreshold;
                float _DepthSensitivity;
                float _NormalThreshold;
                float _NormalSensitivity;
                float _OutlineOpacity;
            CBUFFER_END

            bool IsBackground(float rawDepth)
            {
                #if UNITY_REVERSED_Z
                    return rawDepth <= 0.0001;
                #else
                    return rawDepth >= 0.9999;
                #endif
            }

            float SampleLinearEyeDepth(float2 uv, out bool isBackground)
            {
                float rawDepth = SampleSceneDepth(uv);
                isBackground = IsBackground(rawDepth);
                if (isBackground)
                {
                    return _ProjectionParams.z;
                }

                return LinearEyeDepth(rawDepth, _ZBufferParams);
            }

            float DepthEdge(float centerDepth, bool centerIsBackground, float2 uv, float2 offset)
            {
                float maxDifference = 0.0;

                const float2 taps[4] =
                {
                    float2(1.0, 0.0),
                    float2(-1.0, 0.0),
                    float2(0.0, 1.0),
                    float2(0.0, -1.0)
                };

                [unroll]
                for (int i = 0; i < 4; i++)
                {
                    bool sampleIsBackground;
                    float sampleDepth = SampleLinearEyeDepth(uv + taps[i] * offset, sampleIsBackground);
                    float difference = abs(sampleDepth - centerDepth);

                    if (sampleIsBackground != centerIsBackground)
                    {
                        difference = max(difference, _ProjectionParams.z);
                    }

                    maxDifference = max(maxDifference, difference);
                }

                float threshold = max(_DepthThreshold, 0.0001);
                return smoothstep(threshold, threshold * _DepthSensitivity, maxDifference);
            }

            float NormalEdge(float3 centerNormal, float2 uv, float2 offset)
            {
                float maxDifference = 0.0;

                const float2 taps[4] =
                {
                    float2(1.0, 0.0),
                    float2(-1.0, 0.0),
                    float2(0.0, 1.0),
                    float2(0.0, -1.0)
                };

                [unroll]
                for (int i = 0; i < 4; i++)
                {
                    float3 sampleNormal = SampleSceneNormals(uv + taps[i] * offset);
                    maxDifference = max(maxDifference, length(sampleNormal - centerNormal));
                }

                float threshold = max(_NormalThreshold, 0.0001);
                return smoothstep(threshold, threshold * _NormalSensitivity, maxDifference);
            }

            half4 Frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                float2 uv = input.texcoord;
                float2 pixelOffset = _BlitTexture_TexelSize.xy * _OutlineThickness;

                half4 source = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, uv);

                bool centerIsBackground;
                float centerDepth = SampleLinearEyeDepth(uv, centerIsBackground);
                float3 centerNormal = SampleSceneNormals(uv);

                float depthEdge = DepthEdge(centerDepth, centerIsBackground, uv, pixelOffset);
                float normalEdge = NormalEdge(centerNormal, uv, pixelOffset);
                float edge = saturate(max(depthEdge, normalEdge) * _OutlineOpacity * _OutlineColor.a);

                return lerp(source, half4(_OutlineColor.rgb, 1.0), edge);
            }
            ENDHLSL
        }
    }
}
