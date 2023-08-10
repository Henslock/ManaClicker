// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ClickerRPG/UI/ResetSwipeVFXShader"
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
		[NoScaleOffset]_Stars("Stars", 2D) = "white" {}
		_StarTiling("Star Tiling", Vector) = (1,1,0,0)
		_DistortionStrength("Distortion Strength", Range( 0 , 1)) = 1
		_Alpha("Alpha", Range( 0 , 1)) = 1
		[NoScaleOffset]_DistortionNormalInput("Distortion Normal Input", 2D) = "bump" {}
		_StarsMask("StarsMask", 2D) = "white" {}
		_SwipeTexture("SwipeTexture", 2D) = "white" {}
		_InverseSwipe("InverseSwipe", 2D) = "white" {}
		_MainScroll("Main Scroll", Range( 0 , 1)) = 0
		_Power("Power", Float) = 1.37
		_Warp("Warp", Range( 0 , 1)) = 0.06
		_FakeScreen("Fake Screen", 2D) = "white" {}
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

		GrabPass{ }

		Pass
		{
			Name "Default"
		CGPROGRAM
			#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
			#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex);
			#else
			#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex)
			#endif

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
				float4 ase_texcoord2 : TEXCOORD2;
			};
			
			uniform fixed4 _Color;
			uniform fixed4 _TextureSampleAdd;
			uniform float4 _ClipRect;
			uniform sampler2D _MainTex;
			uniform sampler2D _FakeScreen;
			uniform sampler2D _Main;
			uniform float _MainScroll;
			uniform sampler2D _DistortionNormalInput;
			uniform float _Warp;
			uniform sampler2D _Stars;
			uniform float2 _StarTiling;
			uniform sampler2D _StarsMask;
			uniform float4 _StarsMask_ST;
			ASE_DECLARE_SCREENSPACE_TEXTURE( _GrabTexture )
			uniform float _DistortionStrength;
			uniform float _Alpha;
			uniform sampler2D _SwipeTexture;
			uniform float4 _SwipeTexture_ST;
			uniform float _Power;
			uniform sampler2D _InverseSwipe;
			inline float4 ASE_ComputeGrabScreenPos( float4 pos )
			{
				#if UNITY_UV_STARTS_AT_TOP
				float scale = -1.0;
				#else
				float scale = 1.0;
				#endif
				float4 o = pos;
				o.y = pos.w * 0.5f;
				o.y = ( pos.y - o.y ) * _ProjectionParams.x * scale + o.y;
				return o;
			}
			

			
			v2f vert( appdata_t IN  )
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID( IN );
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
				OUT.worldPosition = IN.vertex;
				float4 ase_clipPos = UnityObjectToClipPos(IN.vertex);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				OUT.ase_texcoord2 = screenPos;
				
				
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

				float2 texCoord184 = IN.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult188 = (float2(texCoord184.x , ( 1.0 - texCoord184.y )));
				float4 temp_cast_0 = (0.03).xxxx;
				float4 temp_cast_1 = (1.1).xxxx;
				float2 appendResult154 = (float2(0.0 , ( 1.0 - (-0.3 + (_MainScroll - 0.0) * (2.0 - -0.3) / (1.0 - 0.0)) )));
				float2 texCoord156 = IN.texcoord.xy * float2( 1,1 ) + appendResult154;
				float mulTime51 = _Time.y * 0.02;
				float2 texCoord50 = IN.texcoord.xy * float2( 0.5,0.3 ) + float2( 0,0 );
				float2 panner49 = ( mulTime51 * float2( 0,1 ) + texCoord50);
				float mulTime72 = _Time.y * 0.02;
				float2 texCoord73 = IN.texcoord.xy * float2( 0.2,0.3 ) + float2( 0,0 );
				float2 panner74 = ( mulTime72 * float2( 0,1 ) + texCoord73);
				float3 temp_output_77_0 = min( UnpackNormal( tex2D( _DistortionNormalInput, panner49 ) ) , UnpackNormal( tex2D( _DistortionNormalInput, panner74 ) ) );
				float3 DistortionScrollingNormalsTex167 = temp_output_77_0;
				float4 tex2DNode1 = tex2D( _Main, ( float3( texCoord156 ,  0.0 ) + ( DistortionScrollingNormalsTex167 * _Warp ) ).xy );
				float smoothstepResult21 = smoothstep( 0.03 , 1.1 , tex2DNode1.r);
				float mulTime90 = _Time.y * 0.05;
				float2 texCoord89 = IN.texcoord.xy * _StarTiling + float2( 0,0 );
				float2 panner88 = ( mulTime90 * float2( 0.15,1 ) + texCoord89);
				float4 smoothstepResult18 = smoothstep( temp_cast_0 , temp_cast_1 , ( smoothstepResult21 * tex2D( _Stars, panner88 ) ));
				float4 temp_cast_5 = (0.59).xxxx;
				float4 temp_cast_6 = (0.49).xxxx;
				float2 uv_StarsMask = IN.texcoord.xy * _StarsMask_ST.xy + _StarsMask_ST.zw;
				float4 smoothstepResult98 = smoothstep( temp_cast_5 , temp_cast_6 , tex2D( _StarsMask, uv_StarsMask ));
				float4 appendResult95 = (float4(smoothstepResult18.rgb , ( smoothstepResult18.a * smoothstepResult98 ).r));
				float4 Stars31 = appendResult95;
				float4 screenPos = IN.ase_texcoord2;
				float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( screenPos );
				float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
				float4 Swipe30 = tex2DNode1;
				float4 screenColor26 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,( float4( (ase_grabScreenPosNorm).xy, 0.0 , 0.0 ) + ( Swipe30 * float4( temp_output_77_0 , 0.0 ) * _DistortionStrength ) ).rg);
				float4 Distortion39 = screenColor26;
				float2 uv_SwipeTexture = IN.texcoord.xy * _SwipeTexture_ST.xy + _SwipeTexture_ST.zw;
				float4 temp_cast_11 = (0.28).xxxx;
				float4 temp_cast_12 = (1.52).xxxx;
				float4 smoothstepResult128 = smoothstep( temp_cast_11 , temp_cast_12 , Swipe30);
				float2 texCoord119 = IN.texcoord.xy * float2( 1,1 ) + float2( 1,0 );
				float4 temp_cast_13 = (_Power).xxxx;
				float4 OutSwipe111 = pow( ( ( Swipe30 * _Alpha * IN.color ) * min( ( tex2D( _SwipeTexture, uv_SwipeTexture ) + smoothstepResult128 ) , ( tex2D( _SwipeTexture, texCoord119 ) + smoothstepResult128 ) ) ) , temp_cast_13 );
				float2 MainScroll158 = appendResult154;
				float2 texCoord197 = IN.texcoord.xy * float2( 1,1 ) + MainScroll158;
				float WarpVal205 = _Warp;
				float4 lerpResult178 = lerp( tex2D( _FakeScreen, appendResult188 ) , ( ( ( IN.color * Stars31.w ) + Distortion39 ) + OutSwipe111 ) , ( 1.0 - tex2D( _InverseSwipe, ( float3( texCoord197 ,  0.0 ) + ( DistortionScrollingNormalsTex167 * WarpVal205 ) ).xy ) ));
				
				half4 color = lerpResult178;
				
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
2759;445;1394;879;-1443.481;922.6642;2.058334;True;False
Node;AmplifyShaderEditor.Vector2Node;71;298.9312,-1283.934;Inherit;False;Constant;_Vector1;Vector 1;4;0;Create;True;0;0;0;False;0;False;0.2,0.3;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TexturePropertyNode;53;-224.1434,-781.8737;Inherit;True;Property;_DistortionNormalInput;Distortion Normal Input;5;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;5b653e484c8e303439ef414b62f969f0;5b653e484c8e303439ef414b62f969f0;True;bump;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.Vector2Node;52;344.2826,-1594.288;Inherit;False;Constant;_Vector0;Vector 0;4;0;Create;True;0;0;0;False;0;False;0.5,0.3;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RegisterLocalVarNode;54;34.85665,-757.8736;Inherit;False;DistortionTex;-1;True;1;0;SAMPLER2D;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;73;505.9311,-1255.934;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;50;551.2825,-1566.288;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;72;570.9312,-1099.934;Inherit;False;1;0;FLOAT;0.02;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;51;616.2826,-1410.288;Inherit;False;1;0;FLOAT;0.02;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;74;773.9258,-1223.751;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;49;819.2772,-1534.105;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;55;808.2828,-1627.288;Inherit;False;54;DistortionTex;1;0;OBJECT;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.GetLocalVarNode;57;796.4298,-1318.888;Inherit;False;54;DistortionTex;1;0;OBJECT;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SamplerNode;35;1023.318,-1564.977;Inherit;True;Property;_DistortionNormals;Distortion Normals;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;153;-1730.99,-190.6952;Inherit;False;Property;_MainScroll;Main Scroll;9;0;Create;True;0;0;0;False;0;False;0;0.409;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;56;1000.43,-1301.888;Inherit;True;Property;_TextureSample0;Texture Sample 0;4;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMinOpNode;77;1332.939,-1432.761;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TFHCRemapNode;171;-1404.536,-164.0765;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-0.3;False;4;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;167;1699.737,-1346.516;Inherit;False;DistortionScrollingNormalsTex;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OneMinusNode;157;-1222.638,-181.9237;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;154;-1003.357,-191.2107;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;166;-1120.528,192.2123;Inherit;False;167;DistortionScrollingNormalsTex;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;169;-1080.418,291.4584;Inherit;False;Property;_Warp;Warp;11;0;Create;True;0;0;0;False;0;False;0.06;0.548;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;123;-869.6257,488.3422;Inherit;False;Property;_StarTiling;Star Tiling;2;0;Create;True;0;0;0;False;0;False;1,1;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;168;-543.7759,298.8503;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;156;-629.2601,44.83395;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;165;-364.6284,122.9123;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleTimeNode;90;-611.9012,656.0219;Inherit;False;1;0;FLOAT;0.05;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;89;-675.9012,519.0219;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;19;-49.12145,346.6271;Inherit;False;Constant;_Min;Min;3;0;Create;True;0;0;0;False;0;False;0.03;0.05;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;88;-408.2862,554.9271;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.15,1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;1;-199.9896,75.42207;Inherit;True;Property;_Main;Main;0;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;f3cec46e621ba934399ca057c1512059;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;20;-39.12145,433.6272;Inherit;False;Constant;_Max;Max;6;0;Create;True;0;0;0;False;0;False;1.1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;21;164.8785,344.6271;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;13;-187.1215,538.6272;Inherit;True;Property;_Stars;Stars;1;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;33df4c11eedfe2549bed6f519df586a5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;340.8787,413.6272;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TexturePropertyNode;109;-214.0848,-541.7837;Inherit;True;Property;_SwipeTexture;SwipeTexture;7;0;Create;True;0;0;0;False;0;False;None;fa4c49b97cbbd33458e91e8507e6ffa0;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SmoothstepOpNode;18;500.0709,469.7043;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;30;307.133,67.31408;Inherit;False;Swipe;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;99;80.58582,1014.868;Inherit;False;Constant;_Float1;Float 1;5;0;Create;True;0;0;0;False;0;False;0.59;0.05;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;100;90.58582,1101.868;Inherit;False;Constant;_Float2;Float 2;6;0;Create;True;0;0;0;False;0;False;0.49;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;91;-156.9012,788.0219;Inherit;True;Property;_StarsMask;StarsMask;6;0;Create;True;0;0;0;False;0;False;-1;None;2ad48d9c4992dc44982d8e472fbe4522;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;110;12.9147,-532.7836;Inherit;False;SwipeTexture;-1;True;1;0;SAMPLER2D;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.CommentaryNode;136;886.5625,1346.254;Inherit;False;1220.554;928.5325;Comment;12;119;126;127;125;116;115;128;117;113;130;131;122;Ethereal Swipe;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;125;1238.27,1947.016;Inherit;False;30;Swipe;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;127;1253.131,2158.787;Inherit;False;Constant;_Float4;Float 4;6;0;Create;True;0;0;0;False;0;False;1.52;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;115;1136.562,1414.122;Inherit;False;110;SwipeTexture;1;0;OBJECT;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.RangedFloatNode;126;1243.131,2071.787;Inherit;False;Constant;_Float3;Float 3;3;0;Create;True;0;0;0;False;0;False;0.28;0.05;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;96;692.7017,600.4464;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.GetLocalVarNode;116;1139.752,1654.005;Inherit;False;110;SwipeTexture;1;0;OBJECT;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SmoothstepOpNode;98;318.7017,865.4464;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;119;1103.563,1741.421;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;1,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GrabScreenPosition;24;1056.488,-1857.937;Inherit;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;23;1028.51,-1079.741;Inherit;False;Property;_DistortionStrength;Distortion Strength;3;0;Create;True;0;0;0;False;0;False;1;0.108;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;97;841.7017,794.4464;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;113;1336.184,1396.254;Inherit;True;Property;_TextureSample1;Texture Sample 1;8;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;128;1527.13,1957.787;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;34;1128.295,-1662.461;Inherit;False;30;Swipe;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;117;1339.373,1660.838;Inherit;True;Property;_TextureSample2;Texture Sample 2;8;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;130;1787.174,1717.741;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ComponentMaskNode;25;1290.488,-1856.937;Inherit;False;True;True;False;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;95;973.7017,595.4464;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;131;1795.558,1499.713;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;103;1424.237,963.5995;Inherit;False;Property;_Alpha;Alpha;4;0;Create;True;0;0;0;False;0;False;1;0.397;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;1526.942,-1557.812;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;107;1484.905,1063.552;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;101;1542.111,880.1835;Inherit;False;30;Swipe;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;104;1734.63,917.7007;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;31;1151.12,599.7355;Inherit;False;Stars;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;205;-793.1488,390.6079;Inherit;False;WarpVal;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;28;1621.488,-1714.938;Inherit;False;2;2;0;FLOAT2;0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;158;-638.2297,-207.5941;Inherit;False;MainScroll;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMinOpNode;122;1955.117,1584.949;Inherit;False;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;203;2948.109,197.7058;Inherit;False;158;MainScroll;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;201;2878.437,320.9591;Inherit;False;167;DistortionScrollingNormalsTex;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;206;2973.977,426.2485;Inherit;False;205;WarpVal;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;106;2569.85,1054.324;Inherit;False;Property;_Power;Power;10;0;Create;True;0;0;0;False;0;False;1.37;1.88;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;32;2253.231,-399.9799;Inherit;False;31;Stars;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;114;2147.232,952.5596;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TexturePropertyNode;192;-225.5341,-992.3835;Inherit;True;Property;_InverseSwipe;InverseSwipe;8;0;Create;True;0;0;0;False;0;False;None;71275b4027c3690469430ec6ce77672a;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.ScreenColorNode;26;1756.489,-1716.837;Inherit;False;Global;_GrabScreen0;Grab Screen 0;3;0;Create;True;0;0;0;False;0;False;Object;-1;False;False;False;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;39;2022.124,-1711.46;Inherit;False;Distortion;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;184;2955.472,-879.4671;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;105;2731.85,963.324;Inherit;False;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.BreakToComponentsNode;84;2478.66,-405.5974;Inherit;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.TextureCoordinatesNode;197;3171.204,159.3807;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;198;3251.489,383.4971;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.VertexColorNode;10;2483.895,-635.1821;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;191;48.63402,-956.655;Inherit;False;InverseSwipeTexture;-1;True;1;0;SAMPLER2D;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SimpleAddOpNode;199;3473.535,243.9591;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;40;2712.428,-265.2978;Inherit;False;39;Distortion;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;2736.499,-561.893;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;186;3169.472,-869.4671;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;111;3119.561,974.5223;Inherit;False;OutSwipe;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;194;3262.851,-26.39049;Inherit;False;191;InverseSwipeTexture;1;0;OBJECT;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SimpleAddOpNode;86;3015.813,-415.5394;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;112;3050.564,-282.121;Inherit;False;111;OutSwipe;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;188;3351.472,-765.467;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;173;3188.293,-1083.521;Inherit;True;Property;_FakeScreen;Fake Screen;12;0;Create;True;0;0;0;False;0;False;None;84508b93f15f2b64386ec07486afc7a3;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SamplerNode;193;3665.851,-35.39049;Inherit;True;Property;_InverseSlider;Inverse Slider;13;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;102;3313.614,-392.634;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;174;3497.604,-897.6254;Inherit;True;Property;_TextureSample3;Texture Sample 3;13;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;204;4039.253,-11.9295;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;178;3962.591,-501.6697;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;151;525.77,975.8153;Inherit;False;StarsMask;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;4280.497,-808.6471;Float;False;True;-1;2;ASEMaterialInspector;0;4;ClickerRPG/UI/ResetSwipeVFXShader;5056123faa0c79b47ab6ad7e8bf059a4;True;Default;0;0;Default;2;True;True;2;5;False;-1;10;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;True;True;True;True;True;0;True;-9;False;False;False;False;False;False;False;True;True;0;True;-5;255;True;-8;255;True;-7;0;True;-4;0;True;-6;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;2;False;-1;True;0;True;-11;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;54;0;53;0
WireConnection;73;0;71;0
WireConnection;50;0;52;0
WireConnection;74;0;73;0
WireConnection;74;1;72;0
WireConnection;49;0;50;0
WireConnection;49;1;51;0
WireConnection;35;0;55;0
WireConnection;35;1;49;0
WireConnection;56;0;57;0
WireConnection;56;1;74;0
WireConnection;77;0;35;0
WireConnection;77;1;56;0
WireConnection;171;0;153;0
WireConnection;167;0;77;0
WireConnection;157;0;171;0
WireConnection;154;1;157;0
WireConnection;168;0;166;0
WireConnection;168;1;169;0
WireConnection;156;1;154;0
WireConnection;165;0;156;0
WireConnection;165;1;168;0
WireConnection;89;0;123;0
WireConnection;88;0;89;0
WireConnection;88;1;90;0
WireConnection;1;1;165;0
WireConnection;21;0;1;1
WireConnection;21;1;19;0
WireConnection;21;2;20;0
WireConnection;13;1;88;0
WireConnection;15;0;21;0
WireConnection;15;1;13;0
WireConnection;18;0;15;0
WireConnection;18;1;19;0
WireConnection;18;2;20;0
WireConnection;30;0;1;0
WireConnection;110;0;109;0
WireConnection;96;0;18;0
WireConnection;98;0;91;0
WireConnection;98;1;99;0
WireConnection;98;2;100;0
WireConnection;97;0;96;3
WireConnection;97;1;98;0
WireConnection;113;0;115;0
WireConnection;128;0;125;0
WireConnection;128;1;126;0
WireConnection;128;2;127;0
WireConnection;117;0;116;0
WireConnection;117;1;119;0
WireConnection;130;0;117;0
WireConnection;130;1;128;0
WireConnection;25;0;24;0
WireConnection;95;0;18;0
WireConnection;95;3;97;0
WireConnection;131;0;113;0
WireConnection;131;1;128;0
WireConnection;36;0;34;0
WireConnection;36;1;77;0
WireConnection;36;2;23;0
WireConnection;104;0;101;0
WireConnection;104;1;103;0
WireConnection;104;2;107;0
WireConnection;31;0;95;0
WireConnection;205;0;169;0
WireConnection;28;0;25;0
WireConnection;28;1;36;0
WireConnection;158;0;154;0
WireConnection;122;0;131;0
WireConnection;122;1;130;0
WireConnection;114;0;104;0
WireConnection;114;1;122;0
WireConnection;26;0;28;0
WireConnection;39;0;26;0
WireConnection;105;0;114;0
WireConnection;105;1;106;0
WireConnection;84;0;32;0
WireConnection;197;1;203;0
WireConnection;198;0;201;0
WireConnection;198;1;206;0
WireConnection;191;0;192;0
WireConnection;199;0;197;0
WireConnection;199;1;198;0
WireConnection;44;0;10;0
WireConnection;44;1;84;3
WireConnection;186;0;184;2
WireConnection;111;0;105;0
WireConnection;86;0;44;0
WireConnection;86;1;40;0
WireConnection;188;0;184;1
WireConnection;188;1;186;0
WireConnection;193;0;194;0
WireConnection;193;1;199;0
WireConnection;102;0;86;0
WireConnection;102;1;112;0
WireConnection;174;0;173;0
WireConnection;174;1;188;0
WireConnection;204;0;193;0
WireConnection;178;0;174;0
WireConnection;178;1;102;0
WireConnection;178;2;204;0
WireConnection;151;0;98;0
WireConnection;0;0;178;0
ASEEND*/
//CHKSM=EAE2F55F9C195679778A4CF1F68FDC17840087A4