Shader "Cube/Grid"
{
    Properties
    {
        _BorderColor("Border Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _GridRatio("Grid Ratio", Float) = 10.0
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 gridPosition : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 gridPosition : TEXCOORD0;
            };

            fixed4 _BorderColor;
            float _GridRatio;

            float grid(float2 position, float2 positionDdx, float2 positionDdy, float gridRatio)
            {
                float2 kernelSize = max(abs(positionDdx), abs(positionDdy));
                float2 kernelExtents = kernelSize * 0.5;
                float2 kernelMin = position - kernelExtents;
                float2 kernelMax = position + kernelExtents;
                float2 integral = (
                    floor(kernelMax)
                    + min(frac(kernelMax) * gridRatio, 1.0)
                    - floor(kernelMin)
                    - min(frac(kernelMin) * gridRatio, 1.0)
                ) / (kernelSize * gridRatio);
                return 1.0 - ((1.0 - integral.x) * (1.0 - integral.y));
            }

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.gridPosition = mul(unity_ObjectToWorld, v.vertex) + (0.5 / _GridRatio);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 position = i.gridPosition;
                return grid(position, ddx(position), ddy(position), _GridRatio) * _BorderColor;
            }
            ENDCG
        }
    }
}
