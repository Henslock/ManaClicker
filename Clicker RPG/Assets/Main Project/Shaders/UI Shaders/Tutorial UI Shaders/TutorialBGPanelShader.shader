// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ClickerRPG/UI/TutorialBGPanelShader"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
		_MainTexture("Main Texture", 2D) = "white" {}
		_Mask("Mask", 2D) = "white" {}
		_SecondaryMask("Secondary Mask", 2D) = "white" {}
		_MaskSlider("Mask Slider", Range( 0 , 1)) = 0
		_Power("Power", Float) = 1
		_BaseColor("Base Color", Color) = (0,0,0,0)
		_Alpha("Alpha", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}

	SubShader
	{
		LOD 0

		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha
		
		
		Pass
		{
		CGPROGRAM
			
			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
			#include "UnityCG.cginc"
			

			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				
			};
			
			uniform fixed4 _Color;
			uniform float _EnableExternalAlpha;
			uniform sampler2D _MainTex;
			uniform sampler2D _AlphaTex;
			uniform float4 _BaseColor;
			uniform sampler2D _MainTexture;
			uniform float4 _MainTexture_ST;
			uniform sampler2D _Mask;
			uniform float4 _Mask_ST;
			uniform float _MaskSlider;
			uniform float _Power;
			uniform sampler2D _SecondaryMask;
			uniform float4 _SecondaryMask_ST;
			uniform float _Alpha;

			
			v2f vert( appdata_t IN  )
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
				
				
				IN.vertex.xyz +=  float3(0,0,0) ; 
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

#if ETC1_EXTERNAL_ALPHA
				// get the color from an external texture (usecase: Alpha support for ETC1 on android)
				fixed4 alpha = tex2D (_AlphaTex, uv);
				color.a = lerp (color.a, alpha.r, _EnableExternalAlpha);
#endif //ETC1_EXTERNAL_ALPHA

				return color;
			}
			
			fixed4 frag(v2f IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				float2 uv_MainTexture = IN.texcoord.xy * _MainTexture_ST.xy + _MainTexture_ST.zw;
				float2 uv_Mask = IN.texcoord.xy * _Mask_ST.xy + _Mask_ST.zw;
				float4 temp_cast_1 = (_MaskSlider).xxxx;
				float dotResult13 = dot( tex2D( _Mask, uv_Mask ) , temp_cast_1 );
				float2 uv_SecondaryMask = IN.texcoord.xy * _SecondaryMask_ST.xy + _SecondaryMask_ST.zw;
				float4 temp_cast_2 = (_MaskSlider).xxxx;
				float dotResult20 = dot( tex2D( _SecondaryMask, uv_SecondaryMask ) , temp_cast_2 );
				float dotResult19 = dot( pow( dotResult13 , _Power ) , pow( dotResult20 , _Power ) );
				float4 appendResult6 = (float4(_BaseColor.rgb , ( ( tex2D( _MainTexture, uv_MainTexture ).a * saturate( dotResult19 ) ) * _Alpha )));
				
				fixed4 c = appendResult6;
				c.rgb *= c.a;
				return c;
			}
		ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18935
2833;621;1394;818;21.93988;570.4338;1.097878;True;False
Node;AmplifyShaderEditor.RangedFloatNode;9;-926.1268,377.5518;Inherit;False;Property;_MaskSlider;Mask Slider;3;0;Create;True;0;0;0;False;0;False;0;0.296;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;18;-920.1268,601.5519;Inherit;True;Property;_SecondaryMask;Secondary Mask;2;0;Create;True;0;0;0;False;0;False;-1;None;2bbde5fb33b8cc44ea395fd6777d8366;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;5;-965.1268,107.5517;Inherit;True;Property;_Mask;Mask;1;0;Create;True;0;0;0;False;0;False;-1;None;19b1e4058d61e5541954bdadbf131072;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;15;-929.1268,473.5517;Inherit;False;Property;_Power;Power;4;0;Create;True;0;0;0;False;0;False;1;29.39;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;20;-562.1269,580.5519;Inherit;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;13;-574.1269,232.5517;Inherit;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;14;-428.127,258.5517;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;21;-421.127,447.5517;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;19;-232.8782,346.4274;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;3;-213,-218;Inherit;True;Property;_MainTexture;Main Texture;0;0;Create;True;0;0;0;False;0;False;-1;None;411f67dec58f66844aebe7ee4d2becaa;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;17;-6,25;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;34;317.3074,39.74194;Inherit;False;Property;_Alpha;Alpha;6;0;Create;True;0;0;0;False;0;False;0;0.8430848;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;172,-112;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;32;141.3074,-422.2581;Inherit;False;Property;_BaseColor;Base Color;5;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;651.3074,-112.2581;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;6;782.9514,-267.7521;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;2;991.4999,-259.7388;Float;False;True;-1;2;ASEMaterialInspector;0;6;ClickerRPG/UI/TutorialBGPanelShader;0f8ba0101102bb14ebf021ddadce9b49;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;2;False;True;3;1;False;-1;10;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;20;0;18;0
WireConnection;20;1;9;0
WireConnection;13;0;5;0
WireConnection;13;1;9;0
WireConnection;14;0;13;0
WireConnection;14;1;15;0
WireConnection;21;0;20;0
WireConnection;21;1;15;0
WireConnection;19;0;14;0
WireConnection;19;1;21;0
WireConnection;17;0;19;0
WireConnection;16;0;3;4
WireConnection;16;1;17;0
WireConnection;35;0;16;0
WireConnection;35;1;34;0
WireConnection;6;0;32;0
WireConnection;6;3;35;0
WireConnection;2;0;6;0
ASEEND*/
//CHKSM=F83B04676D0092E173288D14DF7B448AEB69AAEC