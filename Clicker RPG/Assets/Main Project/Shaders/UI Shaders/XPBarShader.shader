// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ClickerRPG/XPBarShader"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		
		_StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255

		_ColorMask ("Color Mask", Float) = 15

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
		[NoScaleOffset]_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Swipe("Swipe", Range( 0 , 1)) = 0
		[NoScaleOffset]_Shimmer("Shimmer", 2D) = "white" {}
		_TrailLength("Trail Length", Range( 0 , 1)) = 0
		_FlowSpeed("Flow Speed", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}

	SubShader
	{
		LOD 0

		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }
		
		Stencil
		{
			Ref [_Stencil]
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
			CompFront [_StencilComp]
			PassFront [_StencilOp]
			FailFront Keep
			ZFailFront Keep
			CompBack Always
			PassBack Keep
			FailBack Keep
			ZFailBack Keep
		}


		Cull Off
		Lighting Off
		ZWrite Off
		ZTest [unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
		ColorMask [_ColorMask]

		
		Pass
		{
			Name "Default"
		CGPROGRAM
			
			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

			#pragma multi_compile __ UNITY_UI_CLIP_RECT
			#pragma multi_compile __ UNITY_UI_ALPHACLIP
			
			#include "UnityShaderVariables.cginc"

			
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
				half2 texcoord  : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				
			};
			
			uniform fixed4 _Color;
			uniform fixed4 _TextureSampleAdd;
			uniform float4 _ClipRect;
			uniform sampler2D _MainTex;
			uniform sampler2D _TextureSample0;
			uniform float _Swipe;
			uniform sampler2D _Shimmer;
			uniform float _FlowSpeed;
			uniform float _TrailLength;

			
			v2f vert( appdata_t IN  )
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID( IN );
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
				OUT.worldPosition = IN.vertex;
				
				
				OUT.worldPosition.xyz +=  float3( 0, 0, 0 ) ;
				OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

				OUT.texcoord = IN.texcoord;
				
				OUT.color = IN.color * _Color;
				return OUT;
			}

			fixed4 frag(v2f IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				float2 uv_TextureSample01 = IN.texcoord.xy;
				float4 tex2DNode1 = tex2D( _TextureSample0, uv_TextureSample01 );
				float2 appendResult24 = (float2(_Swipe , 0.0));
				float2 texCoord16 = IN.texcoord.xy * float2( 1,1 ) + ( appendResult24 * -1 );
				float temp_output_27_0 = ( 1.0 - saturate( ceil( texCoord16.x ) ) );
				float Swipe35 = temp_output_27_0;
				float temp_output_15_0 = ( tex2DNode1.a * Swipe35 );
				float4 appendResult13 = (float4(tex2DNode1.r , tex2DNode1.g , tex2DNode1.b , temp_output_15_0));
				float MainAlpha60 = temp_output_15_0;
				float2 appendResult46 = (float2(( _ScreenParams.x * 0.005 ) , 1.0));
				float mulTime86 = _Time.y * _FlowSpeed;
				float2 panner50 = ( mulTime86 * float2( 1,0 ) + float2( 0,0 ));
				float2 texCoord42 = IN.texcoord.xy * appendResult46 + panner50;
				float4 temp_cast_0 = (5.0).xxxx;
				float4 color59 = IsGammaSpace() ? float4(1,1,1,0) : float4(1,1,1,0);
				float2 appendResult80 = (float2(( _TrailLength - _Swipe ) , 0.0));
				float2 texCoord70 = IN.texcoord.xy * float2( 1,1 ) + appendResult80;
				float temp_output_73_0 = min( texCoord70.x , temp_output_27_0 );
				float4 Shimmer62 = ( MainAlpha60 * ( ( pow( tex2D( _Shimmer, texCoord42 ) , temp_cast_0 ) * color59 * saturate( ( temp_output_73_0 * 5 ) ) ) + saturate( temp_output_73_0 ) ) );
				
				half4 color = ( appendResult13 + Shimmer62 );
				
				#ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif
				
				#ifdef UNITY_UI_ALPHACLIP
				clip (color.a - 0.001);
				#endif

				return color;
			}
		ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18935
