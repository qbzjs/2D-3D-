#ifndef SIMPLE_SONAR_CORE_INCLUDED
#define SIMPLE_SONAR_CORE_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

// Required to compile gles 2.0 with standard srp library
#pragma prefer_hlslcc gles
#pragma exclude_renderers d3d11_9x
#pragma target 2.0

CBUFFER_START(SonarProperties)
Texture2D _RingTex;
SamplerState sampler_RingTex;
float4 _RingTex_ST;

float4 _RingColor;
half _SonarBlendMode;
half _RingDuration;
half _RingRange;
half _RingSpeed;
half _RingWidth;

half _RingIntensityMax;
half _RingIntensityFalloffPerSecond;
half _RingIntensityFalloffPerMeter;
float4 _RingIntensityFadeOutColor;
float4 _RingIntensityFadeInColor;

half _StartTime;
CBUFFER_END


// The size of these arrays is the number of rings that can be rendered at once.
// If you want to change this, you must also change QueueSize in SimpleSonarShader_SonarSender.cs (or whatever script you're using)
// If you're having graphics perforance issues then definitely try toning this down to what fits your needs.
// Also you may need to restart your Unity editor since it was being weird for me after changing this number. 
#define RingArraySize 64

CBUFFER_START(SonarHitPoints)
float4 _hitPts[RingArraySize]; //xyz are the point and w is the time that it hit
CBUFFER_END
CBUFFER_START(SonarIntensity)
half _Intensity[RingArraySize];
CBUFFER_END
CBUFFER_START(SonarRingPassIndex)
half _RingPassIndex[RingArraySize];
CBUFFER_END
CBUFFER_START(RingColorModified)
float4 _RingColorModified;
CBUFFER_END

struct appdata
{
	float4 vertex : POSITION;
};

struct v2f
{
	float4 vertex : SV_POSITION;
	float3 worldPos : TEXCOORD1;
};

v2f SonarVert(appdata v)
{
	v2f o;

	VertexPositionInputs vertInputs = GetVertexPositionInputs(v.vertex.xyz);    //This function calculates all the relative spaces of the objects vertices
	o.vertex = vertInputs.positionCS;

	o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

	return o;
}

float4 blendSonarColors(float4 col1, float4 col2, half blendModeEnum) {
	switch (blendModeEnum)
	{
	default: // Additive
		return col1 + col2;
		break;
	case 2: // Subtractive
		return col1 - col2;
		break;
	case 3: // Highest individual values
		return float4(max(col1.x, col2.x), max(col1.y, col2.y), max(col1.z, col2.z), max(col1.a, col2.a));
		break;
	case 4: // Multiplicative
		return col1 * col2;
		break;
	}
}

float4 SonarFrag(v2f IN) : SV_Target
{
	float4 combinedColor = (0,0,0,0);

	// Check every point in the array
	// The goal is to set RGB to highest possible values based on current sonar rings
	for (int i = 0; i < RingArraySize; i++) {

		//Make sure we are using the right ring render pass (for multi colors)
		if (_RingPassIndex[i] != SonarPass) continue;

		float timeAlive = _Time.y - _hitPts[i].w;
		// Checks that this ring is still within its max duration
		if (timeAlive <= _RingDuration) {

			half distPointToFrag = distance(_hitPts[i].xyz, IN.worldPos);
			// Checks that the ring is still within its max range
			if (distPointToFrag <= _RingRange) {

				float distTraveled = timeAlive * _RingSpeed;

				half intensity = _Intensity[i] 
					* max(0, (1 - _RingIntensityFalloffPerSecond * timeAlive))
					* max(0, (1 - _RingIntensityFalloffPerMeter * distTraveled))
					/ _RingIntensityMax;

				// Checks that the ring hasn't had too much falloff already;
				if(intensity > 0) {

					// Checks that the frag point is within the ring's width
					if (distPointToFrag < distTraveled && distPointToFrag > distTraveled - _RingWidth) {

						half posInRing = (distTraveled - distPointToFrag) / _RingWidth;

						// Calculate predicted RGB values sampling the texture radially
						float angle = acos(dot(normalize(IN.worldPos - _hitPts[i]), float3(1, 0, 0)));
						float4 ringTexColor = _RingTex.Sample(sampler_RingTex, half2(posInRing, angle));
						
						float4 fadeColor = lerp(_RingIntensityFadeOutColor, _RingIntensityFadeInColor, float4(1, 1, 1, 1) * intensity);
						float4 thisRingCol = blendSonarColors(ringTexColor, fadeColor, 4);

						combinedColor = blendSonarColors(combinedColor, thisRingCol, _SonarBlendMode);
					}
				}
			}
		}
	}

	if (_RingColorModified.x != 0 || _RingColorModified.y != 0 || _RingColorModified.z != 0 || _RingColorModified.a != 0) {
		_RingColor = _RingColorModified;
	}
	combinedColor *= _RingColor;
	// Ensures all values are within 0 and 1
	combinedColor = saturate(combinedColor);
	combinedColor.a = 1;

	return combinedColor;
}

#endif
