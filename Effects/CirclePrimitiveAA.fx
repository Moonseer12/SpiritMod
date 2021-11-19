sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
matrix WorldViewProjection;
texture uTexture;
sampler textureSampler = sampler_state
{
    Texture = (uTexture);
};
float progress;
float4 uColor;
float4 uSecondaryColor;

struct VertexShaderInput
{
	float2 TextureCoordinates : TEXCOORD0;
    float4 Position : POSITION0;
    float4 Color : COLOR0;
};

struct VertexShaderOutput
{
	float2 TextureCoordinates : TEXCOORD0;
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput)0;
    float4 pos = mul(input.Position, WorldViewProjection);
    output.Position = pos;
    
    output.Color = input.Color;

	output.TextureCoordinates = input.TextureCoordinates;

    return output;
};

const float fadeHeight = 0.7f;
float4 MainPS(VertexShaderOutput input) : COLOR0
{    
    float4 color = input.Color;
    float height = (input.TextureCoordinates.x - 0.5f) * 2;
    if (height > fadeHeight)
        color *= pow((1 - ((height - fadeHeight) / (1 - fadeHeight))), 3);
    
    return color;
}

technique BasicColorDrawing
{
    pass Default
	{
        VertexShader = compile vs_2_0 MainVS();
        PixelShader = compile ps_2_0 MainPS();
    }
};