// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ClickerRPG/UI/UpgradeLineShader"
{
	Properties
	{
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex ("Particle Texture", 2D) = "white" {}
		_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
		_MainBeam("MainBeam", 2D) = "white" {}
		[NoScaleOffset]_Waves("Waves", 2D) = "white" {}
		[NoScaleOffset]_CounterWaves("Counter Waves", 2D) = "white" {}
		[NoScaleOffset]_Noise("Noise", 2D) = "white" {}
		_Power("Power", Float) = 1
		_NoiseAlphaIntensity("NoiseAlphaIntensity", Float) = 1
		_FlowSpeed("Flow Speed", Float) = 0
		_LineCenterIntensity("Line Center Intensity", Float) = 1
		[NoScaleOffset]_Mask("Mask", 2D) = "white" {}
		_ScaleOffset("Scale Offset", Vector) = (1,1,0,0)
		_MaskAlpha("Mask Alpha", Range( 0 , 1)) = 1
		_RandomSeed("RandomSeed", Float) = 0
		_SlidingMask("SlidingMask", Range( 0 , 1)) = 0
		_ColorSlidingMask("Color Sliding Mask", Range( 0 , 2)) = 0
		_Pow("Pow", Float) = 3.3
		_InwardColorSecond("Inward Color Second", Color) = (0,0,0,0)
		_InwardColorFirst("Inward Color First", Color) = (0,0,0,0)
		_Alpha("Alpha", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}


	Category 
	{
		SubShader
		{
		LOD 0

			Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
			Blend One One
			ColorMask RGB
			Cull Off
			Lighting Off 
			ZWrite Off
			ZTest LEqual
			
			Pass {
			
				CGPROGRAM
				
				#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
				#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
				#endif
				
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 2.0
				#pragma multi_compile_instancing
				#pragma multi_compile_particles
				#pragma multi_compile_fog
				#include "UnityShaderVariables.cginc"
				#define ASE_NEEDS_FRAG_COLOR


				#include "UnityCG.cginc"

				struct appdata_t 
				{
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					float4 texcoord : TEXCOORD0;
					UNITY_VERTEX_INPUT_INSTANCE_ID
					
				};

				struct v2f 
				{
					float4 vertex : SV_POSITION;
					fixed4 color : COLOR;
					float4 texcoord : TEXCOORD0;
					UNITY_FOG_COORDS(1)
					#ifdef SOFTPARTICLES_ON
					float4 projPos : TEXCOORD2;
					#endif
					UNITY_VERTEX_INPUT_INSTANCE_ID
					UNITY_VERTEX_OUTPUT_STEREO
					
				};
				
				
				#if UNITY_VERSION >= 560
				UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
				#else
				uniform sampler2D_float _CameraDepthTexture;
				#endif

				//Don't delete this comment
				// uniform sampler2D_float _CameraDepthTexture;

				uniform sampler2D _MainTex;
				uniform fixed4 _TintColor;
				uniform float4 _MainTex_ST;
				uniform float _InvFade;
				uniform float4 _InwardColorFirst;
				uniform float4 _InwardColorSecond;
				uniform float _ColorSlidingMask;
				uniform float _Pow;
				uniform float _SlidingMask;
				uniform sampler2D _MainBeam;
				uniform float4 _MainBeam_ST;
				uniform float _LineCenterIntensity;
				uniform sampler2D _Waves;
				uniform float _FlowSpeed;
				uniform sampler2D _CounterWaves;
				uniform sampler2D _Noise;
				uniform float _NoiseAlphaIntensity;
				uniform float _Power;
				uniform sampler2D _Mask;
				uniform float2 _ScaleOffset;
				uniform float _RandomSeed;
				uniform float _MaskAlpha;
				uniform float _Alpha;


				v2f vert ( appdata_t v  )
				{
					v2f o;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
					UNITY_TRANSFER_INSTANCE_ID(v, o);
					

					v.vertex.xyz +=  float3( 0, 0, 0 ) ;
					o.vertex = UnityObjectToClipPos(v.vertex);
					#ifdef SOFTPARTICLES_ON
						o.projPos = ComputeScreenPos (o.vertex);
						COMPUTE_EYEDEPTH(o.projPos.z);
					#endif
					o.color = v.color;
					o.texcoord = v.texcoord;
					UNITY_TRANSFER_FOG(o,o.vertex);
					return o;
				}

				fixed4 frag ( v2f i  ) : SV_Target
				{
					UNITY_SETUP_INSTANCE_ID( i );
					UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( i );

					#ifdef SOFTPARTICLES_ON
						float sceneZ = LinearEyeDepth (SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
						float partZ = i.projPos.z;
						float fade = saturate (_InvFade * (sceneZ-partZ));
						i.color.a *= fade;
					#endif

					float4 lerpResult163 = lerp( _InwardColorFirst , _InwardColorSecond , ( 1.0 - i.color.a ));
					float dotResult140 = dot( ( 1.0 - i.color.a ) , i.color.a );
					float InwardMask148 = pow( ( 1.0 - dotResult140 ) , _Pow );
					float smoothstepResult150 = smoothstep( ( 1.0 - _ColorSlidingMask ) , 1.0 , InwardMask148);
					float4 lerpResult159 = lerp( float4( 1,1,1,0 ) , lerpResult163 , saturate( smoothstepResult150 ));
					float smoothstepResult144 = smoothstep( ( 1.0 - _SlidingMask ) , 1.0 , InwardMask148);
					float2 uv_MainBeam = i.texcoord.xy * _MainBeam_ST.xy + _MainBeam_ST.zw;
					float FlowSpeed83 = _FlowSpeed;
					float mulTime39 = _Time.y * FlowSpeed83;
					float2 texCoord40 = i.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
					float2 panner38 = ( mulTime39 * float2( 1,0 ) + texCoord40);
					float4 tex2DNode34 = tex2D( _Waves, panner38 );
					float temp_output_60_0 = ( FlowSpeed83 / 2.0 );
					float mulTime69 = _Time.y * temp_output_60_0;
					float2 texCoord70 = i.texcoord.xy * float2( 1,1 ) + float2( 0.5,0 );
					float2 panner71 = ( mulTime69 * float2( -1,0 ) + texCoord70);
					float dotResult76 = dot( tex2DNode34 , tex2D( _CounterWaves, panner71 ) );
					float mulTime56 = _Time.y * temp_output_60_0;
					float2 texCoord54 = i.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
					float2 panner57 = ( mulTime56 * float2( -1,0 ) + texCoord54);
					float4 appendResult63 = (float4(dotResult76 , dotResult76 , dotResult76 , saturate( ( tex2DNode34.a - pow( tex2D( _Noise, panner57 ).r , _NoiseAlphaIntensity ) ) )));
					float4 temp_cast_0 = (_Power).xxxx;
					float mulTime87 = _Time.y * ( FlowSpeed83 * 0.2 );
					float2 temp_cast_2 = (_RandomSeed).xx;
					float dotResult4_g1 = dot( temp_cast_2 , float2( 12.9898,78.233 ) );
					float lerpResult10_g1 = lerp( -1.0 , 1.0 , frac( ( sin( dotResult4_g1 ) * 43758.55 ) ));
					float temp_output_116_0 = lerpResult10_g1;
					float2 appendResult120 = (float2(temp_output_116_0 , temp_output_116_0));
					float2 texCoord89 = i.texcoord.xy * _ScaleOffset + appendResult120;
					float2 panner86 = ( mulTime87 * float2( -1,1 ) + texCoord89);
					float4 temp_cast_3 = (( 1.0 - _MaskAlpha )).xxxx;
					

					fixed4 col = ( lerpResult159 * ( saturate( smoothstepResult144 ) * ( ( ( tex2D( _MainBeam, uv_MainBeam ) * i.color * _LineCenterIntensity ) + ( i.color * saturate( pow( appendResult63 , temp_cast_0 ) ) ) ) * max( tex2D( _Mask, panner86 ) , temp_cast_3 ) ) ) * _Alpha );
					UNITY_APPLY_FOG(i.fogCoord, col);
					return col;
				}
				ENDCG 
			}
		}	
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18935
654;246;1394;912;-2516.836;958.7498;1;True;False
Node;AmplifyShaderEditor.RangedFloatNode;59;-1962.374,514.2469;Inherit;False;Property;_FlowSpeed;Flow Speed;6;0;Create;True;0;0;0;False;0;False;0;0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;83;-1754.861,517.9681;Inherit;False;FlowSpeed;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;84;-1491.861,930.9681;Inherit;False;83;FlowSpeed;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;60;-1249.365,1005.384;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;56;-1049.764,1034.584;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;54;-1044.764,878.5844;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;85;-1306.861,349.9681;Inherit;False;83;FlowSpeed;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;40;-1128,153;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;39;-1079,358;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;57;-796.7642,930.5844;Inherit;False;3;0;FLOAT2;1,0;False;2;FLOAT2;-1,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;147;750.0093,-1525;Inherit;False;1188.856;296.5977;Inward Mask;7;148;136;135;141;140;139;131;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleTimeNode;69;-1090.717,677.5079;Inherit;False;1;0;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;70;-1139.717,472.5079;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0.5,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;38;-875,246;Inherit;False;3;0;FLOAT2;1,0;False;2;FLOAT2;1,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;65;-487.958,1255.458;Inherit;False;Property;_NoiseAlphaIntensity;NoiseAlphaIntensity;5;0;Create;True;0;0;0;False;0;False;1;7.67;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;55;-599.7642,944.5844;Inherit;True;Property;_Noise;Noise;3;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;e76609a6f6e7d254599e71bc5ddf0820;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;64;-249.958,1134.458;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;131;800.0093,-1475;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;71;-886.717,565.5079;Inherit;False;3;0;FLOAT2;1,0;False;2;FLOAT2;-1,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;34;-658,209;Inherit;True;Property;_Waves;Waves;1;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;396f4378c10d25f488f18af2d6909c43;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;66;-211.4666,423.3395;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;68;-682.5359,458.5822;Inherit;True;Property;_CounterWaves;Counter Waves;2;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;34;None;fbedef8ac043127469178cbaa98ec488;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;122;687.1573,68.22638;Inherit;False;Property;_RandomSeed;RandomSeed;11;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;139;995.6654,-1473.203;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;88;924.0908,462.972;Inherit;False;83;FlowSpeed;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;67;-46.26668,421.1399;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;116;868.7051,70.711;Inherit;False;Random Range;-1;;1;7b754edb8aebbfb4a9ace907af661cfc;0;3;1;FLOAT2;1555,0;False;2;FLOAT;-1;False;3;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;76;-295.6332,274.4505;Inherit;False;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;140;1196.766,-1469.103;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;120;1096.157,231.2264;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;63;-14.37415,143.2469;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.Vector2Node;94;983.6451,-127.2399;Inherit;False;Property;_ScaleOffset;Scale Offset;9;0;Create;True;0;0;0;False;0;False;1,1;0.33,0.51;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;37;36.79999,281.9001;Inherit;False;Property;_Power;Power;4;0;Create;True;0;0;0;False;0;False;1;0.46;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;141;1341.865,-1452.403;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;90;1128.444,455.5287;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;136;1342.266,-1357.402;Inherit;False;Property;_Pow;Pow;14;0;Create;True;0;0;0;False;0;False;3.3;3.18;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;155;1651.368,-988.7864;Inherit;False;928.4979;307.684;Fade Inwards;5;144;137;149;133;146;;1,0,0,1;0;0
Node;AmplifyShaderEditor.PowerNode;135;1514.865,-1442.503;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;89;1204.711,-37.6721;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;-5,15;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;36;190.2999,185.6;Inherit;False;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;1;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleTimeNode;87;1299.817,436.1066;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;50;215.0213,-314.1767;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;13;200.5,-936.3998;Inherit;True;Property;_MainBeam;MainBeam;0;0;Create;True;0;0;0;False;0;False;-1;None;e684ef8968cd077448f5e98477e59574;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;78;325.8479,-731.0731;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;133;1701.368,-797.1024;Inherit;False;Property;_SlidingMask;SlidingMask;12;0;Create;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;86;1498.329,25.1643;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-1,1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SaturateNode;62;376.926,186.8469;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;95;1751.937,140.1298;Inherit;False;Property;_MaskAlpha;Mask Alpha;10;0;Create;True;0;0;0;False;0;False;1;0.802;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;148;1729.377,-1444.485;Inherit;False;InwardMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;80;349.2479,-546.4739;Inherit;False;Property;_LineCenterIntensity;Line Center Intensity;7;0;Create;True;0;0;0;False;0;False;1;9.43;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;153;2102.242,-1429.07;Inherit;False;Property;_ColorSlidingMask;Color Sliding Mask;13;0;Create;True;0;0;0;False;0;False;0;2;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;52;457.7209,-139.0768;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;149;2008.878,-938.7864;Inherit;False;148;InwardMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;102;2038.36,136.9105;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;81;1770.774,-81.67714;Inherit;True;Property;_Mask;Mask;8;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;fbedef8ac043127469178cbaa98ec488;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;160;2130.468,-1922.977;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;146;2040.96,-833.4696;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;152;2336.51,-1543.623;Inherit;False;148;InwardMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;154;2368.593,-1438.306;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;79;571.548,-814.2722;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SmoothstepOpNode;150;2539.3,-1524.139;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;101;2119.36,26.91052;Inherit;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;162;2416.573,-2279.416;Inherit;False;Property;_InwardColorFirst;Inward Color First;16;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,1,0.2608924,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;77;666.3668,-265.5495;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;161;2326.124,-1921.18;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;144;2236.667,-918.3032;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;158;2423.56,-2104.794;Inherit;False;Property;_InwardColorSecond;Inward Color Second;15;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0.5639737,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;137;2414.866,-911.9025;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;82;2057.374,-298.3774;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;163;2674.573,-1999.416;Inherit;True;3;0;COLOR;1,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;151;2705.499,-1523.739;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;159;3074.277,-1672.079;Inherit;True;3;0;COLOR;1,1,1,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;164;2935.836,-346.7498;Inherit;False;Property;_Alpha;Alpha;17;0;Create;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;125;2662.157,-547.7736;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;156;3262.405,-582.6461;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;51;3596.055,-588.9906;Float;False;True;-1;2;ASEMaterialInspector;0;7;ClickerRPG/UI/UpgradeLineShader;0b6a9f8b4f707c74ca64c0be8e590de0;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;2;True;True;4;1;False;-1;1;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;True;True;True;True;False;0;False;-1;False;False;False;False;False;False;False;False;False;True;2;False;-1;True;3;False;-1;False;True;4;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;False;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;83;0;59;0
WireConnection;60;0;84;0
WireConnection;56;0;60;0
WireConnection;39;0;85;0
WireConnection;57;0;54;0
WireConnection;57;1;56;0
WireConnection;69;0;60;0
WireConnection;38;0;40;0
WireConnection;38;1;39;0
WireConnection;55;1;57;0
WireConnection;64;0;55;1
WireConnection;64;1;65;0
WireConnection;71;0;70;0
WireConnection;71;1;69;0
WireConnection;34;1;38;0
WireConnection;66;0;34;4
WireConnection;66;1;64;0
WireConnection;68;1;71;0
WireConnection;139;0;131;4
WireConnection;67;0;66;0
WireConnection;116;1;122;0
WireConnection;76;0;34;0
WireConnection;76;1;68;0
WireConnection;140;0;139;0
WireConnection;140;1;131;4
WireConnection;120;0;116;0
WireConnection;120;1;116;0
WireConnection;63;0;76;0
WireConnection;63;1;76;0
WireConnection;63;2;76;0
WireConnection;63;3;67;0
WireConnection;141;0;140;0
WireConnection;90;0;88;0
WireConnection;135;0;141;0
WireConnection;135;1;136;0
WireConnection;89;0;94;0
WireConnection;89;1;120;0
WireConnection;36;0;63;0
WireConnection;36;1;37;0
WireConnection;87;0;90;0
WireConnection;86;0;89;0
WireConnection;86;1;87;0
WireConnection;62;0;36;0
WireConnection;148;0;135;0
WireConnection;52;0;50;0
WireConnection;52;1;62;0
WireConnection;102;0;95;0
WireConnection;81;1;86;0
WireConnection;146;0;133;0
WireConnection;154;0;153;0
WireConnection;79;0;13;0
WireConnection;79;1;78;0
WireConnection;79;2;80;0
WireConnection;150;0;152;0
WireConnection;150;1;154;0
WireConnection;101;0;81;0
WireConnection;101;1;102;0
WireConnection;77;0;79;0
WireConnection;77;1;52;0
WireConnection;161;0;160;4
WireConnection;144;0;149;0
WireConnection;144;1;146;0
WireConnection;137;0;144;0
WireConnection;82;0;77;0
WireConnection;82;1;101;0
WireConnection;163;0;162;0
WireConnection;163;1;158;0
WireConnection;163;2;161;0
WireConnection;151;0;150;0
WireConnection;159;1;163;0
WireConnection;159;2;151;0
WireConnection;125;0;137;0
WireConnection;125;1;82;0
WireConnection;156;0;159;0
WireConnection;156;1;125;0
WireConnection;156;2;164;0
WireConnection;51;0;156;0
ASEEND*/
//CHKSM=804152790D877B82A01E8B4A4DB7FB5649999960