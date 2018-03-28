Shader "Custom/Water"
{
	Properties
	{
		_Color("Color", Color) = (1, 1, 1, 1)
		_EdgeColor("Edge Color", Color) = (1, 1, 1, 1)
		_DepthFactor("Depth Factor", float) = 1.0
		_DepthRampTex("Depth Ramp", 2D) = "white" {}
		_MainTex("Main Texture", 2D) = "white" {}
		_Transparency("Transparency", Range(0.0,1)) = 0.5
		_Distance("Distance", Float) = 1
		_Amplitude("Amplitude", Float) = 1
		_Speed("Speed", Float) = 1
		_Amount("Amount", Range(0.0,1.0)) = 1
	}

	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"RenderType" = "Transparent"
		}

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		// Grab the screen behind the object into _BackgroundTexture
		GrabPass
		{
			"_BackgroundTexture"
		}

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
	#include "UnityCG.cginc"

	#pragma vertex vert
	#pragma fragment frag

		

		// Properties
		float4 _Color;
		float4 _EdgeColor;
		float  _DepthFactor;
		sampler2D _CameraDepthTexture;
		sampler2D _DepthRampTex;
		sampler2D _MainTex;
		float4 _MainTex_ST;
		float _Transparency;
		float _Distance;
		float _Amplitude;
		float _Speed;
		float _Amount;

		struct vertexInput
		{
			float4 vertex : POSITION;
			float2 uv : TEXCOORD0;
		};

		struct vertexOutput
		{
			float4 vertex : SV_POSITION;
			float2 uv : TEXCOORD0;
			float4 screenPos : TEXCOORD1;
		};

		vertexOutput vert(vertexInput input)
		{
			vertexOutput output;

			// convert to world space
			output.vertex = UnityObjectToClipPos(input.vertex);

			// compute depth
			output.screenPos = ComputeScreenPos(output.vertex);

			// texture coordinates 
			output.uv = input.uv;

			input.vertex.y += sin(_Time.y * _Speed + input.vertex.y * _Amplitude) * _Distance * _Amount;
			output.vertex = UnityObjectToClipPos(input.vertex);
			output.uv = TRANSFORM_TEX(input.uv, _MainTex);

			return output;
		}

		float4 frag(vertexOutput input) : COLOR
		{
			// apply depth texture
			float4 depthSample = SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, input.screenPos);
			float depth = LinearEyeDepth(depthSample).r;

			// create foamline
			float foamLine = 1 - saturate(_DepthFactor * (depth - input.screenPos.w));
			float4 foamRamp = float4(tex2D(_DepthRampTex, float2(foamLine, 0.5)).rgb, 1.0);

			// sample main texture
			float4 albedo = tex2D(_MainTex, input.uv.xy);

			//float4 col = _Color * foamRamp * albedo;
			float4 col = _Color + foamLine * _EdgeColor * albedo;

			col.a = _Transparency;

			return col;
		}

			ENDCG
		}
	}
}