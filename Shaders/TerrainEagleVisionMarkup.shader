Shader "TerrainEagleVisionMarkup"
{
    SubShader
    {

        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Opaque"
            "Queue" = "AlphaTest"
        }

        Pass
        {
            Stencil
            {
                Ref 1
                Comp Always
                ZFail Replace
            }

            Blend Zero One
            Cull Off
            ZWrite Off
        }
    }
}
