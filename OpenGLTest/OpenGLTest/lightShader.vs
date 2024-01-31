#version 330 core
layout (location = 0) in vec3 aPos;

out vec3 Normal;
out vec3 FragPos;

uniform float time;
uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;


void main()
{
    float amp = 0.1;
    float wavelength = 8;
    float phase = 1;
    float yVal = amp * sin(aPos.x * wavelength + time * phase);
    vec3 binormal = vec3(1, 0, (amp * wavelength * cos(aPos.x * wavelength + time * phase)));
    vec3 tangent = vec3 (0, 1, 0);
    Normal = cross(binormal, tangent);
    FragPos = vec3(model * vec4(aPos.x, yVal, aPos.z, 1.0));
    

    gl_Position = projection * view * vec4(FragPos, 1.0);

}