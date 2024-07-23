Shader "Custom/StencilMask"
{
    SubShader
    {
        Tags { "Queue" = "Geometry+10" }

        Pass
        {
            // Write to the stencil buffer
            Stencil
            {
                Ref 1
                Comp always
                Pass replace
            }

            // Don't actually render anything
            ColorMask 0
        }
    }
}
