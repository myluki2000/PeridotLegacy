#define OCTAVES 4

matrix WorldViewProjection;
float Time;
float Threshold;

struct VertexShaderInput
{
	float4 Position : POSITION;
	float4 Color : COLOR;
	float2 TexCoord : TEXCOORD;
};

struct VertexShaderOutput
{
	float4 PositionPS : POSITION0;
	float4 Position : POSITION1;
	float4 Color : COLOR;
	float2 TexCoord : TEXCOORD;
};

float rand(float2 coord)
{
    return frac(sin(dot(coord, float2(12.9898, 78.233))) * 43758.5453);
}

float noise(float2 coord)
{
	float2 i = floor(coord);
	float2 f = frac(coord);
	
	float y1 = rand(i);
	float y2 = rand(i + float2(1.0, 0));
	float y3 = rand(i + float2(0, 1.0));
	float y4 = rand(i + float2(1.0, 1.0));
	
	float y = lerp(lerp(y1, y2, smoothstep(0, 1, f.x)), lerp(y3, y4, smoothstep(0, 1, f.x)), smoothstep(0, 1, f.y));
	
	return y;
}

float fbm(float2 coord)
{
	float y;
	
	float amplitude = 0.5;
	float frequency = 1.0;
	
	for(int i = 0; i < OCTAVES; i++)
	{
		y += amplitude * noise(frequency * coord);
		frequency *= 2.0;
		amplitude *= 0.5;
	}
	
	return y;
}

VertexShaderOutput MainVS(in VertexShaderInput input)
{
	VertexShaderOutput output;

	output.PositionPS = mul(input.Position, WorldViewProjection);
	output.Position = input.Position;
	output.Color = input.Color;
	output.TexCoord = input.TexCoord;

	return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float2 coord = input.Position / 85.0;
	
	float2 motion = fbm(coord + Time * 0.3);
	
	float v = fbm(coord + motion);
	
	return float4(input.Color.rgb, lerp(0.0, input.Color.a, clamp(v - Threshold, 0.0, 1.0)));
}

technique BasicColorDrawing
{
	pass
	{
		AlphaBlendEnable = TRUE;
		DestBlend = INVSRCALPHA;
		SrcBlend = SRCALPHA;
		VertexShader = compile vs_4_0 MainVS();
		PixelShader = compile ps_4_0 MainPS();
	}
};