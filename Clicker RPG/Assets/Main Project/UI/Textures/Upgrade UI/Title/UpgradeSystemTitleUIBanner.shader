// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ClickerRPG/UI/UpgradeSystem/UpgradeSystemTitleUIBanner"
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
		[NoScaleOffset]_Main("Main", 2D) = "white" {}
		[NoScaleOffset]_ScrollingMask("Scrolling Mask", 2D) = "white" {}
		_Tiling("Tiling", Vector) = (1,1,0,0)
		_Min("Min", Range( 0 , 1)) = 0.4705882
		_Max("Max", Range( 0 , 1)) = 0.6941177
		_Speed("Speed", Float) = 0
		_Power("Power", Float) = 0
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
			uniform sampler2D _Main;
			uniform float _Min;
			uniform float _Max;
			uniform sampler2D _ScrollingMask;
			uniform float _Speed;
			uniform float2 _Tiling;
			uniform float _Power;

			
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

				float2 uv_Main2 = IN.texcoord.xy;
				float4 tex2DNode2 = tex2D( _Main, uv_Main2 );
				float4 temp_cast_1 = (_Min).xxxx;
				float4 temp_cast_2 = (_Max).xxxx;
				float Speed44 = _Speed;
				float mulTime11 = _Time.y * Speed44;
				float2 texCoord9 = IN.texcoord.xy * _Tiling + float2( 0,0 );
				float2 panner10 = ( mulTime11 * float2( -1,0 ) + texCoord9);
				float2 texCoord16 = IN.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float smoothstepResult17 = smoothstep( 0.5 , 0.5 , texCoord16.x);
				float HalfMask18 = smoothstepResult17;
				float mulTime23 = _Time.y * Speed44;
				float2 texCoord24 = IN.texcoord.xy * _Tiling + float2( 0,0 );
				float2 panner25 = ( mulTime23 * float2( 1,0 ) + texCoord24);
				float4 temp_cast_3 = (_Power).xxxx;
				float4 smoothstepResult40 = smoothstep( temp_cast_1 , temp_cast_2 , ( tex2DNode2.a * ( tex2DNode2 + pow( ( ( tex2D( _ScrollingMask, panner10 ) * HalfMask18 ) + ( tex2D( _ScrollingMask, panner25 ) * ( 1.0 - HalfMask18 ) ) ) , temp_cast_3 ) ) ));
				float4 appendResult38 = (float4(tex2DNode2.rgb , smoothstepResult40.r));
				
				half4 color = ( IN.color * appendResult38 * tex2DNode2 );
				
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
2782;412;1394;879;303.5468;436.6154;1;True;False
Node;AmplifyShaderEditor.RangedFloatNode;43;-1201.645,1112.547;Inherit;False;Property;_Speed;Speed;5;0;Create;True;0;0;0;False;0;False;0;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;16;-1566.493,-1020.647;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;44;-1040.645,1114.547;Inherit;False;Speed;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;45;-1358.645,931.5471;Inherit;False;44;Speed;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;15;-1565.593,522.3421;Inherit;False;Property;_Tiling;Tiling;2;0;Create;True;0;0;0;False;0;False;1,1;1.75,0.66;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.GetLocalVarNode;46;-1248.645,394.5471;Inherit;False;44;Speed;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;17;-1331.493,-1003.647;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0.5;False;2;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;4;-1116.03,-613.5966;Inherit;True;Property;_ScrollingMask;Scrolling Mask;1;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;None;2b984ed6e4bd345498d0d57478516933;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TextureCoordinatesNode;9;-1118.377,229.3637;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;11;-1082.377,400.3637;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;24;-1215.311,788.6883;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;18;-1067.493,-979.6475;Inherit;False;HalfMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;23;-1179.311,959.6883;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;5;-853.2996,-610.4141;Inherit;False;Mask;-1;True;1;0;SAMPLER2D;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.GetLocalVarNode;26;-911.6823,726.0766;Inherit;False;5;Mask;1;0;OBJECT;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.PannerNode;10;-829.3767,276.3637;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-1,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;6;-814.7487,166.752;Inherit;False;5;Mask;1;0;OBJECT;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.PannerNode;25;-929.3102,835.6883;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;27;-720.1696,1230.135;Inherit;False;18;HalfMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;7;-602.8823,234.6994;Inherit;True;Property;_TextureSample0;Texture Sample 0;2;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;29;-526.1696,1217.635;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;21;-644.4877,831.5417;Inherit;True;Property;_TextureSample1;Texture Sample 1;2;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;20;-528.066,531.2568;Inherit;False;18;HalfMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-286.8695,924.8353;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-280.066,400.2568;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;47;72.13324,722.8032;Inherit;False;Property;_Power;Power;6;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;30;-9.414356,448.3749;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;2;-71.01766,53.26468;Inherit;True;Property;_Main;Main;0;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;29577da711b02f745a596d65d3899b0a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;34;207.9669,481.6467;Inherit;True;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;36;481.3322,334.6044;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;693.6419,147.6866;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;41;769.6419,377.6866;Inherit;False;Property;_Min;Min;3;0;Create;True;0;0;0;False;0;False;0.4705882;0.19;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;42;762.6419,460.6866;Inherit;False;Property;_Max;Max;4;0;Create;True;0;0;0;False;0;False;0.6941177;0.688;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;40;1136.642,199.6866;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;1;72.73976,-454.9208;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;38;752.6419,-17.31342;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;850.7397,-188.9208;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;1057.14,-234.3208;Float;False;True;-1;2;ASEMaterialInspector;0;4;ClickerRPG/UI/UpgradeSystem/UpgradeSystemTitleUIBanner;5056123faa0c79b47ab6ad7e8bf059a4;True;Default;0;0;Default;2;False;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;True;True;True;True;True;0;True;-9;False;False;False;False;False;False;False;True;True;0;True;-5;255;True;-8;255;True;-7;0;True;-4;0;True;-6;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;2;False;-1;True;0;True;-11;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;44;0;43;0
WireConnection;17;0;16;1
WireConnection;9;0;15;0
WireConnection;11;0;46;0
WireConnection;24;0;15;0
WireConnection;18;0;17;0
WireConnection;23;0;45;0
WireConnection;5;0;4;0
WireConnection;10;0;9;0
WireConnection;10;1;11;0
WireConnection;25;0;24;0
WireConnection;25;1;23;0
WireConnection;7;0;6;0
WireConnection;7;1;10;0
WireConnection;29;0;27;0
WireConnection;21;0;26;0
WireConnection;21;1;25;0
WireConnection;28;0;21;0
WireConnection;28;1;29;0
WireConnection;19;0;7;0
WireConnection;19;1;20;0
WireConnection;30;0;19;0
WireConnection;30;1;28;0
WireConnection;34;0;30;0
WireConnection;34;1;47;0
WireConnection;36;0;2;0
WireConnection;36;1;34;0
WireConnection;39;0;2;4
WireConnection;39;1;36;0
WireConnection;40;0;39;0
WireConnection;40;1;41;0
WireConnection;40;2;42;0
WireConnection;38;0;2;0
WireConnection;38;3;40;0
WireConnection;3;0;1;0
WireConnection;3;1;38;0
WireConnection;3;2;2;0
WireConnection;0;0;3;0
ASEEND*/
//CHKSM=17E4D82448201A141A69A44B5508477FEABDAD76