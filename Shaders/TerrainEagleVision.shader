Shader "TerrainEagleVision"
{
    Properties
    {
        Color_8f36033cb7114db69f9e92e4813eeed3("Color", Color) = (0, 0, 0, 0)
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
    }
        SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Opaque"
            "UniversalMaterialType" = "Unlit"
            "Queue" = "AlphaTest"
        }
        Pass
        {
            Name "Pass"
            Tags
            {
        // LightMode: <None>
    }

    // Render State
    Cull Back
    Blend One Zero
    ZTest LEqual
    ZWrite On

        // Debug
        // <None>

        // --------------------------------------------------
        // Pass

        HLSLPROGRAM

        // Pragmas
        #pragma target 2.0
        #pragma only_renderers gles gles3 glcore
        #pragma multi_compile_instancing
        #pragma multi_compile_fog
        #pragma vertex vert
        #pragma fragment frag

        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>

        // Keywords
        #pragma multi_compile _ LIGHTMAP_ON
        #pragma multi_compile _ DIRLIGHTMAP_COMBINED
        #pragma shader_feature _ _SAMPLE_GI
        // GraphKeywords: <None>

        // Defines
        #define _AlphaClip 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_VIEWDIRECTION_WS
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_UNLIT
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

        // --------------------------------------------------
        // Structs and Packing

        struct Attributes
        {
            float3 positionOS : POSITION;
            float3 normalOS : NORMAL;
            float4 tangentOS : TANGENT;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
            float4 positionCS : SV_POSITION;
            float3 positionWS;
            float3 normalWS;
            float3 viewDirectionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
            float3 WorldSpaceNormal;
            float3 WorldSpaceViewDirection;
            float3 WorldSpacePosition;
            float4 ScreenPosition;
        };
        struct VertexDescriptionInputs
        {
            float3 ObjectSpaceNormal;
            float3 ObjectSpaceTangent;
            float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
            float4 positionCS : SV_POSITION;
            float3 interp0 : TEXCOORD0;
            float3 interp1 : TEXCOORD1;
            float3 interp2 : TEXCOORD2;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };

        PackedVaryings PackVaryings(Varyings input)
        {
            PackedVaryings output;
            output.positionCS = input.positionCS;
            output.interp0.xyz = input.positionWS;
            output.interp1.xyz = input.normalWS;
            output.interp2.xyz = input.viewDirectionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        Varyings UnpackVaryings(PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.interp0.xyz;
            output.normalWS = input.interp1.xyz;
            output.viewDirectionWS = input.interp2.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }

        // --------------------------------------------------
        // Graph

        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 Color_8f36033cb7114db69f9e92e4813eeed3;
        CBUFFER_END

            // Object and Global properties

            // Graph Functions

            void Unity_FresnelEffect_float(float3 Normal, float3 ViewDir, float Power, out float Out)
            {
                Out = pow((1.0 - saturate(dot(normalize(Normal), normalize(ViewDir)))), Power);
            }

            void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
            {
                Out = A * B;
            }

            void Unity_Dither_float(float In, float4 ScreenPosition, out float Out)
            {
                float2 uv = ScreenPosition.xy * _ScreenParams.xy;
                float DITHER_THRESHOLDS[16] =
                {
                    1.0 / 17.0,  9.0 / 17.0,  3.0 / 17.0, 11.0 / 17.0,
                    13.0 / 17.0,  5.0 / 17.0, 15.0 / 17.0,  7.0 / 17.0,
                    4.0 / 17.0, 12.0 / 17.0,  2.0 / 17.0, 10.0 / 17.0,
                    16.0 / 17.0,  8.0 / 17.0, 14.0 / 17.0,  6.0 / 17.0
                };
                uint index = (uint(uv.x) % 4) * 4 + uint(uv.y) % 4;
                Out = In - DITHER_THRESHOLDS[index];
            }

            // Graph Vertex
            struct VertexDescription
            {
                float3 Position;
                float3 Normal;
                float3 Tangent;
            };

            VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
            {
                VertexDescription description = (VertexDescription)0;
                description.Position = IN.ObjectSpacePosition;
                description.Normal = IN.ObjectSpaceNormal;
                description.Tangent = IN.ObjectSpaceTangent;
                return description;
            }

            // Graph Pixel
            struct SurfaceDescription
            {
                float3 BaseColor;
                float Alpha;
                float AlphaClipThreshold;
            };

            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
            {
                SurfaceDescription surface = (SurfaceDescription)0;
                float4 Color_ac0eb3d9c9ea4a5f94933ba4a6283731 = IsGammaSpace() ? float4(1, 1, 1, 0) : float4(SRGBToLinear(float3(1, 1, 1)), 0);
                float _FresnelEffect_77c17e76b9e047b29a574609ba5103b9_Out_3;
                Unity_FresnelEffect_float(IN.WorldSpaceNormal, IN.WorldSpaceViewDirection, 1.39, _FresnelEffect_77c17e76b9e047b29a574609ba5103b9_Out_3);
                float4 _Multiply_73fd6e1b518e4e67b22f9e96685d69d7_Out_2;
                Unity_Multiply_float(Color_ac0eb3d9c9ea4a5f94933ba4a6283731, (_FresnelEffect_77c17e76b9e047b29a574609ba5103b9_Out_3.xxxx), _Multiply_73fd6e1b518e4e67b22f9e96685d69d7_Out_2);
                float _Dither_dc95ad6a6a134ac980e7a26b0766f861_Out_2;
                Unity_Dither_float(1, float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0), _Dither_dc95ad6a6a134ac980e7a26b0766f861_Out_2);
                surface.BaseColor = (_Multiply_73fd6e1b518e4e67b22f9e96685d69d7_Out_2.xyz);
                surface.Alpha = _Dither_dc95ad6a6a134ac980e7a26b0766f861_Out_2;
                surface.AlphaClipThreshold = 0;
                return surface;
            }

            // --------------------------------------------------
            // Build Graph Inputs

            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
            {
                VertexDescriptionInputs output;
                ZERO_INITIALIZE(VertexDescriptionInputs, output);

                output.ObjectSpaceNormal = input.normalOS;
                output.ObjectSpaceTangent = input.tangentOS;
                output.ObjectSpacePosition = input.positionOS;

                return output;
            }

            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
            {
                SurfaceDescriptionInputs output;
                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

                // must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
                float3 unnormalizedNormalWS = input.normalWS;
                const float renormFactor = 1.0 / length(unnormalizedNormalWS);


                output.WorldSpaceNormal = renormFactor * input.normalWS.xyz;		// we want a unit length Normal Vector node in shader graph


                output.WorldSpaceViewDirection = input.viewDirectionWS; //TODO: by default normalized in HD, but not in universal
                output.WorldSpacePosition = input.positionWS;
                output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
            #else
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            #endif
            #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                return output;
            }


            // --------------------------------------------------
            // Main

            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/UnlitPass.hlsl"

            ENDHLSL
        }
        Pass
        {
            Name "ShadowCaster"
            Tags
            {
                "LightMode" = "ShadowCaster"
            }

                // Render State
                Cull Back
                Blend One Zero
                ZTest LEqual
                ZWrite On
                ColorMask 0

                // Debug
                // <None>

                // --------------------------------------------------
                // Pass

                HLSLPROGRAM

                // Pragmas
                #pragma target 2.0
                #pragma only_renderers gles gles3 glcore
                #pragma multi_compile_instancing
                #pragma vertex vert
                #pragma fragment frag

                // DotsInstancingOptions: <None>
                // HybridV1InjectedBuiltinProperties: <None>

                // Keywords
                // PassKeywords: <None>
                // GraphKeywords: <None>

                // Defines
                #define _AlphaClip 1
                #define ATTRIBUTES_NEED_NORMAL
                #define ATTRIBUTES_NEED_TANGENT
                #define VARYINGS_NEED_POSITION_WS
                #define FEATURES_GRAPH_VERTEX
                /* WARNING: $splice Could not find named fragment 'PassInstancing' */
                #define SHADERPASS SHADERPASS_SHADOWCASTER
                /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

                // Includes
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

                // --------------------------------------------------
                // Structs and Packing

                struct Attributes
                {
                    float3 positionOS : POSITION;
                    float3 normalOS : NORMAL;
                    float4 tangentOS : TANGENT;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : INSTANCEID_SEMANTIC;
                    #endif
                };
                struct Varyings
                {
                    float4 positionCS : SV_POSITION;
                    float3 positionWS;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                    #endif
                };
                struct SurfaceDescriptionInputs
                {
                    float3 WorldSpacePosition;
                    float4 ScreenPosition;
                };
                struct VertexDescriptionInputs
                {
                    float3 ObjectSpaceNormal;
                    float3 ObjectSpaceTangent;
                    float3 ObjectSpacePosition;
                };
                struct PackedVaryings
                {
                    float4 positionCS : SV_POSITION;
                    float3 interp0 : TEXCOORD0;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                    #endif
                };

                PackedVaryings PackVaryings(Varyings input)
                {
                    PackedVaryings output;
                    output.positionCS = input.positionCS;
                    output.interp0.xyz = input.positionWS;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    return output;
                }
                Varyings UnpackVaryings(PackedVaryings input)
                {
                    Varyings output;
                    output.positionCS = input.positionCS;
                    output.positionWS = input.interp0.xyz;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    return output;
                }

                // --------------------------------------------------
                // Graph

                // Graph Properties
                CBUFFER_START(UnityPerMaterial)
                float4 Color_8f36033cb7114db69f9e92e4813eeed3;
                CBUFFER_END

                    // Object and Global properties

                    // Graph Functions

                    void Unity_Dither_float(float In, float4 ScreenPosition, out float Out)
                    {
                        float2 uv = ScreenPosition.xy * _ScreenParams.xy;
                        float DITHER_THRESHOLDS[16] =
                        {
                            1.0 / 17.0,  9.0 / 17.0,  3.0 / 17.0, 11.0 / 17.0,
                            13.0 / 17.0,  5.0 / 17.0, 15.0 / 17.0,  7.0 / 17.0,
                            4.0 / 17.0, 12.0 / 17.0,  2.0 / 17.0, 10.0 / 17.0,
                            16.0 / 17.0,  8.0 / 17.0, 14.0 / 17.0,  6.0 / 17.0
                        };
                        uint index = (uint(uv.x) % 4) * 4 + uint(uv.y) % 4;
                        Out = In - DITHER_THRESHOLDS[index];
                    }

                // Graph Vertex
                struct VertexDescription
                {
                    float3 Position;
                    float3 Normal;
                    float3 Tangent;
                };

                VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                {
                    VertexDescription description = (VertexDescription)0;
                    description.Position = IN.ObjectSpacePosition;
                    description.Normal = IN.ObjectSpaceNormal;
                    description.Tangent = IN.ObjectSpaceTangent;
                    return description;
                }

                // Graph Pixel
                struct SurfaceDescription
                {
                    float Alpha;
                    float AlphaClipThreshold;
                };

                SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                {
                    SurfaceDescription surface = (SurfaceDescription)0;
                    float _Dither_dc95ad6a6a134ac980e7a26b0766f861_Out_2;
                    Unity_Dither_float(1, float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0), _Dither_dc95ad6a6a134ac980e7a26b0766f861_Out_2);
                    surface.Alpha = _Dither_dc95ad6a6a134ac980e7a26b0766f861_Out_2;
                    surface.AlphaClipThreshold = 0;
                    return surface;
                }

                // --------------------------------------------------
                // Build Graph Inputs

                VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                {
                    VertexDescriptionInputs output;
                    ZERO_INITIALIZE(VertexDescriptionInputs, output);

                    output.ObjectSpaceNormal = input.normalOS;
                    output.ObjectSpaceTangent = input.tangentOS;
                    output.ObjectSpacePosition = input.positionOS;

                    return output;
                }

                SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                {
                    SurfaceDescriptionInputs output;
                    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





                    output.WorldSpacePosition = input.positionWS;
                    output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                #else
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                #endif
                #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                    return output;
                }


                // --------------------------------------------------
                // Main

                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShadowCasterPass.hlsl"

                ENDHLSL
            }
            Pass
            {
                Name "DepthOnly"
                Tags
                {
                    "LightMode" = "DepthOnly"
                }

                    // Render State
                    Cull Back
                    Blend One Zero
                    ZTest LEqual
                    ZWrite On
                    ColorMask 0

                    // Debug
                    // <None>

                    // --------------------------------------------------
                    // Pass

                    HLSLPROGRAM

                    // Pragmas
                    #pragma target 2.0
                    #pragma only_renderers gles gles3 glcore
                    #pragma multi_compile_instancing
                    #pragma vertex vert
                    #pragma fragment frag

                    // DotsInstancingOptions: <None>
                    // HybridV1InjectedBuiltinProperties: <None>

                    // Keywords
                    // PassKeywords: <None>
                    // GraphKeywords: <None>

                    // Defines
                    #define _AlphaClip 1
                    #define ATTRIBUTES_NEED_NORMAL
                    #define ATTRIBUTES_NEED_TANGENT
                    #define VARYINGS_NEED_POSITION_WS
                    #define FEATURES_GRAPH_VERTEX
                    /* WARNING: $splice Could not find named fragment 'PassInstancing' */
                    #define SHADERPASS SHADERPASS_DEPTHONLY
                    /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

                    // Includes
                    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
                    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

                    // --------------------------------------------------
                    // Structs and Packing

                    struct Attributes
                    {
                        float3 positionOS : POSITION;
                        float3 normalOS : NORMAL;
                        float4 tangentOS : TANGENT;
                        #if UNITY_ANY_INSTANCING_ENABLED
                        uint instanceID : INSTANCEID_SEMANTIC;
                        #endif
                    };
                    struct Varyings
                    {
                        float4 positionCS : SV_POSITION;
                        float3 positionWS;
                        #if UNITY_ANY_INSTANCING_ENABLED
                        uint instanceID : CUSTOM_INSTANCE_ID;
                        #endif
                        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                        uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                        #endif
                        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                        uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                        #endif
                        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                        FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                        #endif
                    };
                    struct SurfaceDescriptionInputs
                    {
                        float3 WorldSpacePosition;
                        float4 ScreenPosition;
                    };
                    struct VertexDescriptionInputs
                    {
                        float3 ObjectSpaceNormal;
                        float3 ObjectSpaceTangent;
                        float3 ObjectSpacePosition;
                    };
                    struct PackedVaryings
                    {
                        float4 positionCS : SV_POSITION;
                        float3 interp0 : TEXCOORD0;
                        #if UNITY_ANY_INSTANCING_ENABLED
                        uint instanceID : CUSTOM_INSTANCE_ID;
                        #endif
                        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                        uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                        #endif
                        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                        uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                        #endif
                        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                        FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                        #endif
                    };

                    PackedVaryings PackVaryings(Varyings input)
                    {
                        PackedVaryings output;
                        output.positionCS = input.positionCS;
                        output.interp0.xyz = input.positionWS;
                        #if UNITY_ANY_INSTANCING_ENABLED
                        output.instanceID = input.instanceID;
                        #endif
                        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                        output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                        #endif
                        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                        output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                        #endif
                        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                        output.cullFace = input.cullFace;
                        #endif
                        return output;
                    }
                    Varyings UnpackVaryings(PackedVaryings input)
                    {
                        Varyings output;
                        output.positionCS = input.positionCS;
                        output.positionWS = input.interp0.xyz;
                        #if UNITY_ANY_INSTANCING_ENABLED
                        output.instanceID = input.instanceID;
                        #endif
                        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                        output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                        #endif
                        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                        output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                        #endif
                        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                        output.cullFace = input.cullFace;
                        #endif
                        return output;
                    }

                    // --------------------------------------------------
                    // Graph

                    // Graph Properties
                    CBUFFER_START(UnityPerMaterial)
                    float4 Color_8f36033cb7114db69f9e92e4813eeed3;
                    CBUFFER_END

                        // Object and Global properties

                        // Graph Functions

                        void Unity_Dither_float(float In, float4 ScreenPosition, out float Out)
                        {
                            float2 uv = ScreenPosition.xy * _ScreenParams.xy;
                            float DITHER_THRESHOLDS[16] =
                            {
                                1.0 / 17.0,  9.0 / 17.0,  3.0 / 17.0, 11.0 / 17.0,
                                13.0 / 17.0,  5.0 / 17.0, 15.0 / 17.0,  7.0 / 17.0,
                                4.0 / 17.0, 12.0 / 17.0,  2.0 / 17.0, 10.0 / 17.0,
                                16.0 / 17.0,  8.0 / 17.0, 14.0 / 17.0,  6.0 / 17.0
                            };
                            uint index = (uint(uv.x) % 4) * 4 + uint(uv.y) % 4;
                            Out = In - DITHER_THRESHOLDS[index];
                        }

                    // Graph Vertex
                    struct VertexDescription
                    {
                        float3 Position;
                        float3 Normal;
                        float3 Tangent;
                    };

                    VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                    {
                        VertexDescription description = (VertexDescription)0;
                        description.Position = IN.ObjectSpacePosition;
                        description.Normal = IN.ObjectSpaceNormal;
                        description.Tangent = IN.ObjectSpaceTangent;
                        return description;
                    }

                    // Graph Pixel
                    struct SurfaceDescription
                    {
                        float Alpha;
                        float AlphaClipThreshold;
                    };

                    SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                    {
                        SurfaceDescription surface = (SurfaceDescription)0;
                        float _Dither_dc95ad6a6a134ac980e7a26b0766f861_Out_2;
                        Unity_Dither_float(1, float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0), _Dither_dc95ad6a6a134ac980e7a26b0766f861_Out_2);
                        surface.Alpha = _Dither_dc95ad6a6a134ac980e7a26b0766f861_Out_2;
                        surface.AlphaClipThreshold = 0;
                        return surface;
                    }

                    // --------------------------------------------------
                    // Build Graph Inputs

                    VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                    {
                        VertexDescriptionInputs output;
                        ZERO_INITIALIZE(VertexDescriptionInputs, output);

                        output.ObjectSpaceNormal = input.normalOS;
                        output.ObjectSpaceTangent = input.tangentOS;
                        output.ObjectSpacePosition = input.positionOS;

                        return output;
                    }

                    SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                    {
                        SurfaceDescriptionInputs output;
                        ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





                        output.WorldSpacePosition = input.positionWS;
                        output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                    #else
                    #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                    #endif
                    #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                        return output;
                    }


                    // --------------------------------------------------
                    // Main

                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthOnlyPass.hlsl"

                    ENDHLSL
                }
    }
        SubShader
                    {
                        Tags
                        {
                            "RenderPipeline" = "UniversalPipeline"
                            "RenderType" = "Opaque"
                            "UniversalMaterialType" = "Unlit"
                            "Queue" = "AlphaTest"
                        }
                        Pass
                        {
                            Name "Pass"
                            Tags
                            {
                        // LightMode: <None>
                    }

                    // Render State
                    Cull Back
                    Blend One Zero
                    ZTest LEqual
                    ZWrite On

                        // Debug
                        // <None>

                        // --------------------------------------------------
                        // Pass

                        HLSLPROGRAM

                        // Pragmas
                        #pragma target 4.5
                        #pragma exclude_renderers gles gles3 glcore
                        #pragma multi_compile_instancing
                        #pragma multi_compile_fog
                        #pragma multi_compile _ DOTS_INSTANCING_ON
                        #pragma vertex vert
                        #pragma fragment frag

                        // DotsInstancingOptions: <None>
                        // HybridV1InjectedBuiltinProperties: <None>

                        // Keywords
                        #pragma multi_compile _ LIGHTMAP_ON
                        #pragma multi_compile _ DIRLIGHTMAP_COMBINED
                        #pragma shader_feature _ _SAMPLE_GI
                        // GraphKeywords: <None>

                        // Defines
                        #define _AlphaClip 1
                        #define ATTRIBUTES_NEED_NORMAL
                        #define ATTRIBUTES_NEED_TANGENT
                        #define VARYINGS_NEED_POSITION_WS
                        #define VARYINGS_NEED_NORMAL_WS
                        #define VARYINGS_NEED_VIEWDIRECTION_WS
                        #define FEATURES_GRAPH_VERTEX
                        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
                        #define SHADERPASS SHADERPASS_UNLIT
                        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

                        // Includes
                        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

                        // --------------------------------------------------
                        // Structs and Packing

                        struct Attributes
                        {
                            float3 positionOS : POSITION;
                            float3 normalOS : NORMAL;
                            float4 tangentOS : TANGENT;
                            #if UNITY_ANY_INSTANCING_ENABLED
                            uint instanceID : INSTANCEID_SEMANTIC;
                            #endif
                        };
                        struct Varyings
                        {
                            float4 positionCS : SV_POSITION;
                            float3 positionWS;
                            float3 normalWS;
                            float3 viewDirectionWS;
                            #if UNITY_ANY_INSTANCING_ENABLED
                            uint instanceID : CUSTOM_INSTANCE_ID;
                            #endif
                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                            #endif
                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                            #endif
                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                            #endif
                        };
                        struct SurfaceDescriptionInputs
                        {
                            float3 WorldSpaceNormal;
                            float3 WorldSpaceViewDirection;
                            float3 WorldSpacePosition;
                            float4 ScreenPosition;
                        };
                        struct VertexDescriptionInputs
                        {
                            float3 ObjectSpaceNormal;
                            float3 ObjectSpaceTangent;
                            float3 ObjectSpacePosition;
                        };
                        struct PackedVaryings
                        {
                            float4 positionCS : SV_POSITION;
                            float3 interp0 : TEXCOORD0;
                            float3 interp1 : TEXCOORD1;
                            float3 interp2 : TEXCOORD2;
                            #if UNITY_ANY_INSTANCING_ENABLED
                            uint instanceID : CUSTOM_INSTANCE_ID;
                            #endif
                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                            #endif
                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                            #endif
                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                            #endif
                        };

                        PackedVaryings PackVaryings(Varyings input)
                        {
                            PackedVaryings output;
                            output.positionCS = input.positionCS;
                            output.interp0.xyz = input.positionWS;
                            output.interp1.xyz = input.normalWS;
                            output.interp2.xyz = input.viewDirectionWS;
                            #if UNITY_ANY_INSTANCING_ENABLED
                            output.instanceID = input.instanceID;
                            #endif
                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                            #endif
                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                            #endif
                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                            output.cullFace = input.cullFace;
                            #endif
                            return output;
                        }
                        Varyings UnpackVaryings(PackedVaryings input)
                        {
                            Varyings output;
                            output.positionCS = input.positionCS;
                            output.positionWS = input.interp0.xyz;
                            output.normalWS = input.interp1.xyz;
                            output.viewDirectionWS = input.interp2.xyz;
                            #if UNITY_ANY_INSTANCING_ENABLED
                            output.instanceID = input.instanceID;
                            #endif
                            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                            #endif
                            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                            #endif
                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                            output.cullFace = input.cullFace;
                            #endif
                            return output;
                        }

                        // --------------------------------------------------
                        // Graph

                        // Graph Properties
                        CBUFFER_START(UnityPerMaterial)
                        float4 Color_8f36033cb7114db69f9e92e4813eeed3;
                        CBUFFER_END

                            // Object and Global properties

                            // Graph Functions

                            void Unity_FresnelEffect_float(float3 Normal, float3 ViewDir, float Power, out float Out)
                            {
                                Out = pow((1.0 - saturate(dot(normalize(Normal), normalize(ViewDir)))), Power);
                            }

                            void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
                            {
                                Out = A * B;
                            }

                            void Unity_Dither_float(float In, float4 ScreenPosition, out float Out)
                            {
                                float2 uv = ScreenPosition.xy * _ScreenParams.xy;
                                float DITHER_THRESHOLDS[16] =
                                {
                                    1.0 / 17.0,  9.0 / 17.0,  3.0 / 17.0, 11.0 / 17.0,
                                    13.0 / 17.0,  5.0 / 17.0, 15.0 / 17.0,  7.0 / 17.0,
                                    4.0 / 17.0, 12.0 / 17.0,  2.0 / 17.0, 10.0 / 17.0,
                                    16.0 / 17.0,  8.0 / 17.0, 14.0 / 17.0,  6.0 / 17.0
                                };
                                uint index = (uint(uv.x) % 4) * 4 + uint(uv.y) % 4;
                                Out = In - DITHER_THRESHOLDS[index];
                            }

                            // Graph Vertex
                            struct VertexDescription
                            {
                                float3 Position;
                                float3 Normal;
                                float3 Tangent;
                            };

                            VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                            {
                                VertexDescription description = (VertexDescription)0;
                                description.Position = IN.ObjectSpacePosition;
                                description.Normal = IN.ObjectSpaceNormal;
                                description.Tangent = IN.ObjectSpaceTangent;
                                return description;
                            }

                            // Graph Pixel
                            struct SurfaceDescription
                            {
                                float3 BaseColor;
                                float Alpha;
                                float AlphaClipThreshold;
                            };

                            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                            {
                                SurfaceDescription surface = (SurfaceDescription)0;
                                float4 Color_ac0eb3d9c9ea4a5f94933ba4a6283731 = IsGammaSpace() ? float4(1, 1, 1, 0) : float4(SRGBToLinear(float3(1, 1, 1)), 0);
                                float _FresnelEffect_77c17e76b9e047b29a574609ba5103b9_Out_3;
                                Unity_FresnelEffect_float(IN.WorldSpaceNormal, IN.WorldSpaceViewDirection, 1.39, _FresnelEffect_77c17e76b9e047b29a574609ba5103b9_Out_3);
                                float4 _Multiply_73fd6e1b518e4e67b22f9e96685d69d7_Out_2;
                                Unity_Multiply_float(Color_ac0eb3d9c9ea4a5f94933ba4a6283731, (_FresnelEffect_77c17e76b9e047b29a574609ba5103b9_Out_3.xxxx), _Multiply_73fd6e1b518e4e67b22f9e96685d69d7_Out_2);
                                float _Dither_dc95ad6a6a134ac980e7a26b0766f861_Out_2;
                                Unity_Dither_float(1, float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0), _Dither_dc95ad6a6a134ac980e7a26b0766f861_Out_2);
                                surface.BaseColor = (_Multiply_73fd6e1b518e4e67b22f9e96685d69d7_Out_2.xyz);
                                surface.Alpha = _Dither_dc95ad6a6a134ac980e7a26b0766f861_Out_2;
                                surface.AlphaClipThreshold = 0;
                                return surface;
                            }

                            // --------------------------------------------------
                            // Build Graph Inputs

                            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                            {
                                VertexDescriptionInputs output;
                                ZERO_INITIALIZE(VertexDescriptionInputs, output);

                                output.ObjectSpaceNormal = input.normalOS;
                                output.ObjectSpaceTangent = input.tangentOS;
                                output.ObjectSpacePosition = input.positionOS;

                                return output;
                            }

                            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                            {
                                SurfaceDescriptionInputs output;
                                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

                                // must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
                                float3 unnormalizedNormalWS = input.normalWS;
                                const float renormFactor = 1.0 / length(unnormalizedNormalWS);


                                output.WorldSpaceNormal = renormFactor * input.normalWS.xyz;		// we want a unit length Normal Vector node in shader graph


                                output.WorldSpaceViewDirection = input.viewDirectionWS; //TODO: by default normalized in HD, but not in universal
                                output.WorldSpacePosition = input.positionWS;
                                output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
                            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                            #else
                            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                            #endif
                            #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                                return output;
                            }


                            // --------------------------------------------------
                            // Main

                            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
                            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/UnlitPass.hlsl"

                            ENDHLSL
                        }
                        Pass
                        {
                            Name "ShadowCaster"
                            Tags
                            {
                                "LightMode" = "ShadowCaster"
                            }

                                // Render State
                                Cull Back
                                Blend One Zero
                                ZTest LEqual
                                ZWrite On
                                ColorMask 0

                                // Debug
                                // <None>

                                // --------------------------------------------------
                                // Pass

                                HLSLPROGRAM

                                // Pragmas
                                #pragma target 4.5
                                #pragma exclude_renderers gles gles3 glcore
                                #pragma multi_compile_instancing
                                #pragma multi_compile _ DOTS_INSTANCING_ON
                                #pragma vertex vert
                                #pragma fragment frag

                                // DotsInstancingOptions: <None>
                                // HybridV1InjectedBuiltinProperties: <None>

                                // Keywords
                                // PassKeywords: <None>
                                // GraphKeywords: <None>

                                // Defines
                                #define _AlphaClip 1
                                #define ATTRIBUTES_NEED_NORMAL
                                #define ATTRIBUTES_NEED_TANGENT
                                #define VARYINGS_NEED_POSITION_WS
                                #define FEATURES_GRAPH_VERTEX
                                /* WARNING: $splice Could not find named fragment 'PassInstancing' */
                                #define SHADERPASS SHADERPASS_SHADOWCASTER
                                /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

                                // Includes
                                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
                                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

                                // --------------------------------------------------
                                // Structs and Packing

                                struct Attributes
                                {
                                    float3 positionOS : POSITION;
                                    float3 normalOS : NORMAL;
                                    float4 tangentOS : TANGENT;
                                    #if UNITY_ANY_INSTANCING_ENABLED
                                    uint instanceID : INSTANCEID_SEMANTIC;
                                    #endif
                                };
                                struct Varyings
                                {
                                    float4 positionCS : SV_POSITION;
                                    float3 positionWS;
                                    #if UNITY_ANY_INSTANCING_ENABLED
                                    uint instanceID : CUSTOM_INSTANCE_ID;
                                    #endif
                                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                                    #endif
                                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                                    #endif
                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                                    #endif
                                };
                                struct SurfaceDescriptionInputs
                                {
                                    float3 WorldSpacePosition;
                                    float4 ScreenPosition;
                                };
                                struct VertexDescriptionInputs
                                {
                                    float3 ObjectSpaceNormal;
                                    float3 ObjectSpaceTangent;
                                    float3 ObjectSpacePosition;
                                };
                                struct PackedVaryings
                                {
                                    float4 positionCS : SV_POSITION;
                                    float3 interp0 : TEXCOORD0;
                                    #if UNITY_ANY_INSTANCING_ENABLED
                                    uint instanceID : CUSTOM_INSTANCE_ID;
                                    #endif
                                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                                    #endif
                                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                                    #endif
                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                                    #endif
                                };

                                PackedVaryings PackVaryings(Varyings input)
                                {
                                    PackedVaryings output;
                                    output.positionCS = input.positionCS;
                                    output.interp0.xyz = input.positionWS;
                                    #if UNITY_ANY_INSTANCING_ENABLED
                                    output.instanceID = input.instanceID;
                                    #endif
                                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                                    #endif
                                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                                    #endif
                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                    output.cullFace = input.cullFace;
                                    #endif
                                    return output;
                                }
                                Varyings UnpackVaryings(PackedVaryings input)
                                {
                                    Varyings output;
                                    output.positionCS = input.positionCS;
                                    output.positionWS = input.interp0.xyz;
                                    #if UNITY_ANY_INSTANCING_ENABLED
                                    output.instanceID = input.instanceID;
                                    #endif
                                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                                    #endif
                                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                                    #endif
                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                    output.cullFace = input.cullFace;
                                    #endif
                                    return output;
                                }

                                // --------------------------------------------------
                                // Graph

                                // Graph Properties
                                CBUFFER_START(UnityPerMaterial)
                                float4 Color_8f36033cb7114db69f9e92e4813eeed3;
                                CBUFFER_END

                                    // Object and Global properties

                                    // Graph Functions

                                    void Unity_Dither_float(float In, float4 ScreenPosition, out float Out)
                                    {
                                        float2 uv = ScreenPosition.xy * _ScreenParams.xy;
                                        float DITHER_THRESHOLDS[16] =
                                        {
                                            1.0 / 17.0,  9.0 / 17.0,  3.0 / 17.0, 11.0 / 17.0,
                                            13.0 / 17.0,  5.0 / 17.0, 15.0 / 17.0,  7.0 / 17.0,
                                            4.0 / 17.0, 12.0 / 17.0,  2.0 / 17.0, 10.0 / 17.0,
                                            16.0 / 17.0,  8.0 / 17.0, 14.0 / 17.0,  6.0 / 17.0
                                        };
                                        uint index = (uint(uv.x) % 4) * 4 + uint(uv.y) % 4;
                                        Out = In - DITHER_THRESHOLDS[index];
                                    }

                                // Graph Vertex
                                struct VertexDescription
                                {
                                    float3 Position;
                                    float3 Normal;
                                    float3 Tangent;
                                };

                                VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                                {
                                    VertexDescription description = (VertexDescription)0;
                                    description.Position = IN.ObjectSpacePosition;
                                    description.Normal = IN.ObjectSpaceNormal;
                                    description.Tangent = IN.ObjectSpaceTangent;
                                    return description;
                                }

                                // Graph Pixel
                                struct SurfaceDescription
                                {
                                    float Alpha;
                                    float AlphaClipThreshold;
                                };

                                SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                                {
                                    SurfaceDescription surface = (SurfaceDescription)0;
                                    float _Dither_dc95ad6a6a134ac980e7a26b0766f861_Out_2;
                                    Unity_Dither_float(1, float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0), _Dither_dc95ad6a6a134ac980e7a26b0766f861_Out_2);
                                    surface.Alpha = _Dither_dc95ad6a6a134ac980e7a26b0766f861_Out_2;
                                    surface.AlphaClipThreshold = 0;
                                    return surface;
                                }

                                // --------------------------------------------------
                                // Build Graph Inputs

                                VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                                {
                                    VertexDescriptionInputs output;
                                    ZERO_INITIALIZE(VertexDescriptionInputs, output);

                                    output.ObjectSpaceNormal = input.normalOS;
                                    output.ObjectSpaceTangent = input.tangentOS;
                                    output.ObjectSpacePosition = input.positionOS;

                                    return output;
                                }

                                SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                                {
                                    SurfaceDescriptionInputs output;
                                    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





                                    output.WorldSpacePosition = input.positionWS;
                                    output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
                                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                                #else
                                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                                #endif
                                #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                                    return output;
                                }


                                // --------------------------------------------------
                                // Main

                                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
                                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShadowCasterPass.hlsl"

                                ENDHLSL
                            }
                            Pass
                            {
                                Name "DepthOnly"
                                Tags
                                {
                                    "LightMode" = "DepthOnly"
                                }

                                    // Render State
                                    Cull Back
                                    Blend One Zero
                                    ZTest LEqual
                                    ZWrite On
                                    ColorMask 0

                                    // Debug
                                    // <None>

                                    // --------------------------------------------------
                                    // Pass

                                    HLSLPROGRAM

                                    // Pragmas
                                    #pragma target 4.5
                                    #pragma exclude_renderers gles gles3 glcore
                                    #pragma multi_compile_instancing
                                    #pragma multi_compile _ DOTS_INSTANCING_ON
                                    #pragma vertex vert
                                    #pragma fragment frag

                                    // DotsInstancingOptions: <None>
                                    // HybridV1InjectedBuiltinProperties: <None>

                                    // Keywords
                                    // PassKeywords: <None>
                                    // GraphKeywords: <None>

                                    // Defines
                                    #define _AlphaClip 1
                                    #define ATTRIBUTES_NEED_NORMAL
                                    #define ATTRIBUTES_NEED_TANGENT
                                    #define VARYINGS_NEED_POSITION_WS
                                    #define FEATURES_GRAPH_VERTEX
                                    /* WARNING: $splice Could not find named fragment 'PassInstancing' */
                                    #define SHADERPASS SHADERPASS_DEPTHONLY
                                    /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

                                    // Includes
                                    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                                    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                                    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                                    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
                                    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

                                    // --------------------------------------------------
                                    // Structs and Packing

                                    struct Attributes
                                    {
                                        float3 positionOS : POSITION;
                                        float3 normalOS : NORMAL;
                                        float4 tangentOS : TANGENT;
                                        #if UNITY_ANY_INSTANCING_ENABLED
                                        uint instanceID : INSTANCEID_SEMANTIC;
                                        #endif
                                    };
                                    struct Varyings
                                    {
                                        float4 positionCS : SV_POSITION;
                                        float3 positionWS;
                                        #if UNITY_ANY_INSTANCING_ENABLED
                                        uint instanceID : CUSTOM_INSTANCE_ID;
                                        #endif
                                        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                        uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                                        #endif
                                        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                        uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                                        #endif
                                        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                        FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                                        #endif
                                    };
                                    struct SurfaceDescriptionInputs
                                    {
                                        float3 WorldSpacePosition;
                                        float4 ScreenPosition;
                                    };
                                    struct VertexDescriptionInputs
                                    {
                                        float3 ObjectSpaceNormal;
                                        float3 ObjectSpaceTangent;
                                        float3 ObjectSpacePosition;
                                    };
                                    struct PackedVaryings
                                    {
                                        float4 positionCS : SV_POSITION;
                                        float3 interp0 : TEXCOORD0;
                                        #if UNITY_ANY_INSTANCING_ENABLED
                                        uint instanceID : CUSTOM_INSTANCE_ID;
                                        #endif
                                        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                        uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                                        #endif
                                        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                        uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                                        #endif
                                        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                        FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                                        #endif
                                    };

                                    PackedVaryings PackVaryings(Varyings input)
                                    {
                                        PackedVaryings output;
                                        output.positionCS = input.positionCS;
                                        output.interp0.xyz = input.positionWS;
                                        #if UNITY_ANY_INSTANCING_ENABLED
                                        output.instanceID = input.instanceID;
                                        #endif
                                        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                        output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                                        #endif
                                        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                        output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                                        #endif
                                        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                        output.cullFace = input.cullFace;
                                        #endif
                                        return output;
                                    }
                                    Varyings UnpackVaryings(PackedVaryings input)
                                    {
                                        Varyings output;
                                        output.positionCS = input.positionCS;
                                        output.positionWS = input.interp0.xyz;
                                        #if UNITY_ANY_INSTANCING_ENABLED
                                        output.instanceID = input.instanceID;
                                        #endif
                                        #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                                        output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                                        #endif
                                        #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                                        output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                                        #endif
                                        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                        output.cullFace = input.cullFace;
                                        #endif
                                        return output;
                                    }

                                    // --------------------------------------------------
                                    // Graph

                                    // Graph Properties
                                    CBUFFER_START(UnityPerMaterial)
                                    float4 Color_8f36033cb7114db69f9e92e4813eeed3;
                                    CBUFFER_END

                                        // Object and Global properties

                                        // Graph Functions

                                        void Unity_Dither_float(float In, float4 ScreenPosition, out float Out)
                                        {
                                            float2 uv = ScreenPosition.xy * _ScreenParams.xy;
                                            float DITHER_THRESHOLDS[16] =
                                            {
                                                1.0 / 17.0,  9.0 / 17.0,  3.0 / 17.0, 11.0 / 17.0,
                                                13.0 / 17.0,  5.0 / 17.0, 15.0 / 17.0,  7.0 / 17.0,
                                                4.0 / 17.0, 12.0 / 17.0,  2.0 / 17.0, 10.0 / 17.0,
                                                16.0 / 17.0,  8.0 / 17.0, 14.0 / 17.0,  6.0 / 17.0
                                            };
                                            uint index = (uint(uv.x) % 4) * 4 + uint(uv.y) % 4;
                                            Out = In - DITHER_THRESHOLDS[index];
                                        }

                                    // Graph Vertex
                                    struct VertexDescription
                                    {
                                        float3 Position;
                                        float3 Normal;
                                        float3 Tangent;
                                    };

                                    VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                                    {
                                        VertexDescription description = (VertexDescription)0;
                                        description.Position = IN.ObjectSpacePosition;
                                        description.Normal = IN.ObjectSpaceNormal;
                                        description.Tangent = IN.ObjectSpaceTangent;
                                        return description;
                                    }

                                    // Graph Pixel
                                    struct SurfaceDescription
                                    {
                                        float Alpha;
                                        float AlphaClipThreshold;
                                    };

                                    SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                                    {
                                        SurfaceDescription surface = (SurfaceDescription)0;
                                        float _Dither_dc95ad6a6a134ac980e7a26b0766f861_Out_2;
                                        Unity_Dither_float(1, float4(IN.ScreenPosition.xy / IN.ScreenPosition.w, 0, 0), _Dither_dc95ad6a6a134ac980e7a26b0766f861_Out_2);
                                        surface.Alpha = _Dither_dc95ad6a6a134ac980e7a26b0766f861_Out_2;
                                        surface.AlphaClipThreshold = 0;
                                        return surface;
                                    }

                                    // --------------------------------------------------
                                    // Build Graph Inputs

                                    VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                                    {
                                        VertexDescriptionInputs output;
                                        ZERO_INITIALIZE(VertexDescriptionInputs, output);

                                        output.ObjectSpaceNormal = input.normalOS;
                                        output.ObjectSpaceTangent = input.tangentOS;
                                        output.ObjectSpacePosition = input.positionOS;

                                        return output;
                                    }

                                    SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                                    {
                                        SurfaceDescriptionInputs output;
                                        ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





                                        output.WorldSpacePosition = input.positionWS;
                                        output.ScreenPosition = ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
                                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                                    #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                                    #else
                                    #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                                    #endif
                                    #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                                        return output;
                                    }


                                    // --------------------------------------------------
                                    // Main

                                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
                                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                                    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthOnlyPass.hlsl"

                                    ENDHLSL
                                }
                    }
                        FallBack "Hidden/Shader Graph/FallbackError"
}
