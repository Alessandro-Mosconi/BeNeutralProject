Shader "Custom/OutlineShader"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _Outline ("Outline width", Range (.002, 0.03)) = .005
    }

    SubShader
    {
        Tags
        {
            "Queue"="Overlay"
        }

        Pass
        {
            Name "OUTLINE"
            Tags {"LightMode"="Always"}

            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite On
            Cull Front

            ColorMask RGB
            ColorMask A

            Stencil
            {
                Ref 1
                Comp always
                Pass replace
            }

            Material
            {
                Emission [_OutlineColor]
            }
        }
    }

    SubShader
    {
        Tags
        {
            "Queue"="Overlay"
        }

        Pass
        {
            Name "OUTLINE"
            Tags {"LightMode"="Always"}

            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite On
            Cull Front

            ColorMask RGB
            ColorMask A

            Stencil
            {
                Ref 1
                Comp equal
                Pass replace
            }

            Material
            {
                Emission [_OutlineColor]
            }
        }
    }

    SubShader
    {
        Tags
        {
            "Queue"="Overlay"
        }

        Pass
        {
            Name "OUTLINE"
            Tags {"LightMode"="Always"}

            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite On
            Cull Front

            ColorMask RGB
            ColorMask A

            Stencil
            {
                Ref 1
                Comp always
                Pass replace
            }

            Material
            {
                Emission [_OutlineColor]
            }
        }

        Pass
        {
            Name "BASE"
            Tags {"LightMode"="Always"}

            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Back

            Material
            {
                Diffuse [_Color]
            }
        }
    }
}