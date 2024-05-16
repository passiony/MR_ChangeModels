Shader "loom/fake_shadow_test_pass_order"
{
    Properties
    {
        // 地面Y高度
        _GroundY ("地面Y高度 (外部传入)", float) = 0
        // 影子颜色
        _Shadow_Color ("影子颜色", Color) = (1,1,1,1)
        // 影子长度
        _Shadow_Length ("影子长度", float) = 0
        // 影子旋转角度
        _Shadow_Rotated ("影子旋转角度", range(0,360)) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Geometry+1"  // 地面要先绘制，所以设置为 Geometry+1
        }

        Pass
        {
            // 渲染阴影 Pass

            Stencil
            {
                Ref 1
                Comp Greater
                Pass Replace
            }

            Blend SrcAlpha oneMinusSrcAlpha   

            Offset -2,-2

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag_shadow
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 worldPos : TEXCOORD0;
                float cacheWorldY : TEXCOORD1;
            };

            half _GroundY;
            half4 _Shadow_Color;   
            half _Shadow_Length;     
            half _Shadow_Rotated;
            
            v2f vert(appdata v)
            {
                v2f o = (v2f)0;

                o.worldPos = mul(unity_ObjectToWorld,v.vertex);
                o.cacheWorldY = o.worldPos.y;

                o.worldPos.y = _GroundY;

                half lerpVal = lerp(0,_Shadow_Length,o.cacheWorldY-_GroundY);

                half radian = _Shadow_Rotated / 180.0 * UNITY_PI;
                half2 ratatedAngle = half2((0*cos(radian)-1*sin(radian)),(0*sin(radian)+1*cos(radian)));
                
                o.worldPos.xz += lerpVal * ratatedAngle;
                
                o.pos = mul(UNITY_MATRIX_VP,o.worldPos);

                return o;
            }

            fixed4 frag_shadow(v2f i) : SV_TARGET
            {
                clip(i.cacheWorldY - _GroundY);
                return _Shadow_Color;
            }

            ENDCG
        }


    }
}
