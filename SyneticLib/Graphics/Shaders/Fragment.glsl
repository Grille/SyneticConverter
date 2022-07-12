#version 450 core
out vec4 FragColor;

uniform vec3 uColor;

in vec3 fDebug;

void main()
{
    FragColor = vec4(fDebug+0.5, 1.0f);
} 