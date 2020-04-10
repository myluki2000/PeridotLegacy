//-----------------------------------------------------------------------------
// SpriteEffect.fx
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

#include "Macros.fxh"


DECLARE_TEXTURE(Texture, 0);
DECLARE_TEXTURE(GlowMap, 1);


BEGIN_CONSTANTS
MATRIX_CONSTANTS

    float4x4 MatrixTransform;

END_CONSTANTS


struct VSOutput
{
	float4 position		: SV_Position;
	float4 color		: COLOR0;
    float2 texCoord		: TEXCOORD0;
};

struct FSOutput
{
	float4 Color		: SV_Target0;
	float4 Glow			: SV_Target1;
};

VSOutput SpriteVertexShader(	float4 position	: SV_Position,
								float4 color	: COLOR0,
								float2 texCoord	: TEXCOORD0)
{
	VSOutput output;
    output.position = mul(position, MatrixTransform);
	output.color = color;
	output.texCoord = texCoord;
	return output;
}


FSOutput SpritePixelShader(VSOutput input)
{
	FSOutput fout;
    fout.Color = SAMPLE_TEXTURE(Texture, input.texCoord) * input.color;
	fout.Glow = SAMPLE_TEXTURE(GlowMap, input.texCoord);
	
	return fout;
}

TECHNIQUE( SpriteBatch, SpriteVertexShader, SpritePixelShader );