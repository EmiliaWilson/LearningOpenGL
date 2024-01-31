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
    float yVal = 0.1 * sin(aPos.x * 8 + time * 1);
    FragPos = vec3(model * vec4(aPos.x, yVal, aPos.z, 1.0));
    

    gl_Position = projection * view * model * vec4(aPos.x, yVal, aPos.z, 1.0);

}