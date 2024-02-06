#version 330 core


out vec4 FragColor;

in vec3 TexCoords;

uniform samplerCube skybox;

void main()
{
    const float gamma = 2.2; 
    vec3 color =  texture(skybox, TexCoords).rgb;
    vec3 result = pow(color, vec3(gamma));
    FragColor = vec4(result, 1.0);
}
