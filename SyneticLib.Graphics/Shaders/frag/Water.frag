#version 450 core
uniform sampler2D uTexture0; 
uniform vec3 uColorDiffuse;
uniform float uShaderType;

in vec3 fPos;
in vec3 fNorm;
in vec2 fUV0;

out vec4 FragColor;

void main()
{
    FragColor = texture(uTexture0, fUV0) * vec4(uColorDiffuse,1);
} 