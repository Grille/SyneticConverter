#version 450 core
uniform sampler2D uTexture0; 
uniform sampler2D uTexture1; 
uniform sampler2D uTexture2; 
uniform vec3 uColorDiffuse;
uniform float uShaderType;

in vec3 fPos;
in vec3 fNorm;
in vec2 fUV0;

in vec4 fLightColor;

out vec4 FragColor;

void main()
{
    vec3 sunLocation = vec3(0.1,0.6,0.3);
    vec3 sunColor = vec3(0.69,0.61,0.56);
    float shadowColor = smoothstep (0.4375, 0.5625, 1-fLightColor.a);

    float diff = max(0, dot(fNorm, sunLocation)) * 1.8;

    vec3 light = sunColor*diff*shadowColor+fLightColor.rgb;

    FragColor = texture(uTexture0, fUV0) * vec4(light,1);
} 