#version 450 core
uniform sampler2D texture0; 
uniform sampler2D texture1; 
uniform sampler2D texture2; 

uniform vec3 uColor;

in vec3 fPos;
in vec3 fNorm;
in vec2 fUV;
in vec3 fLight;

out vec4 FragColor;

void main()
{
    FragColor = texture(texture0, fUV);// * vec4(fLight,0);// vec4(fNorm, 1.0f);
} 