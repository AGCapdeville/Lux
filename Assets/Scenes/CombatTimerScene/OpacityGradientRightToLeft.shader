Shader "Custom/OpacityGradientRightToLeft"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _OpacityRight ("Right Opacity", Range(0,1)) = 1.0
        _OpacityLeft ("Left Opacity", Range(0,1)) = 0.0
        _FadeStart ("Fade Start", Range(0,1)) = 0.2
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 200
        
        CGPROGRAM
        #pragma surface surf Lambert alpha:blend
        
        sampler2D _MainTex;
        fixed4 _Color;
        half _OpacityRight;
        half _OpacityLeft;
        half _FadeStart;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            float width = IN.uv_MainTex.x; // Use the x-coordinate of the UV for the gradient
            float gradient = 1.0; // Default to fully opaque
            
            // Calculate the gradient based on the fade start point
            if (width <= _FadeStart)
            {
                gradient = lerp(_OpacityRight, _OpacityLeft, width / _FadeStart);
            }
            else
            {
                gradient = _OpacityLeft;
            }
            
            c.a *= gradient;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
