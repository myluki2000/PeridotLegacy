#include "Macros.fxh"

#define SAMPLE_COUNT 25

DECLARE_TEXTURE(Texture, 0);

BEGIN_CONSTANTS

	float2 SampleOffsets[SAMPLE_COUNT];
	float SampleWeights[SAMPLE_COUNT];

MATRIX_CONSTANTS

    float4x4 MatrixTransform;

END_CONSTANTS

struct VSOutput
{
	float4 Position		: SV_Position;
	float4 Color 		: COLOR0;
    float2 TexCoord		: TEXCOORD0;
};

VSOutput VShader(float4 position : POSITION0,
				 float4 color : COLOR0,
				 float2 texCoord : TEXCOORD0)
{
	VSOutput output;
    output.Position = mul(position, MatrixTransform);
	output.Color = color;
	output.TexCoord = texCoord;
	return output;
}


float4 FShader(VSOutput pin) : SV_Target0
{
    float4 sum = 0;
	for(int i = 0; i < SAMPLE_COUNT; i++) {
		sum += SAMPLE_TEXTURE(Texture, pin.TexCoord + SampleOffsets[i]) * SampleWeights[i];
	}
	
	return sum * pin.Color;
}

technique SpriteBatch
{ 
	pass
	{ 
		VertexShader = compile vs_4_0 VShader();
		PixelShader = compile ps_4_0 FShader();
	}
}