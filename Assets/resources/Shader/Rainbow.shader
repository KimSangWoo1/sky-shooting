// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Rainbow shader with lots of adjustable properties!

Shader "_Shaders/Rainbow" {
	Properties{
		_Saturation("Saturation", Range(0.0, 1.0)) = 0.8
		_Luminosity("Luminosity", Range(0.0, 1.0)) = 0.5
		_Spread("Spread", Range(0.5, 10.0)) = 3.8
		_Speed("Speed", Range(-10.0, 10.0)) = 2.4
		_TimeOffset("TimeOffset", Range(0.0, 6.28318531)) = 0.0
	}
		SubShader{
		Pass{
		CGPROGRAM

#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"
//#include "ShaderTools.cginc"

		fixed _Saturation;
	fixed _Luminosity;
	half _Spread;
	half _Speed;
	half _TimeOffset;

	struct vertexInput {
		float4 vertex : POSITION;
		float4 texcoord0 : TEXCOORD0;
	};

	struct fragmentInput {
		float4 position : SV_POSITION;
		float4 texcoord0 : TEXCOORD0;
		fixed3 localPosition : TEXCOORD1;
	};


	inline fixed4 RGBtoHSL(fixed4 rgb) {
		fixed4 hsl = fixed4(0.0, 0.0, 0.0, rgb.w);

		fixed vMin = min(min(rgb.x, rgb.y), rgb.z);
		fixed vMax = max(max(rgb.x, rgb.y), rgb.z);
		fixed vDelta = vMax - vMin;

		hsl.z = (vMax + vMin) / 2.0;

		if (vDelta == 0.0) {
			hsl.x = hsl.y = 0.0;
		}
		else {
			if (hsl.z < 0.5) hsl.y = vDelta / (vMax + vMin);
			else hsl.y = vDelta / (2.0 - vMax - vMin);

			float rDelta = (((vMax - rgb.x) / 6.0) + (vDelta / 2.0)) / vDelta;
			float gDelta = (((vMax - rgb.y) / 6.0) + (vDelta / 2.0)) / vDelta;
			float bDelta = (((vMax - rgb.z) / 6.0) + (vDelta / 2.0)) / vDelta;

			if (rgb.x == vMax) hsl.x = bDelta - gDelta;
			else if (rgb.y == vMax) hsl.x = (1.0 / 3.0) + rDelta - bDelta;
			else if (rgb.z == vMax) hsl.x = (2.0 / 3.0) + gDelta - rDelta;

			if (hsl.x < 0.0) hsl.x += 1.0;
			if (hsl.x > 1.0) hsl.x -= 1.0;
		}

		return hsl;
	}

	inline fixed hueToRGB(float v1, float v2, float vH) {
		if (vH < 0.0) vH += 1.0;
		if (vH > 1.0) vH -= 1.0;
		if ((6.0 * vH) < 1.0) return (v1 + (v2 - v1) * 6.0 * vH);
		if ((2.0 * vH) < 1.0) return (v2);
		if ((3.0 * vH) < 2.0) return (v1 + (v2 - v1) * ((2.0 / 3.0) - vH) * 6.0);
		return v1;
	}

	inline fixed4 HSLtoRGB(fixed4 hsl) {
		fixed4 rgb = fixed4(0.0, 0.0, 0.0, hsl.w);

		if (hsl.y == 0) {
			rgb.xyz = hsl.zzz;
		}
		else {
			float v1;
			float v2;

			if (hsl.z < 0.5) v2 = hsl.z * (1 + hsl.y);
			else v2 = (hsl.z + hsl.y) - (hsl.y * hsl.z);

			v1 = 2.0 * hsl.z - v2;

			rgb.x = hueToRGB(v1, v2, hsl.x + (1.0 / 3.0));
			rgb.y = hueToRGB(v1, v2, hsl.x);
			rgb.z = hueToRGB(v1, v2, hsl.x - (1.0 / 3.0));
		}

		return rgb;
	}
	fragmentInput vert(vertexInput i) {
		fragmentInput o;
		o.position = UnityObjectToClipPos(i.vertex);
		o.texcoord0 = i.texcoord0;
		o.localPosition = i.vertex.xyz; +fixed3(0.5, 0.5, 0.5);
		return o;
	}

	fixed4 frag(fragmentInput i) : SV_TARGET{
		fixed2 lPos = i.localPosition / _Spread;
	half time = _Time.y * _Speed / _Spread;
	half timeWithOffset = time + _TimeOffset;
	fixed sine = sin(timeWithOffset);
	fixed cosine = cos(timeWithOffset);
	//fixed hue = (lPos.x * sine + lPos.y * cosine) / 2.0;
	//fixed hue = (lPos.x * 0 - lPos.y) / 2.0;
	fixed hue = (-lPos.y) / 2.0;
	hue += time;
	while (hue < 0.0) hue += 1.0;
	while (hue > 1.0) hue -= 1.0;
	fixed4 hsl = fixed4(hue, _Saturation, _Luminosity, 1.0);
	return HSLtoRGB(hsl);
	}

		ENDCG
	}
	}
		FallBack "Diffuse"
}