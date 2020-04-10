#include "Macros.fxh"

DECLARE_TEXTURE(Texture, 0);
DECLARE_TEXTURE(GlowMap, 1);

BEGIN_CONSTANTS

MATRIX_CONSTANTS

float4x4 WorldViewProj;

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

struct FSOutput
{
	float4 Color : SV_Target0;
	float4 Glow : SV_Target1;
};

VSOutput VShader(VSInput vin)
{
	VSOutput vout;
	
	vout.PositionPS = mul(vin.Position, WorldViewProj);
	vout.TexCoord = vin.TexCoord;
	
	return vout;
}

FSOutput FShader(VSOutput pin)
{
	FSOutput fout;
	
	fout.Color = SAMPLE_TEXTURE(Texture, pin.TexCoord.xy / pin.TexCoord.z);
	fout.Glow = SAMPLE_TEXTURE(GlowMap, pin.TexCoord.xy / pin.TexCoord.z);

	return fout;
}

TECHNIQUE(QuadEffect, VShader, FShader);