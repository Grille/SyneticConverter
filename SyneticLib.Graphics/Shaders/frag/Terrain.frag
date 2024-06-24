#version 450 core
uniform sampler2D uTexture0; 
uniform sampler2D uTexture1; 
uniform sampler2D uTexture2; 

uniform vec3 uColorDiffuse;
uniform float uShaderType;

uniform int uMode0;

in vec3 fPos;
in vec3 fNorm;
in vec2 fUV0;
in vec2 fUV1;
in vec3 fBlend;

in vec2 fUVMat0;
in vec2 fUVMat1;
in vec2 fUVMat2;

in vec4 fLightColor;

layout (location = 0) out vec4 Diffuse;
layout (location = 1) out vec4 Normal;
layout (location = 2) out vec4 Light;

const int Terrain = 0;
const int Road = 1;
const int Colorkey = 13;

float blend(float value){
    return smoothstep(0.4375, 0.5625, value);
}


void main()
{
    int mode = (uMode0 >> 4) & 15;
    switch (mode){
        case Terrain:
            vec4 layer0 = texture(uTexture0, fUVMat0);
            vec4 layer1 = texture(uTexture1, fUVMat1);
            vec4 layer2 = texture(uTexture2, fUVMat2);

            float a = blend(fBlend.r) * layer2.a;
            float c = blend(fBlend.b) * layer0.a * layer1.a;

            vec3 color = (layer0.rgb*(1.-c)+layer1.rgb*c)*(1.-a)+layer2.rgb*a;
            Diffuse = vec4(color, 1);
        break;

        case Colorkey:
            Diffuse = texture(uTexture0, fUV0);
            if (Diffuse.a < 0.5){
                discard;
            }
        break;

        default:
            Diffuse = texture(uTexture0, fUV0);
        break;
    }

    Normal = vec4(fNorm,1);
    Light = fLightColor;
} 