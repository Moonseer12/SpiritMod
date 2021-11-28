sampler uImage0 : register(s0);
sampler uImage1 : register(s1);

texture PortalNoise;
sampler pNoise = sampler_state
{
    Texture = PortalNoise;
};

texture DistortionNoise;
sampler dNoise = sampler_state
{
    Texture = DistortionNoise;
};

float Timer;
float Rotation;
float DistortionStrength = 0.1f;

float2 rotate(float2 coords, float delta)
{
    float2 ret;
    ret.x = (coords.x * cos(delta)) - (coords.y * sin(delta));
    ret.y = (coords.x * sin(delta)) + (coords.y * cos(delta));
    return ret;
}

float4 MainPS(float2 coords : TEXCOORD0, float4 ocolor : COLOR0) : COLOR0
{
    float2 displacement = (tex2D(dNoise, rotate(coords, Timer)).rg - float2(0.5f, 0.5f)) * DistortionStrength;
    float4 color = tex2D(uImage0, coords + displacement);
    if (color.a == 0)
        return float4(0, 0, 0, 0);
    
    color *= ocolor;
    float strength = (sqrt(pow(coords.x - 0.5f, 2) + pow(coords.y - 0.5f, 2)) * 2);
    strength = max(strength - 0.05f, 0);
    
    float4 noiseColor = tex2D(pNoise, rotate(coords - float2(0.5f, 0.5f), Rotation) + float2(0.5f, 0.5f));
    noiseColor = color * noiseColor.r * 2;
    
    
    float4 finalcolor = lerp(color, noiseColor, strength);
    finalcolor.rgb *= 1 + (1 - finalcolor.a) / 4;
    return finalcolor;
}

technique BasicColorDrawing
{
    pass WhiteSprite
    {
        PixelShader = compile ps_2_0 MainPS();
    }
};