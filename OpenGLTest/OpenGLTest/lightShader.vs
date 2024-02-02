#version 330 core
layout (location = 0) in vec3 aPos;

vec3 calcWave(vec2 position, vec2 randDir, float freq, float timeshift);
vec3 getWaves(vec2 position, int interations);

out vec3 FragPos;
out vec3 Normal;

uniform float time;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

#define NUM_WAVES 16
#define DRAG 0.25
void main()
{
    vec3 worldView = vec3(model * vec4(aPos, 1.0));
    vec3 values = getWaves(worldView.xz, NUM_WAVES);


    vec3 binormal = vec3(1, values.y, 0);
    vec3 tangent = vec3(0, values.z, 1);
    vec3 norm = cross(tangent, binormal);
    mat3 normalMatrix = mat3(transpose(inverse(model)));
    Normal = normalize(normalMatrix * norm);
    FragPos = vec3(vec4(worldView.x, values.x, worldView.z, 1.0));
    

    gl_Position = projection * view * vec4(FragPos, 1.0);

}

vec3 calcWave(vec2 position, vec2 randDir, float freq, float timeshift) {
    float x = dot(randDir,position) * freq + timeshift;
    float yVal =  exp(sin(x)-1);
    float binormalY = freq * randDir.x * cos(x) * exp(sin(x) - 1);
    float tangentY = freq * randDir.y * cos(x) * exp(sin(x) - 1);
    return vec3(yVal, binormalY, tangentY);
}

vec3 getWaves(vec2 position, int interations) {

    float sumOfWaves = 0.0;
    float sumOfDx = 0.0;
    float sumOfDy = 0.0;
    float sumOfAmp = 0.0;
    float iter = 0.0;
    float amp = 0.1;
    float wavelength = 0.75;
    float freq = (2/wavelength);
    float phase = 1 * (2/wavelength);

    for (int i = 0; i < interations; i++) {
        vec2 randDir = vec2(sin(iter), cos(iter));
        vec3 res = calcWave(position, randDir, freq, time*phase);
        
        position += randDir * res.y * amp * DRAG;
        sumOfWaves += res.x * amp;
        sumOfDx += res.y * amp;
        sumOfDy += res.z * amp;
        sumOfAmp += amp;


        amp = amp * 0.72;
        freq = freq * 1.26;
        iter += 1232.323;
        phase *= 1.05;
    }
    sumOfAmp = 1.0;
    return vec3(sumOfWaves/sumOfAmp, sumOfDx/sumOfAmp, sumOfDy/sumOfAmp);
}