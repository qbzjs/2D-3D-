// SimpleSonarShader scripts and shaders were written by Drew Okenfuss.
// For this shader to work, the object must have values passed in to it from the SimpleSonarShader_Parent.cs script.
// By default, this happens by having the object be a child of SimpleSonarShader_Parent.
Shader "MadeByProfessorOakie/SimpleSonarShader" {
	Properties{
		_RingColor("Ring Color Tint", Color) = (1,1,1,1)
		_SonarBlendMode("Multi-Rings Color Blend Mode (See comment)", int) = 1 // See blendSonarColors(...) in SimpleSonarCore.hlsl for values
		_RingDuration("Ring Lifetime Duration (Max)", float) = 20
		_RingRange("Ring Range (Max)", float) = 20
		_RingSpeed("Ring Speed", float) = 1
		_RingWidth("Ring Width", float) = 0.1
		_RingIntensityMax("Ring Intensity Maximum input", float) = 10
		_RingIntensityFalloffPerSecond("Ring Intensity Falloff Per Second", float) = 0.1
		_RingIntensityFalloffPerMeter("Ring Intensity Falloff Per Meter", float) = 0.1
		_RingIntensityFadeOutColor("Color to Fade To at Low Intensity", Color) = (0, 0, 0, 1)
		_RingIntensityFadeInColor("Color to Fade To at High Intensity", Color) = (1, 1, 1, 1)
		_RingTex("Ring Texture", 2D) = "white" {}
	}

	SubShader{
		Tags{"RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" }
		LOD 200

		// If you want more then 2 colors then go ahead and copy this and paste more passes below and change them to 2, 3, 4, etc. 
		Pass {
			Name "Sonar Shader Pass 0"
			Tags{"LightMode" = "UniversalForward"}

			Blend SrcAlpha One

		HLSLPROGRAM

			#define SonarPass 0
			#pragma vertex SonarVert
			#pragma fragment SonarFrag
			#include "Assets\Downloaded_Assets\Shader_Assets\SonarEffect\MadeByProfessorOakie\SimpleSonarCore.hlsl"

		ENDHLSL
		}

		Pass {
			Name "Sonar Shader Pass 1"
			Tags{"LightMode" = "UniversalForward"}

			Blend SrcAlpha One

		HLSLPROGRAM
			
			#define SonarPass 1
			#pragma vertex SonarVert
			#pragma fragment SonarFrag
			#include "Assets\Downloaded_Assets\Shader_Assets\SonarEffect\MadeByProfessorOakie\SimpleSonarCore.hlsl"

		ENDHLSL
		}

	}

	FallBack "Universal Render Pipeline/Lit"
}
