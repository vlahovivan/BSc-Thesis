/*Please do support www.bitshiftprogrammer.com by joining the facebook page : fb.com/BitshiftProgrammer
Legal Stuff:
This code is free to use no restrictions but attribution would be appreciated.
Any damage caused either partly or completly due to usage of this stuff is not my responsibility*/

Shader "Custom/WaterShader"
{
	Properties
	{
		[NoScaleOffset] _DispTex("Displacement Texture", 2D) = "white" {}
        _Amount("Displacement Amount", Range(0,20)) = 0.5
		_DispTexScale("Displacement Scale", Range(0, 1)) = 1.0
	}
	SubShader
	{
		Tags {"Queue" = "Transparent" "RenderType" = "Transparent" }

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			float2 random2(float2 p);

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			struct Input {
				float2 uv_BumpTex : TEXCOORD1;
			};

			float _Amount;
			float _DispTexScale;
        	sampler2D _DispTex;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;

				o.vertex.y += (tex2Dlod(_DispTex, float4((_DispTexScale + 0.125 * sin(0.5 * _Time)) * o.uv.xy, 0, 0)).r - 0.5)  * _Amount;
				return o;
			}
			float2 random2(float2 p)
			{
				return frac(sin(float2(dot(p,float2(117.12,341.7)),dot(p,float2(269.5,123.3))))*43458.5453);
			}
			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col;
				float2 uv = i.uv;
				uv *= 6.0; //Scaling amount (larger number more cells can be seen)
				float2 iuv = floor(uv); //gets integer values no floating point
				float2 fuv = frac(uv); // gets only the fractional part
				float minDist = 1.0;  // minimun distance
				for (int y = -1; y <= 1; y++)
				{
					for (int x = -1; x <= 1; x++)
					{
						// Position of neighbour on the grid
						float2 neighbour = float2(float(x), float(y));
						// Random position from current + neighbour place in the grid
						float2 pointv = random2(iuv + neighbour);
						// Move the point with time
						pointv = 0.5 + 0.5*sin(_Time.z / 2.0 + 6.2236*pointv);//each point moves in a certain way
																		// Vector between the pixel and the point
						float2 diff = neighbour + pointv - fuv;
						// Distance to the point
						float dist = length(diff);
						// Keep the closer distance
						minDist = min(minDist, dist);
					}
				}
				// Draw the min distance (distance field)
                col = minDist * float4(0.4, 1.0, 1.0, 0.5) + (1.0 - minDist) * float4(0.0, 0.6, 1.0, 0.5);
				return col;
			}
		ENDCG
		}
	}
}