#version 330 core
layout (location = 0) in vec3 aPos;

out vec3 FragPos;
out vec3 Normal;

uniform float time;
uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

#define NUM_WAVES 8
void main()
{
    vec3 worldView = vec3(model * vec4(aPos, 1.0));
    float iter = 0;
    float amp = .066;
    float wavelength = 0.75;
    float freq = (2/wavelength);
    float speed = 1;
    float phase = speed * (2/wavelength);

    float yVal = 0.0;
    float binormalY = 0.0;
    float tangentY = 0.0;
    for (int i = 0; i < NUM_WAVES; i++) {
        vec2 randDir = normalize(vec2(sin(iter), cos(iter)));
        float x = dot(randDir,worldView.xz) * freq + time * phase;
        yVal += (amp * exp(sin(x)-1));
        binormalY += (amp * freq * randDir.x * cos(x) * exp(sin(x) - 1));
        tangentY += (amp * freq * randDir.y * cos(x) * exp(sin(x) - 1));
        amp = amp * 0.82;
        freq = freq * 1.18;
        phase *= 1.18;
        iter += 1232.323;
    }


    vec3 binormal = vec3(1, binormalY, 0);
    vec3 tangent = vec3(0, tangentY, 1);
    vec3 norm = cross(tangent, binormal);
    mat3 normalMatrix = mat3(transpose(inverse(view)));
    Normal = normalize(vec3(vec4(normalMatrix * norm, 0.0)));
    FragPos = vec3(vec4(worldView.x, yVal, worldView.z, 1.0));
    

    gl_Position = projection * view * vec4(FragPos, 1.0);

}