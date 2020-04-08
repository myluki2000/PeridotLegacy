#include "Macros.fxh"

DECLARE_TEXTURE(Texture, 0);

BEGIN_CONSTANTS

MATRIX_CONSTANTS

float4x4 WorldViewProj          _vs(c15)          _cb(c0);

END_CONSTANTS

struct VSInput
{
    float4 Position : SV_Position;
    float3 TexCoord : TEXCOORD0;
};

struct VSOutput
{
	float4 PositionPS : POSITION0;
	float3 TexCoord : TEXCOORD0;
};

VSOutput VShader(VSInput vin)
{
	VSOutput vout;
	
	vout.PositionPS = mul(vin.Position, WorldViewProj);
	vout.TexCoord = vin.TexCoord;
	
	return vout;
}

float4 FShader(VSOutput pin) : SV_Target0
{
	return SAMPLE_TEXTURE(Texture, pin.TexCoord.xy / pin.TexCoord.z);
}

TECHNIQUE(QuadEffect, VShader, FShader);