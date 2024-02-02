#version 330 core
layout (location = 0) in vec3 aPos;

vec3 calcWave(vec2 position, vec2 randDir, float freq, float timeshift);
vec3 getWaves(vec2 position, int interations);

out VS_OUT {
    vec3 normal;
} vs_out;

uniform mat4 view;
uniform mat4 model;

uniform float time;

#define NUM_WAVES 1
#define DRAG 0.25

void main()
{

    vec3 worldView = vec3(model * vec4(aPos, 1.0));
    vec3 values = getWaves(worldView.xz, NUM_WAVES);


    vec3 binormal = vec3(1, values.y, 0);
    vec3 tangent = vec3(0, values.z, 1);
    vec3 norm = cross(tangent, binormal);
    mat3 normalMatrix = mat3(transpose(inverse(view * model)));
    vs_out.normal = normalize(normalMatrix * norm);
    vec3 fragPos = vec3(vec4(worldView.x, values.x, worldView.z, 1.0));
    

    gl_Position = view * vec4(fragPos, 1.0);
}

vec3 calcWave(vec2 position, vec2 randDir, float freq, float timeshift) {
    float x = dot(randDir,position) * freq + timeshift;
    float yVal =  exp(sin(x)-1);
    float binormalY = freq * randDir.x * cos(x) * exp(sin(x) - 1);
    float tangentY = freq * randDir.y * cos(x) * exp(sin(x) - 1);
    return vec3(yVal, binormalY, tangentY);
}

vec3 getWaves(vec2 position, int interations) {
    float finalDx;
    float finalDy;

    float sumOfWaves = 0.0;
    float sumOfAmp = 0.0;
    float iter = 0;
    float amp = .08;
    float wavelength = 0.75;
    float freq = (2/wavelength);
    float phase = 1 * (2/wavelength);

    for (int i = 0; i < interations; i++) {
        vec2 randDir = vec2(sin(iter), cos(iter));
        vec3 res = calcWave(position, randDir, freq, time*phase);
        
        position += randDir * res.y * amp * DRAG;
        sumOfWaves += res.x * amp;
        sumOfAmp += amp;

        if(i == (interations - 1)) {
            finalDx = res.y * amp;
            finalDy = res.z * amp;
        }

        amp = amp * 0.72;
        freq = freq * 1.26;
        iter += 1232.323;
    }
    return vec3((sumOfWaves / sumOfAmp), finalDx, finalDy);
}