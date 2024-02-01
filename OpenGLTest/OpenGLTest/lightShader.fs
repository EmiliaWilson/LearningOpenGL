#version 330 core


struct Material {

    vec3 diffuse;
    vec3 specular;

    float shininess;
};

struct DirLight {
    vec3 direction;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

struct PointLight {
    vec3 position;

    float constant;
    float linear;
    float quadratic;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};


vec3 CalcDirLight(DirLight light, vec3 normal, vec3 viewDir);
vec3 CalcPointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir);

out vec4 FragColor;

in vec3 Normal;

in vec3 FragPos;

uniform vec3 viewPos;

uniform Material material;

uniform  DirLight dirLight;

#define NR_POINT_LIGHTS 1
uniform  PointLight pointLights[NR_POINT_LIGHTS];

void main()
{
    vec3 viewDir = normalize (viewPos-FragPos);
    //vec3 result = vec3(0.0, 0.0, 0.0);
    vec3 result = CalcDirLight(dirLight, Normal, viewDir);
    //for (int i = 0; i < NR_POINT_LIGHTS; i++) {
    //    result += CalcPointLight(pointLights[i], Normal, FragPos, viewDir);
    //}
    result = pow(result, vec3(1.0/2.2)); //gamma correction
    FragColor = vec4(result, 1.0);
}

vec3 CalcDirLight(DirLight light, vec3 normal, vec3 viewDir) {
    vec3 lightDir = normalize(-light.direction);
    vec3 halfwayDir = normalize(lightDir + viewDir);

    vec3 ambient = light.ambient * material.diffuse;

    float diff = max(dot(normal, lightDir), 0.0);
    vec3 diffuse = light.diffuse * diff * material.diffuse;
        
    float spec = pow(max(dot(normal, halfwayDir), 0.0), material.shininess);
    vec3 specular = light.specular * spec * material.specular;

    return (ambient + diffuse);
}

vec3 CalcPointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir) {
    vec3 lightDir = normalize(light.position - fragPos);
    vec3 halfwayDir = normalize(lightDir + viewDir);

    vec3 ambient = light.ambient * material.diffuse;
    float diff = max(dot(normal, lightDir), 0.0);
    vec3 diffuse = light.diffuse * diff * material.diffuse;
        
    float spec = pow(max(dot(normal, halfwayDir), 0.0), material.shininess);
    vec3 specular = light.specular * spec * material.specular;


    float distance = length(light.position - fragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));
    ambient *= attenuation;
    diffuse *= attenuation;
    specular *= attenuation;
        
    return (ambient + diffuse + specular);
}
