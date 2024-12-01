Shader "Unlit/Transparent Colored (Packed) (TextureClip)" {
Properties {
 _MainTex ("Base (RGB), Alpha (A)", 2D) = "black" { }
}
SubShader { 
 LOD 200
 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
  ZWrite Off
  Cull Off
  Fog { Mode Off }
  Blend SrcAlpha OneMinusSrcAlpha
  ColorMask RGB
  Offset -1, -1
  GpuProgramID 8988
Program "vp" {
SubProgram "gles " {
"!!GLES
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp mat4 glstate_matrix_mvp;
uniform mediump vec4 _MainTex_ST;
varying mediump vec4 xlv_COLOR;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
void main ()
{
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_COLOR = _glesColor;
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = ((_glesVertex.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
uniform sampler2D _ClipTex;
varying mediump vec4 xlv_COLOR;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
void main ()
{
  mediump vec4 col_1;
  mediump vec4 mask_2;
  mediump float alpha_3;
  highp vec2 P_4;
  P_4 = ((xlv_TEXCOORD1 * 0.5) + vec2(0.5, 0.5));
  lowp float tmpvar_5;
  tmpvar_5 = texture2D (_ClipTex, P_4).w;
  alpha_3 = tmpvar_5;
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_MainTex, xlv_TEXCOORD0);
  mask_2 = tmpvar_6;
  mediump vec4 tmpvar_7;
  tmpvar_7 = clamp (ceil((xlv_COLOR - 0.5)), 0.0, 1.0);
  mediump vec4 tmpvar_8;
  tmpvar_8 = clamp (((
    (tmpvar_7 * 0.51)
   - xlv_COLOR) / -0.49), 0.0, 1.0);
  col_1.xyz = tmpvar_8.xyz;
  col_1.w = (tmpvar_8.w * alpha_3);
  mask_2 = (mask_2 * tmpvar_7);
  col_1.w = (col_1.w * ((mask_2.x + mask_2.y) + (mask_2.z + mask_2.w)));
  gl_FragData[0] = col_1;
}


#endif
"
}
SubProgram "gles3 " {
"!!GLES3
#ifdef VERTEX
#version 300 es
precision highp float;
precision highp int;
uniform 	vec4 _Time;
uniform 	vec4 _SinTime;
uniform 	vec4 _CosTime;
uniform 	vec4 unity_DeltaTime;
uniform 	vec3 _WorldSpaceCameraPos;
uniform 	vec4 _ProjectionParams;
uniform 	vec4 _ScreenParams;
uniform 	vec4 _ZBufferParams;
uniform 	vec4 unity_OrthoParams;
uniform 	vec4 unity_CameraWorldClipPlanes[6];
uniform 	mat4 unity_CameraProjection;
uniform 	mat4 unity_CameraInvProjection;
uniform 	vec4 _WorldSpaceLightPos0;
uniform 	vec4 _LightPositionRange;
uniform 	vec4 unity_4LightPosX0;
uniform 	vec4 unity_4LightPosY0;
uniform 	vec4 unity_4LightPosZ0;
uniform 	mediump vec4 unity_4LightAtten0;
uniform 	mediump vec4 unity_LightColor[8];
uniform 	vec4 unity_LightPosition[8];
uniform 	mediump vec4 unity_LightAtten[8];
uniform 	vec4 unity_SpotDirection[8];
uniform 	mediump vec4 unity_SHAr;
uniform 	mediump vec4 unity_SHAg;
uniform 	mediump vec4 unity_SHAb;
uniform 	mediump vec4 unity_SHBr;
uniform 	mediump vec4 unity_SHBg;
uniform 	mediump vec4 unity_SHBb;
uniform 	mediump vec4 unity_SHC;
uniform 	mediump vec3 unity_LightColor0;
uniform 	mediump vec3 unity_LightColor1;
uniform 	mediump vec3 unity_LightColor2;
uniform 	mediump vec3 unity_LightColor3;
uniform 	vec4 unity_ShadowSplitSpheres[4];
uniform 	vec4 unity_ShadowSplitSqRadii;
uniform 	vec4 unity_LightShadowBias;
uniform 	vec4 _LightSplitsNear;
uniform 	vec4 _LightSplitsFar;
uniform 	mat4 unity_World2Shadow[4];
uniform 	mediump vec4 _LightShadowData;
uniform 	vec4 unity_ShadowFadeCenterAndType;
uniform 	mat4 glstate_matrix_mvp;
uniform 	mat4 glstate_matrix_modelview0;
uniform 	mat4 glstate_matrix_invtrans_modelview0;
uniform 	mat4 _Object2World;
uniform 	mat4 _World2Object;
uniform 	vec4 unity_LODFade;
uniform 	mat4 glstate_matrix_transpose_modelview0;
uniform 	mat4 glstate_matrix_projection;
uniform 	lowp vec4 glstate_lightmodel_ambient;
uniform 	mat4 unity_MatrixV;
uniform 	mat4 unity_MatrixVP;
uniform 	lowp vec4 unity_AmbientSky;
uniform 	lowp vec4 unity_AmbientEquator;
uniform 	lowp vec4 unity_AmbientGround;
uniform 	lowp vec4 unity_FogColor;
uniform 	vec4 unity_FogParams;
uniform 	vec4 unity_LightmapST;
uniform 	vec4 unity_DynamicLightmapST;
uniform 	vec4 unity_SpecCube0_BoxMax;
uniform 	vec4 unity_SpecCube0_BoxMin;
uniform 	vec4 unity_SpecCube0_ProbePosition;
uniform 	mediump vec4 unity_SpecCube0_HDR;
uniform 	vec4 unity_SpecCube1_BoxMax;
uniform 	vec4 unity_SpecCube1_BoxMin;
uniform 	vec4 unity_SpecCube1_ProbePosition;
uniform 	mediump vec4 unity_SpecCube1_HDR;
uniform 	lowp vec4 unity_ColorSpaceGrey;
uniform 	lowp vec4 unity_ColorSpaceDouble;
uniform 	mediump vec4 unity_ColorSpaceDielectricSpec;
uniform 	mediump vec4 unity_ColorSpaceLuminance;
uniform 	mediump vec4 unity_Lightmap_HDR;
uniform 	mediump vec4 unity_DynamicLightmap_HDR;
uniform 	mediump vec4 _MainTex_ST;
in highp vec4 in_POSITION0;
in mediump vec4 in_COLOR0;
in highp vec2 in_TEXCOORD0;
out mediump vec4 vs_COLOR0;
out highp vec2 vs_TEXCOORD0;
out highp vec2 vs_TEXCOORD1;
highp vec4 t0;
void main()
{
    //Instruction 241
    //MUL
    t0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    //Instruction 242
    //MAD
    t0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + t0;
    //Instruction 243
    //MAD
    t0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + t0;
    //Instruction 244
    //MAD
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + t0;
    //Instruction 245
    //MOV
    vs_COLOR0 = in_COLOR0;
    //Instruction 246
    //MAD
    vs_TEXCOORD1.xy = in_POSITION0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    //Instruction 247
    //MOV
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
    //Instruction 248
    //RET
    return;
}

#endif
#ifdef FRAGMENT
#version 300 es
precision highp float;
precision highp int;
uniform lowp sampler2D _ClipTex;
uniform lowp sampler2D _MainTex;
in mediump vec4 vs_COLOR0;
in highp vec2 vs_TEXCOORD0;
in highp vec2 vs_TEXCOORD1;
layout(location = 0) out mediump vec4 SV_Target0;
mediump vec4 t16_0;
highp vec2 t1;
lowp vec4 t10_1;
mediump vec2 t16_2;
mediump float t16_9;
void main()
{
    //Instruction 226
    //ADD
    t16_0 = vs_COLOR0 + vec4(-0.5, -0.5, -0.5, -0.5);
    //Instruction 227
    //ROUND_PI
    t16_0 = ceil(t16_0);
    t16_0 = clamp(t16_0, 0.0, 1.0);
    //Instruction 228
    //SAMPLE
    t10_1 = texture(_MainTex, vs_TEXCOORD0.xy);
    //Instruction 229
    //MUL
    t16_2.xy = t16_0.xy * t10_1.xy;
    //Instruction 230
    //ADD
    t16_2.x = t16_2.y + t16_2.x;
    //Instruction 231
    //MAD
    t16_2.x = t10_1.z * t16_0.z + t16_2.x;
    //Instruction 232
    //MAD
    t16_2.x = t10_1.w * t16_0.w + t16_2.x;
    //Instruction 233
    //MAD
    t16_0 = t16_0 * vec4(0.50999999, 0.50999999, 0.50999999, 0.50999999) + (-vs_COLOR0);
    //Instruction 234
    //MUL
    t16_0 = t16_0 * vec4(-2.04081631, -2.04081631, -2.04081631, -2.04081631);
    t16_0 = clamp(t16_0, 0.0, 1.0);
    //Instruction 235
    //MAD
    t1.xy = vs_TEXCOORD1.xy * vec2(0.5, 0.5) + vec2(0.5, 0.5);
    //Instruction 236
    //SAMPLE
    t10_1.x = texture(_ClipTex, t1.xy).w;
    //Instruction 237
    //MUL
    t16_9 = t16_0.w * t10_1.x;
    //Instruction 238
    //MOV
    SV_Target0.xyz = t16_0.xyz;
    //Instruction 239
    //MUL
    SV_Target0.w = t16_2.x * t16_9;
    //Instruction 240
    //RET
    return;
}

#endif
"
}
}
Program "fp" {
SubProgram "gles " {
"!!GLES"
}
SubProgram "gles3 " {
"!!GLES3"
}
}
 }
}
Fallback Off
}