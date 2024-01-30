#version 330 core


struct Material {
    sampler2D diffuse;
    sampler2D specular;
    sampler2D emission;

    float shininess;
    float constant;
    float linear;
    float quadratic;
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

struct SpotLight {
    vec3 position;
    vec3 direction;
    float cutOff;
    float outerCutOff;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;

    float constant;
    float linear;
    float quadratic;
};

vec3 CalcDirLight(DirLight light, vec3 normal, vec3 viewDir);
vec3 CalcPointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir);
vec3 CalcSpotLight(SpotLight light, vec3 normal, vec3 fragPos, vec3 viewDir);

out vec4 FragColor;

in vec2 TexCoords;

in vec3 Normal;
in vec3 FragPos;

uniform bool flashlightOn;

uniform vec3 viewPos;

uniform Material material;

uniform  DirLight dirLight;

#define NR_POINT_LIGHTS 4
uniform  PointLight pointLights[NR_POINT_LIGHTS];

uniform SpotLight spotLight;

void main()
{
    vec3 norm = normalize(Normal);
    vec3 viewDir = normalize (viewPos-FragPos);
    //vec3 result = vec3(0.0, 0.0, 0.0);
    vec3 result = CalcDirLight(dirLight, norm, viewDir);
    for (int i = 0; i < NR_POINT_LIGHTS; i++) {
        result += CalcPointLight(pointLights[i], norm, FragPos, viewDir);
    }
    if (flashlightOn) {
        result += CalcSpotLight(spotLight, norm, FragPos, viewDir);
    }
    vec3 emission = texture(material.emission, TexCoords).rgb;
    float distance = length(viewPos-FragPos);
    float attenuation = 1.0 / (material.constant + material.linear * distance + material.quadratic * (distance * distance));
    result = emission*attenuation + result;
    result = pow(result, vec3(1.0/2.2)); //gamma correction
    FragColor = vec4(result, 1.0);
}

vec3 CalcDirLight(DirLight light, vec3 normal, vec3 viewDir) {
    vec3 lightDir = normalize(-light.direction);
    vec3 halfwayDir = normalize(lightDir + viewDir);

    vec3 ambient = light.ambient * vec3(texture(material.diffuse, TexCoords));

    float diff = max(dot(normal, lightDir), 0.0);
    vec3 diffuse = light.diffuse * diff * vec3(texture(material.diffuse, TexCoords));
        
    float spec = pow(max(dot(normal, halfwayDir), 0.0), material.shininess);
    vec3 specular = light.specular * spec * vec3(texture(material.specular, TexCoords));

    return (ambient + diffuse + specular);
}

vec3 CalcPointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir) {
    vec3 lightDir = normalize(light.position - fragPos);
    vec3 halfwayDir = normalize(lightDir + viewDir);

    vec3 ambient = light.ambient * vec3(texture(material.diffuse, TexCoords));
    float diff = max(dot(normal, lightDir), 0.0);
    vec3 diffuse = light.diffuse * diff * vec3(texture(material.diffuse, TexCoords));
        
    float spec = pow(max(dot(normal, halfwayDir), 0.0), material.shininess);
    vec3 specular = light.specular * spec * vec3(texture(material.specular, TexCoords));


    float distance = length(light.position - fragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));
    ambient *= attenuation;
    diffuse *= attenuation;
    specular *= attenuation;
        
    return (ambient + diffuse + specular);
}

vec3 CalcSpotLight(SpotLight light, vec3 normal, vec3 fragPos, vec3 viewDir) {
    
    vec3 lightDir = normalize(light.position - fragPos);
    vec3 halfwayDir = normalize(lightDir + viewDir);

    float theta = dot(lightDir, normalize(-light.direction));
    float epsilon = light.cutOff - light.outerCutOff;
    float intensity = clamp((theta - light.outerCutOff) / epsilon, 0.0, 1.0);

    if (theta > light.cutOff) {
        vec3 ambient = light.ambient * vec3(texture(material.diffuse, TexCoords));

        float diff = max(dot(normal, lightDir), 0.0);
        vec3 diffuse = light.diffuse * diff * vec3(texture(material.diffuse, TexCoords));
        
        
        float spec = pow(max(dot(normal, halfwayDir), 0.0), material.shininess);
        vec3 specular = light.specular * spec * vec3(texture(material.specular, TexCoords));

        float distance = length(light.position - fragPos);
        float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));

        diffuse *= attenuation;
        specular *= attenuation;
        
        return(ambient + diffuse + specular);

    } else if (theta > light.outerCutOff) {
        vec3 ambient = light.ambient * vec3(texture(material.diffuse, TexCoords));

        float diff = max(dot(normal, lightDir), 0.0);
        vec3 diffuse = light.diffuse * diff * vec3(texture(material.diffuse, TexCoords));
        
        float spec = pow(max(dot(normal, halfwayDir), 0.0), material.shininess);
        vec3 specular = light.specular * spec * vec3(texture(material.specular, TexCoords));

        float distance = length(light.position - fragPos);
        float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));

        diffuse *= attenuation;
        specular *= attenuation;

        diffuse *= intensity;
        specular *= intensity;

        return (ambient + diffuse + specular);

    } else {
        return (light.ambient * vec3(texture(material.diffuse, TexCoords)));
    }
    
}