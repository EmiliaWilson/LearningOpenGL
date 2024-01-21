#version 330 core

out vec4 FragColor;

in vec2 TexCoords;

uniform sampler2D screenTexture;

const float offset = 1.0 /00.0;  

void main()
{

    FragColor = texture(screenTexture, TexCoords);
}  