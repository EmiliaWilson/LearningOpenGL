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

struct FogParams {
    vec3 color;

    float density;
    float fogStart;
    float fogEnd;
};

vec3 calcReflection(vec3 normal, vec3 viewDir);
vec3 CalcDirLight(DirLight light, vec3 normal, vec3 viewDir);
vec3 CalcPointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir);
vec3 calcFog(FogParams fog, vec3 inputColor, vec3 viewPos, vec3 fragPos);

out vec4 FragColor;

in vec3 Normal;

in vec3 FragPos;

uniform samplerCube skybox;

uniform vec3 viewPos;

uniform Material material;

uniform  DirLight dirLight;

uniform FogParams fog;

#define NR_POINT_LIGHTS 1
uniform  PointLight pointLights[NR_POINT_LIGHTS];

void main()
{
    vec3 viewDir = normalize(viewPos-FragPos);
    //vec3 result = vec3(0.0, 0.0, 0.0);
    vec3 result = CalcDirLight(dirLight, Normal, viewDir);
    //for (int i = 0; i < NR_POINT_LIGHTS; i++) {
    //    result += CalcPointLight(pointLights[i], Normal, FragPos, viewDir);
    //}
    result += calcReflection(Normal, viewDir);
    //result = calcFog(fog, result, viewPos, FragPos);
    FragColor = vec4(result, 1.0);
}

vec3 calcReflection(vec3 normal, vec3 viewDir) {
    float reflectAmount = 1.0;
    float ratio = 1.00 / 1.33;
    vec3 R = refract(viewDir, normal, ratio);
    return (reflectAmount * texture(skybox, R).rgb);
}

vec3 CalcDirLight(DirLight light, vec3 normal, vec3 viewDir) {
    vec3 lightDir = normalize(-light.direction);
    vec3 halfwayDir = normalize(lightDir + viewDir);

    vec3 ambient = light.ambient * material.diffuse;

    float diff = max(dot(normal, lightDir), 0.0);
    vec3 diffuse = light.diffuse * diff * material.diffuse;
        
    float spec = pow(max(dot(normal, halfwayDir), 0.0), material.shininess);
    vec3 specular = light.specular * spec * material.specular;

    return (ambient + diffuse + specular);
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

//works but ugly
vec3 calcFog(FogParams fog, vec3 inputColor, vec3 viewPos, vec3 fragPos) {
    float fragDistance = length(viewPos - fragPos);
    float near = 0.1;
    float far = 100;
    float ndc = gl_FragCoord.z * 2.0 - 1.0;
    float linearDepth = (2.0 * near * far) / (far * near - ndc * (far - near));
    float fogValue = 1.0 - clamp((exp(-fog.density * fragDistance)), 0.0, 1.0);
    return mix(inputColor, fog.color, fogValue);
}