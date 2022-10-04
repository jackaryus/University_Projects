float4x4 World;
float4x4 View;
float4x4 Projection;

uniform extern texture Texture;

float innerCone;
float outerCone;

float PointLightDistanceSquared;
float SpotLightDistanceSquared1;
float SpotLightDistanceSquared2;

float3 PointLightPos;
float3 SpotLightPos1;
float3 SpotLightPos2;

float3 PointLightDiffuseColour;
float3 SpotLightDiffuseColour1;
float3 SpotLightDiffuseColour2;

float3 PointLightDirection;
float3 SpotLightDirection1;
float3 SpotLightDirection2;

sampler TextureSampler = sampler_state
{
	Texture = <Texture>;
};

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float4 Texcoord : TEXCOORD0;
	float3 Normal : NORMAL0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float2 Texcoord : TEXCOORD0;
	float3 Normal : TEXCOORD1;
	float3 WorldPos : TEXCOORD2;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;
	
	// Calculate viewProjection matrix (this can done outside of GPU)
	float4x4 viewProjection = mul(View, Projection);
	
	float4 posWorld = mul(input.Position, World);
	
	// Calculate the position of the vector against the world, view and projection matrices
	output.Position = mul(posWorld, viewProjection);
	
	// Store the texture coordinates for the pixel shader
	output.Texcoord = input.Texcoord;
	
	// Calculate the normal vector against the world
	output.Normal = mul(input.Normal, (float3x3)World);
	
	// Store world space position
	output.WorldPos = posWorld;
	
	return output;
}

float4 PixelShaderFunction (VertexShaderOutput input) : COLOR0
{
	// Sample the texture pixel using the texture coordinates
	float4 tex = tex2D(TextureSampler, input.Texcoord);
	
	// Calculate cosine of the inner and outer cone angles of the spotlight
	float2 cosAngles = cos(float2(outerCone, innerCone) * 0.5f);
	
	// Normal
	float3 n = normalize(input.Normal);
	
	// Point Light for the player
	float3 lightDir = normalize((input.WorldPos) - PointLightPos);
	float diffuseLighting = saturate(dot(input.Normal, -PointLightDirection));
	diffuseLighting += (PointLightDistanceSquared / dot(PointLightPos - input.WorldPos, PointLightPos - input.WorldPos));
	float4 Lantern = float4(saturate(tex.xyz * PointLightDiffuseColour * diffuseLighting * 0.8), 1.0) + (tex * 0.05);
	
	//SpotLight 1
	float3 lightDir2 = (SpotLightPos1 - input.WorldPos) / SpotLightDistanceSquared1;
	float attenuation = saturate(1.0f - dot(lightDir2, lightDir2));//reduction in strength (fall off)
	float3 normalizedLightDir = normalize(lightDir2);
	float spotDot = dot(-normalizedLightDir, normalize(SpotLightDirection1));
	float spotEffect = smoothstep(cosAngles[0], cosAngles[1], spotDot);
	attenuation *= spotEffect;
	float normalizedDotLightDir = saturate(dot(n, normalizedLightDir));
	float4 SpotLight1 = (float4(SpotLightDiffuseColour1, 1.0) * normalizedDotLightDir * attenuation);
	
	//SpotLight 2 
	float3 lightDir3 = (SpotLightPos2 - input.WorldPos) / SpotLightDistanceSquared2;
	float attenuation2 = saturate(1.0f - dot(lightDir3, lightDir3));
	float3 normalizedLightDir2 = normalize(lightDir3);
	float spotDot2 = dot(-normalizedLightDir2, normalize(SpotLightDirection2));
	float spotEffect2 = smoothstep(cosAngles[0], cosAngles[1], spotDot2);
	attenuation2 *= spotEffect2;
	float normalizedDotLightDir2 = saturate(dot(n, normalizedLightDir2));
	float4 SpotLight2 = (float4(SpotLightDiffuseColour2, 1.0) * normalizedDotLightDir2 * attenuation2);
	
	// Return the each different light and texture
	return Lantern + SpotLight1 + SpotLight2 * tex;
}

technique Technique1
{
	pass Pass1
	{
		VertexShader = compile vs_3_0 VertexShaderFunction();
		PixelShader = compile ps_3_0 PixelShaderFunction();
	}
}