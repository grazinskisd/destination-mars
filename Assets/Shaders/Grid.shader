shader "Custom/Grid"
{
	Properties
	{
		_Color ("Grid Color", Color) = (1,1,1,1)
      	_GridSize("Grid Size", Float) = 10
	}
	SubShader
	{
		Tags { "Queue"="Background" "RenderType"="Transparent" }
		LOD 100
		ZWrite Off
		ZTest Always

		Pass
		{
         		Blend SrcAlpha OneMinusSrcAlpha
         		Offset -20, -20
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

         float _GridSize;
		 fixed4 _Color;

	struct appdata
	{
		float4 vertex : POSITION;
	};

	struct v2f
	{
		float2 uv : TEXCOORD0;
		float4 vertex : SV_POSITION;
	};


	v2f vert (appdata v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uv = mul(unity_ObjectToWorld, v.vertex).xz;
		return o;
	}

         float DrawGrid(float2 uv, float sz, float aa)
         {
            float aaThresh = aa;
            float aaMin = aa*0.01;

            float2 gUV = uv / sz + aaThresh;

            float2 fl = floor(gUV);
            gUV = frac(gUV);
            gUV -= aaThresh;
            gUV = smoothstep(aaThresh, aaMin, abs(gUV));
            float d = max(gUV.x, gUV.y);

            return d;
         }

			fixed4 frag (v2f i) : SV_Target
			{
				fixed r = DrawGrid(i.uv, _GridSize, 0.05);
				return _Color * r;
			}
			ENDCG
		}
	}
}