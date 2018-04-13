Shader "Custom/EditModeMarker"
{
	Properties
	{
		_Color("Color", Color) = (1, 1, 1, 1)
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


		Pass
		{

			CGPROGRAM
			#include "UnityCG.cginc"
			
			#pragma vertex vert
			#pragma fragment frag



			// Properties
			float4 _Color;
			float _Transparency;

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);

				return o;
			}

			float4 frag(v2f i) : COLOR
			{
				float4 col = _Color;
				col.a = 1 - _Transparency;

				return col;
			}

			ENDCG
		}
	}
}