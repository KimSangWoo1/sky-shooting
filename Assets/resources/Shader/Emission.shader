Shader "Custom/Emission"
{
    //사용자 인터페이스
    Properties
    {
        //변수("이름",자료형) = 초기값
        fR("Red",Range(0,1)) = 1.0
        fG("Green",Range(0,1)) = 0.0
        fB("Blue",Range(0,1)) = 0.0

    }

        SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200

        //CG 언어  CGPROGRAM  ~ ENDCG   
        CGPROGRAM
        //Surface Shader임을 알려주는 #paragma
        #pragma surface surf  Standard fullforwardshadows  
        #pragma target 3.0  

        struct Input
        {
            float2 uv_MainTex;
        };

        float fR;
        float fG;
        float fB;

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            o.Emission = float3(fR, fG, fB);
        }
        ENDCG
    }
        FallBack "Diffuse"
}

