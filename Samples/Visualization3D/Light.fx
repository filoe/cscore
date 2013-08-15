technique Main
{
	pass P0
	{
		VertexShader = compile vs_2_0 LightVertexShader();
		PixelShader = compile ps_2_0 LightPixelShader();
	}
}