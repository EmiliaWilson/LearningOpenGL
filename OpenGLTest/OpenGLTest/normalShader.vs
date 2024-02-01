#version 330 core
layout (location = 0) in vec3 aPos;

out VS_OUT {
    vec3 normal;
} vs_out;

uniform mat4 view;
uniform mat4 model;

uniform float time;

#define NUM_WAVES 5

void main()
{

    vec3 worldView = vec3(model * vec4(aPos, 1.0));
    float iter = 0;
    float amp = .5;
    float wavelength = 1;
    float freq = (2/wavelength);
    float speed = 1;
    float phase = speed * (2/wavelength);

    float yVal = 0.0;
    float binormalY = 0.0;
    float tangentY = 0.0;
    for (int i = 0; i < NUM_WAVES; i++) {
        vec2 randDir = normalize(vec2(sin(iter), cos(iter)));
        float x = dot(randDir,worldView.xz) * freq + time * phase;
        yVal += (amp * sin(x));
        binormalY += (amp * wavelength * randDir.x * cos(x));
        tangentY += (amp * wavelength * randDir.y * cos(x));
        amp = amp * 0.5;
        freq = freq * 2;
        phase *= 1.18;
        iter += 1232.323;
    }


    vec3 binormal = vec3(1, binormalY, 0);
    vec3 tangent = vec3(0, tangentY, 1);
    vec3 norm = cross(tangent, binormal);
    mat3 normalMatrix = mat3(transpose(inverse(view)));
    vs_out.normal = normalize(vec3(vec4(normalMatrix * norm, 0.0)));
    vec3 fragPos = vec3(vec4(worldView.x, yVal, worldView.z, 1.0));
    

    gl_Position =  view * vec4(fragPos, 1.0);
}