2649;409;1535;948;1174.35;58.30493;1;True;False
Node;AmplifyShaderEditor.CommentaryNode;34;-1810.445,1126.648;Inherit;False;1368;400.0002;Swipe Logic;8;14;24;26;25;16;33;22;27;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-1760.445,1276.648;Inherit;False;Property;_Swipe;Swipe;1;0;Create;True;0;0;0;False;0;False;0;0.6;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.IntNode;26;-1448.445,1410.648;Inherit;False;Constant;_Int0;Int 0;2;0;Create;True;0;0;0;False;0;False;-1;0;False;0;1;INT;0
Node;AmplifyShaderEditor.DynamicAppendNode;24;-1446.492,1298.648;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-1295.445,1349.648;Inherit;False;2;2;0;FLOAT2;0,0;False;1;INT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;92;-2473.749,-194.6869;Inherit;False;1463.02;591.4;Shimmer;12;47;87;45;48;86;50;46;42;58;37;57;59;;0,0.7815788,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;16;-1161.445,1176.648;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;78;-1914.589,792.1168;Inherit;False;Property;_TrailLength;Trail Length;3;0;Create;True;0;0;0;False;0;False;0;0.13;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;81;-1594.144,802.2312;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenParams;45;-2250.33,-144.6869;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CeilOpNode;33;-929.4852,1197.01;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;47;-2223.33,44.31311;Inherit;False;Constant;_Float0;Float 0;3;0;Create;True;0;0;0;False;0;False;0.005;0.017;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;87;-2423.749,185.4208;Inherit;False;Property;_FlowSpeed;Flow Speed;4;0;Create;True;0;0;0;False;0;False;0;0.296;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;80;-1449.819,826.2502;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;86;-2147.64,190.591;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;48;-2049.33,5.313092;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;22;-794.4446,1195.648;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;27;-640.4446,1190.648;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;46;-1917.33,8.313091;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;50;-1957.33,118.313;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;70;-1246.856,766.1197;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.IntNode;85;-942.9392,994.9108;Inherit;False;Constant;_Int1;Int 1;5;0;Create;True;0;0;0;False;0;False;5;0;False;0;1;INT;0
Node;AmplifyShaderEditor.SimpleMinOpNode;73;-991.4891,755.1902;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;42;-1757.456,42.38602;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;35;-356.9381,1187.312;Inherit;False;Swipe;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;83;-759.4998,753.0344;Inherit;True;2;2;0;FLOAT;0;False;1;INT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;58;-1528.73,204.7131;Inherit;False;Constant;_Float1;Float 1;4;0;Create;True;0;0;0;False;0;False;5;5.34;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-438.1003,-223.7;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;1f4abd77bd7b451459a039c2c01726c6;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;36;-303.4563,-22.34532;Inherit;False;35;Swipe;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;37;-1529.019,8.453091;Inherit;True;Property;_Shimmer;Shimmer;2;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;e76609a6f6e7d254599e71bc5ddf0820;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-28.34241,-55.52607;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;57;-1213.729,78.71307;Inherit;False;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;59;-1243.729,184.7131;Inherit;False;Constant;_Color0;Color 0;4;0;Create;True;0;0;0;False;0;False;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;84;-535.5933,752.8168;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;-492.4287,236.5696;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;60;147.7893,-43.77002;Inherit;False;MainAlpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;91;-746.2782,634.7039;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;89;-280.5769,356.425;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;61;-282.8707,229.051;Inherit;False;60;MainAlpha;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;90;-71.2354,374.9304;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;62;87.29797,358.6534;Inherit;False;Shimmer;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;13;106.9001,-185.7;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;63;196.6773,130.3893;Inherit;False;62;Shimmer;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;55;405.9712,47.1131;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;12;584,65;Float;False;True;-1;2;ASEMaterialInspector;0;4;ClickerRPG/XPBarShader;5056123faa0c79b47ab6ad7e8bf059a4;True;Default;0;0;Default;2;True;True;2;5;False;-1;10;False;-1;2;5;False;-1;10;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;True;True;True;True;True;0;True;-9;False;False;False;False;False;False;False;True;True;0;True;-5;255;True;-8;255;True;-7;0;True;-4;0;True;-6;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;2;False;-1;True;0;True;-11;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;24;0;14;0
WireConnection;25;0;24;0
WireConnection;25;1;26;0
WireConnection;16;1;25;0
WireConnection;81;0;78;0
WireConnection;81;1;14;0
WireConnection;33;0;16;1
WireConnection;80;0;81;0
WireConnection;86;0;87;0
WireConnection;48;0;45;1
WireConnection;48;1;47;0
WireConnection;22;0;33;0
WireConnection;27;0;22;0
WireConnection;46;0;48;0
WireConnection;50;1;86;0
WireConnection;70;1;80;0
WireConnection;73;0;70;1
WireConnection;73;1;27;0
WireConnection;42;0;46;0
WireConnection;42;1;50;0
WireConnection;35;0;27;0
WireConnection;83;0;73;0
WireConnection;83;1;85;0
WireConnection;37;1;42;0
WireConnection;15;0;1;4
WireConnection;15;1;36;0
WireConnection;57;0;37;0
WireConnection;57;1;58;0
WireConnection;84;0;83;0
WireConnection;56;0;57;0
WireConnection;56;1;59;0
WireConnection;56;2;84;0
WireConnection;60;0;15;0
WireConnection;91;0;73;0
WireConnection;89;0;56;0
WireConnection;89;1;91;0
WireConnection;90;0;61;0
WireConnection;90;1;89;0
WireConnection;62;0;90;0
WireConnection;13;0;1;1
WireConnection;13;1;1;2
WireConnection;13;2;1;3
WireConnection;13;3;15;0
WireConnection;55;0;13;0
WireConnection;55;1;63;0
WireConnection;12;0;55;0
ASEEND*/
//CHKSM=7D6C22BC13A098C066768011D2C405B07429D21A