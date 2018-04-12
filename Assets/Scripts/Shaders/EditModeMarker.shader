Shader "Custom/EditModeMarker"
{
	Properties
	{
		_Color("Color", Color) = (1, 1, 1, 1)
		_EdgeColor("Edge Color", Color) = (1, 1, 1, 1)
		_DepthFactor("Depth Factor", float) = 1.0
		_DepthRampTex("Depth Ramp", 2D) = "white" {}
		_Transparency("Transparency", Range(0.0,1)) = 0.5
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
			float _Transparency;

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

			output.vertex = UnityObjectToClipPos(input.vertex);

			return output;
		}

		float4 frag(vertexOutput input) : COLOR
		{
			// apply depth texture
			float4 depthSample = SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, input.screenPos);
			float depth = LinearEyeDepth(depthSample).r;

			// create foamline
			float foamLine = 1 - saturate(_DepthFactor * (depth - input.screenPos.w));
			//float4 foamRamp = float4(tex2D(_DepthRampTex, float2(foamLine, 0.5)).rgb, 1.0);


			//float4 col = _Color * foamRamp * albedo;
			float4 col = _Color + foamLine * _EdgeColor;

			col.a = _Transparency;

			return col;
		}

		ENDCG
		}
	}
}