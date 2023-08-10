// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ClickerRPG/UI/UpgradeSystem/TextInfoObjectBGShader"
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
		_Slider("Slider", Range( 0 , 1)) = 0.7398014
		_Gap("Gap", Float) = 0.1
		_FadeColor("Fade Color", Color) = (1,1,1,0)
		[NoScaleOffset]_FadeTex("Fade Tex", 2D) = "white" {}

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
		Blend SrcAlpha OneMinusSrcAlpha
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
			#define ASE_NEEDS_FRAG_COLOR

			
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
			uniform float _Slider;
			uniform sampler2D _FadeTex;
			uniform float _Gap;
			uniform float4 _FadeColor;

			
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

				float temp_output_62_0 = (-0.2 + (_Slider - 0.0) * (1.4 - -0.2) / (1.0 - 0.0));
				float temp_output_7_0 = ( temp_output_62_0 + 0.1 );
				float2 texCoord3 = IN.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float smoothstepResult2 = smoothstep( temp_output_62_0 , temp_output_7_0 , texCoord3.x);
				float temp_output_8_0 = ( 1.0 - smoothstepResult2 );
				float4 temp_cast_0 = (0.09).xxxx;
				float4 temp_cast_1 = (0.671).xxxx;
				float2 texCoord48 = IN.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner47 = ( 0.25 * _Time.y * float2( 1,0 ) + texCoord48);
				float2 texCoord63 = IN.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner64 = ( 0.1 * _Time.y * float2( 1,1 ) + texCoord63);
				float smoothstepResult20 = smoothstep( ( temp_output_62_0 - _Gap ) , temp_output_7_0 , texCoord3.x);
				float4 Swipe30 = ( ( temp_output_8_0 - ( 1.0 - smoothstepResult20 ) ) * _FadeColor );
				float4 smoothstepResult58 = smoothstep( temp_cast_0 , temp_cast_1 , ( pow( min( min( tex2D( _FadeTex, panner47 ) , tex2D( _FadeTex, panner64 ) ) , Swipe30 ) , 2.0 ) + pow( Swipe30 , 8.0 ) ));
				float4 SwipeFX40 = smoothstepResult58;
				
				half4 color = ( ( IN.color * temp_output_8_0 ) + SwipeFX40 + pow( Swipe30 , 6.0 ) );
				
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
2729;516;1394;832;-860.8356;-1493.719;1;True;False
Node;AmplifyShaderEditor.RangedFloatNode;5;-1365.752,264.582;Inherit;False;Property;_Slider;Slider;0;0;Create;True;0;0;0;False;0;False;0.7398014;0.647;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-837.8694,556.9297;Inherit;False;Property;_Gap;Gap;1;0;Create;True;0;0;0;False;0;False;0.1;0.68;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-801.113,402.3033;Inherit;False;Constant;_Add;Add;0;0;Create;True;0;0;0;False;0;False;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;62;-1032.649,272.6441;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-0.2;False;4;FLOAT;1.4;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;3;-768.113,129.3032;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;21;-647.8694,539.9297;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;7;-647.1131,365.3033;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;20;-485.8694,504.9297;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0.1;False;2;FLOAT;0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;2;-497.1132,198.3032;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0.1;False;2;FLOAT;0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;32;-1153.765,835.6957;Inherit;True;Property;_FadeTex;Fade Tex;3;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;fa4c49b97cbbd33458e91e8507e6ffa0;2b984ed6e4bd345498d0d57478516933;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.OneMinusNode;24;-235.7796,500.175;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;8;-216.8914,197.2542;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;26;109.8117,485.5584;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;48;156.5919,1491.983;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;28;114.4999,719.7145;Inherit;False;Property;_FadeColor;Fade Color;2;0;Create;True;0;0;0;False;0;False;1,1,1,0;0,0.8397059,0.9150943,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;33;-912.6723,841.31;Inherit;False;FadeMaskTex;-1;True;1;0;SAMPLER2D;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;63;129.2394,1824.205;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;381.4999,558.7145;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PannerNode;64;363.4988,1837.68;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,1;False;1;FLOAT;0.1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;65;358.1258,1671.829;Inherit;False;33;FadeMaskTex;1;0;OBJECT;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.PannerNode;47;390.8513,1505.458;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,0;False;1;FLOAT;0.25;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;34;385.4783,1339.607;Inherit;False;33;FadeMaskTex;1;0;OBJECT;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SamplerNode;31;610.7674,1349.823;Inherit;True;Property;_FadeMask;FadeMask;3;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;30;554.0826,559.2118;Inherit;False;Swipe;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;66;583.4149,1682.045;Inherit;True;Property;_TextureSample0;Texture Sample 0;3;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;36;1041.567,1749.129;Inherit;False;30;Swipe;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMinOpNode;68;933.5697,1450.94;Inherit;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMinOpNode;39;1242.075,1654.548;Inherit;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;43;1195.588,2147.786;Inherit;False;30;Swipe;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;44;1391.905,2152.392;Inherit;False;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;8;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;45;1529.92,1662.196;Inherit;False;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;2;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;42;1691.733,1903.726;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;60;1604.343,2138.25;Inherit;False;Constant;_Float1;Float 1;4;0;Create;True;0;0;0;False;0;False;0.671;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;59;1599.343,2053.25;Inherit;False;Constant;_Float0;Float 0;5;0;Create;True;0;0;0;False;0;False;0.09;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;58;1914.325,1990.272;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;51;804.0635,231.7807;Inherit;False;30;Swipe;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;40;2143.778,1729.314;Inherit;False;SwipeFX;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;1;-108.2,-143.3;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;173,149.5;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;41;852.9371,99.51452;Inherit;False;40;SwipeFX;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;50;999.3802,236.3869;Inherit;False;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;6;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;27;1094.899,-53.24746;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;1317.603,-34.85902;Float;False;True;-1;2;ASEMaterialInspector;0;4;ClickerRPG/UI/UpgradeSystem/TextInfoObjectBGShader;5056123faa0c79b47ab6ad7e8bf059a4;True;Default;0;0;Default;2;False;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;True;True;True;True;True;0;True;-9;False;False;False;False;False;False;False;True;True;0;True;-5;255;True;-8;255;True;-7;0;True;-4;0;True;-6;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;2;False;-1;True;0;True;-11;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;62;0;5;0
WireConnection;21;0;62;0
WireConnection;21;1;22;0
WireConnection;7;0;62;0
WireConnection;7;1;6;0
WireConnection;20;0;3;1
WireConnection;20;1;21;0
WireConnection;20;2;7;0
WireConnection;2;0;3;1
WireConnection;2;1;62;0
WireConnection;2;2;7;0
WireConnection;24;0;20;0
WireConnection;8;0;2;0
WireConnection;26;0;8;0
WireConnection;26;1;24;0
WireConnection;33;0;32;0
WireConnection;29;0;26;0
WireConnection;29;1;28;0
WireConnection;64;0;63;0
WireConnection;47;0;48;0
WireConnection;31;0;34;0
WireConnection;31;1;47;0
WireConnection;30;0;29;0
WireConnection;66;0;65;0
WireConnection;66;1;64;0
WireConnection;68;0;31;0
WireConnection;68;1;66;0
WireConnection;39;0;68;0
WireConnection;39;1;36;0
WireConnection;44;0;43;0
WireConnection;45;0;39;0
WireConnection;42;0;45;0
WireConnection;42;1;44;0
WireConnection;58;0;42;0
WireConnection;58;1;59;0
WireConnection;58;2;60;0
WireConnection;40;0;58;0
WireConnection;4;0;1;0
WireConnection;4;1;8;0
WireConnection;50;0;51;0
WireConnection;27;0;4;0
WireConnection;27;1;41;0
WireConnection;27;2;50;0
WireConnection;0;0;27;0
ASEEND*/
//CHKSM=807C47E1DBAA514F5100274039C7AAE409F30B5D