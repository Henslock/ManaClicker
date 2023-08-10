// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ClickerRPG/UI/UnitSelection/BackgroundFire"
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
		[NoScaleOffset]_MainTexture("Main Texture", 2D) = "white" {}
		[NoScaleOffset]_Mask("Mask", 2D) = "white" {}
		[NoScaleOffset]_SecondaryTexture("Secondary Texture", 2D) = "white" {}
		_MainTextureUVTiling("Main Texture UV Tiling", Vector) = (1,1,0,0)
		_SecondaryTextureUVTiling("Secondary Texture UV Tiling", Vector) = (1,1,0,0)
		_AnimationPower("Animation Power", Float) = 1
		_Speed("Speed", Range( 0 , 1)) = 1
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
		Blend SrcAlpha One
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
			uniform sampler2D _MainTexture;
			uniform float _Speed;
			uniform float2 _MainTextureUVTiling;
			uniform sampler2D _SecondaryTexture;
			uniform float2 _SecondaryTextureUVTiling;
			uniform float _AnimationPower;
			uniform sampler2D _Mask;

			
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

				float mulTime9 = _Time.y * _Speed;
				float2 appendResult20 = (float2(_MainTextureUVTiling));
				float2 texCoord8 = IN.texcoord.xy * appendResult20 + float2( 0,0 );
				float2 panner7 = ( mulTime9 * float2( 0,-1 ) + texCoord8);
				float mulTime14 = _Time.y * _Speed;
				float2 appendResult22 = (float2(_SecondaryTextureUVTiling));
				float2 texCoord13 = IN.texcoord.xy * appendResult22 + float2( 0,0 );
				float2 panner15 = ( mulTime14 * float2( 0,-0.5 ) + texCoord13);
				float4 temp_cast_0 = (_AnimationPower).xxxx;
				float2 uv_Mask3 = IN.texcoord.xy;
				
				half4 color = ( IN.color * pow( max( tex2D( _MainTexture, panner7 ) , tex2D( _SecondaryTexture, panner15 ) ) , temp_cast_0 ) * tex2D( _Mask, uv_Mask3 ) );
				
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
2744;447;1394;912;1229.831;252.8096;1.050129;True;False
Node;AmplifyShaderEditor.Vector2Node;19;-1465.104,-148.5336;Inherit;False;Property;_MainTextureUVTiling;Main Texture UV Tiling;3;0;Create;True;0;0;0;False;0;False;1,1;1.06,0.65;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;21;-1649.104,177.4664;Inherit;False;Property;_SecondaryTextureUVTiling;Secondary Texture UV Tiling;4;0;Create;True;0;0;0;False;0;False;1,1;0.87,0.7;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.DynamicAppendNode;20;-1145.104,-98.53357;Inherit;False;FLOAT2;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;24;-1345.104,46.46643;Inherit;False;Property;_Speed;Speed;6;0;Create;True;0;0;0;False;0;False;1;0.1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;22;-1363.104,155.4664;Inherit;False;FLOAT2;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;9;-959.9248,54.7092;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;8;-989.9248,-77.2908;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;13;-1073.592,217.0426;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;14;-1043.592,349.0426;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;7;-717.9248,-57.2908;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,-1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;15;-801.5915,237.0426;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,-0.5;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;4;-493.9247,192.7092;Inherit;True;Property;_SecondaryTexture;Secondary Texture;2;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;2b984ed6e4bd345498d0d57478516933;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-508,-57;Inherit;True;Property;_MainTexture;Main Texture;0;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;fa4c49b97cbbd33458e91e8507e6ffa0;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMaxOpNode;16;-114.9248,83.70923;Inherit;False;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;23;-111.1036,343.4664;Inherit;False;Property;_AnimationPower;Animation Power;5;0;Create;True;0;0;0;False;0;False;1;0.92;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;1;291.0899,-256.6045;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;3;316.8983,326.2501;Inherit;True;Property;_Mask;Mask;1;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;40fd3ee39b47b0b4f9694e1cef3615cb;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;18;77.89636,219.4664;Inherit;False;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;473.0752,-112.2908;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;710,-157;Float;False;True;-1;2;ASEMaterialInspector;0;4;ClickerRPG/UI/UnitSelection/BackgroundFire;5056123faa0c79b47ab6ad7e8bf059a4;True;Default;0;0;Default;2;True;True;8;5;False;-1;1;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;True;True;True;True;True;0;True;-9;False;False;False;False;False;False;True;True;True;1;True;-5;255;True;-8;255;True;-7;0;True;-4;0;True;-6;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;2;False;-1;True;0;True;-11;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;20;0;19;0
WireConnection;22;0;21;0
WireConnection;9;0;24;0
WireConnection;8;0;20;0
WireConnection;13;0;22;0
WireConnection;14;0;24;0
WireConnection;7;0;8;0
WireConnection;7;1;9;0
WireConnection;15;0;13;0
WireConnection;15;1;14;0
WireConnection;4;1;15;0
WireConnection;2;1;7;0
WireConnection;16;0;2;0
WireConnection;16;1;4;0
WireConnection;18;0;16;0
WireConnection;18;1;23;0
WireConnection;17;0;1;0
WireConnection;17;1;18;0
WireConnection;17;2;3;0
WireConnection;0;0;17;0
ASEEND*/
//CHKSM=B4788B1D3F891E6955D70A727E3F5BE53A94DEF6