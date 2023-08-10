// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/PanningUITexture"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
		[NoScaleOffset]_Texture("Texture", 2D) = "white" {}
		_Color("Color", Color) = (0,0,0,0)
		_Alpha("Alpha", Range( 0 , 1)) = 1
		[NoScaleOffset]_ScrollingPattern("ScrollingPattern", 2D) = "white" {}
		_PanDirection("Pan Direction", Vector) = (0,0,0,0)
		_Tiling("Tiling", Vector) = (1,1,0,0)
		_ScrollingSpeed("Scrolling Speed", Range( 0 , 0.1)) = 0
		_ScrollingPatternVisbility("Scrolling Pattern Visbility", Range( 0 , 1)) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}

	SubShader
	{
		LOD 0

		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }

		Cull Off
		Lighting Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha, One One
		
		
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
				float2 texcoord  : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				
			};
			
			uniform fixed4 _Color;
			uniform float _EnableExternalAlpha;
			uniform sampler2D _MainTex;
			uniform sampler2D _AlphaTex;
			uniform sampler2D _Texture;
			uniform float _Alpha;
			uniform sampler2D _ScrollingPattern;
			uniform float _ScrollingSpeed;
			uniform float2 _PanDirection;
			uniform float2 _Tiling;
			uniform float _ScrollingPatternVisbility;

			
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

				float2 uv_Texture146 = IN.texcoord.xy;
				float4 break158 = ( _Color * tex2D( _Texture, uv_Texture146 ) );
				float4 temp_cast_0 = (_Alpha).xxxx;
				float mulTime141 = _Time.y * _ScrollingSpeed;
				float2 texCoord143 = IN.texcoord.xy * _Tiling + float2( 0,0 );
				float2 panner144 = ( mulTime141 * _PanDirection + texCoord143);
				float4 appendResult159 = (float4(break158.r , break158.g , break158.b , ( break158.a * saturate( ( temp_cast_0 - ( tex2D( _ScrollingPattern, panner144 ) * _ScrollingPatternVisbility ) ) ) ).r));
				
				fixed4 c = appendResult159;
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
2671;374;1535;948;1314.946;601.3364;1.3;True;False
Node;AmplifyShaderEditor.RangedFloatNode;139;-1334.625,418.7927;Inherit;False;Property;_ScrollingSpeed;Scrolling Speed;6;0;Create;True;0;0;0;False;0;False;0;0.0182;0;0.1;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;140;-1326.625,99.79272;Inherit;False;Property;_Tiling;Tiling;5;0;Create;True;0;0;0;False;0;False;1,1;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;143;-1108.625,130.7927;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;142;-1085.625,285.7927;Inherit;False;Property;_PanDirection;Pan Direction;4;0;Create;True;0;0;0;False;0;False;0,0;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleTimeNode;141;-1070.625,421.7927;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;144;-836.6249,228.7927;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;145;-607.6249,182.7927;Inherit;True;Property;_ScrollingPattern;ScrollingPattern;3;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;6406818b1353a3440999dca8bd3e0dc8;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;147;-589.6249,442.7927;Inherit;False;Property;_ScrollingPatternVisbility;Scrolling Pattern Visbility;7;0;Create;True;0;0;0;False;0;False;1;0.09;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;148;-599.6249,67.79272;Inherit;False;Property;_Alpha;Alpha;2;0;Create;True;0;0;0;False;0;False;1;0.144;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;146;-601.6249,-189.2073;Inherit;True;Property;_Texture;Texture;0;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;67ffe7b93490fb24a90430a977e9396c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;150;-279.6249,343.7927;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;151;-535.6249,-385.2073;Inherit;False;Property;_Color;Color;1;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;153;-38.62494,82.79272;Inherit;False;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;152;-223.6249,-268.2073;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.BreakToComponentsNode;158;-4.7771,-347.8102;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SaturateNode;160;106.2229,81.18982;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;198;255.3667,46.84045;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;159;343.2229,-194.8102;Inherit;False;COLOR;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;197;504,-44.2;Float;False;True;-1;2;ASEMaterialInspector;0;6;Custom/PanningUITexture;0f8ba0101102bb14ebf021ddadce9b49;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;2;True;True;2;5;False;-1;10;False;-1;4;1;False;-1;1;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;False;False;True;True;2;False;-1;False;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;143;0;140;0
WireConnection;141;0;139;0
WireConnection;144;0;143;0
WireConnection;144;2;142;0
WireConnection;144;1;141;0
WireConnection;145;1;144;0
WireConnection;150;0;145;0
WireConnection;150;1;147;0
WireConnection;153;0;148;0
WireConnection;153;1;150;0
WireConnection;152;0;151;0
WireConnection;152;1;146;0
WireConnection;158;0;152;0
WireConnection;160;0;153;0
WireConnection;198;0;158;3
WireConnection;198;1;160;0
WireConnection;159;0;158;0
WireConnection;159;1;158;1
WireConnection;159;2;158;2
WireConnection;159;3;198;0
WireConnection;197;0;159;0
ASEEND*/
//CHKSM=28F0EAD2B48D6D2E08F81B1273B1A9926CED1675