#version 330

uniform vec4 uEyePosition;

in vec4 oNormal;
in vec4 oSurfacePosition;

out vec4 FragColour;

struct LightProperties 
{ 
	vec4 Position; 
	vec3 AmbientLight; 
	vec3 DiffuseLight; 
	vec3 SpecularLight; 
};

uniform LightProperties uLight;

struct MaterialProperties 
{ 
	vec3 AmbientReflectivity; 
	vec3 DiffuseReflectivity; 
	vec3 SpecularReflectivity; 
	float Shininess; 
};

uniform MaterialProperties uMaterial;

void main()
{
	vec4 lightDir = normalize(uLight.Position - oSurfacePosition);
	vec4 eyeDirection = normalize(uEyePosition - oSurfacePosition); 
	vec4 reflectedVector = reflect(-lightDir, oNormal);

	float diffuseFactor = max(dot(oNormal, lightDir), 0);
	float specularFactor = pow(max(dot( reflectedVector, eyeDirection), 0.0), uMaterial.Shininess);
	vec3 ambient = uLight.AmbientLight * uMaterial.AmbientReflectivity;
	if (diffuseFactor > 0.0)
	{
		vec3 diffuse = uLight.DiffuseLight * uMaterial.DiffuseReflectivity * diffuseFactor;
		vec3 specular = uLight.SpecularLight * uMaterial.SpecularReflectivity * specularFactor;
		FragColour = vec4(ambient + diffuse + specular, 1);
	}
	else
	{
		FragColour = vec4(ambient, 1);
	}

}