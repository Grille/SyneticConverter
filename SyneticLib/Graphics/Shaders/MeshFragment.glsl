#version 450 core
uniform sampler2D texture0; 
uniform vec3 uColorDiffuse;

in vec3 fPos;
in vec3 fNorm;
in vec2 fUV;

out vec4 FragColor;

void main()
{
    FragColor = texture(texture0, fUV);
} 