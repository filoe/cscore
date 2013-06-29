/*

% Description of my shader.
% Second line of description for my shader.

keywords: material classic

date: YYMMDD

*/

struct VS_IN
{
	float4 pos : POSITION;
	float4 col : COLOR0;
	float2 UV : TEXCOORD0;
};

struct PS_IN
{
	float4 pos : POSITION;
	float4 col : COLOR0;
	float2 UV : TEXCOORD0;
};

float4x4 WorldViewProj : WorldViewProjection;

//texture
texture ColorTexture : DIFFUSE <
	string ResourceName = "default_color.dds";
	string UIName = "Diffuse Texture";
	string ResourceType = "2D";
>;

sampler2D ColorSampler = sampler_state
{
	Texture = <ColorTexture>;
	FILTER = MIN_MAG_MIP_LINEAR;
	AddressU = Wrap;
	AddressV = Wrap;
};

PS_IN VS( VS_IN input )
{
	PS_IN output = (PS_IN)0;
	output.pos = mul(input.pos, WorldViewProj);
	output.col = input.col;
	output.UV = input.UV.xy;
	
	return output;
}

float4 PS( PS_IN input ) : COLOR
{
	input.col.rgb = tex2D(ColorSampler, input.UV).rgb;
	return input.col;
}

technique Main {
	pass P0 {
		VertexShader = compile vs_2_0 VS();
        PixelShader  = compile ps_2_0 PS();
	}
}
