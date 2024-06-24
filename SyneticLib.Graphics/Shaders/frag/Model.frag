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

layout (location = 0) out vec4 Diffuse;
layout (location = 1) out vec4 Normal;
layout (location = 2) out vec4 Light;

void main()
{
    Diffuse = texture(uTexture0, fUV0);// * vec4(uColorDiffuse,1);
    Normal = vec4(fNorm,1);
    Light = vec4(1,1,1,1);
} 