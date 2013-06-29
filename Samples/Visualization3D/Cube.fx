struct VS_IN
{
	float4 Position : POSITION;
	float4 Color : COLOR0;
};

struct PS_IN
{
	float4 Position : POSITION;
	float4 Color : COLOR0;
	float4  BR : COLOR1;
};

float4x4 worldViewProj : WorldViewProjection;
float value : Value;
float heightFactor : HeightFactor = 10.0;
float baseHeight : BaseHeight = 1.0;

float3 CalculateColor(float v, float4 colorh, float4 colorl)
{
	v = v * 3;
	//if(v > 1)
	//	v = 1;

	return lerp(colorl.rgb, colorh.rgb, value);
}

PS_IN VS( VS_IN input )
{
	PS_IN output = (PS_IN)0;

	bool istop = input.Position.y == baseHeight;

	if(istop)
	{
		input.Position.y = input.Position.y * (value * heightFactor) + 0.01f;
		output.BR.r = 0.0f;
	}
	else
	{
		output.BR.r = 0.5f;
	}
	output.Position = mul(input.Position, worldViewProj);
	output.Color = input.Color;
	//output.istop = istop;
	
	return output;
}

float4 PS( PS_IN input ) : COLOR
{
	float4 colorh = float4(0.0, 0.0, 1.0, 0.0); //blau
	float4 colorl = float4(0.0, 1.0, 0.0, 0.0); //grün
	input.Color.rgb = CalculateColor(value, colorh, colorl);
	input.Color.rgb = input.Color.rgb + input.BR.r;
	return input.Color;
}

technique Main {
	pass P0 {
		VertexShader = compile vs_2_0 VS();
        PixelShader  = compile ps_2_0 PS();
	}
}