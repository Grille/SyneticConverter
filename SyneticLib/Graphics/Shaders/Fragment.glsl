#version 450 core
out vec4 FragColor;

uniform vec3 uColor;

in vec3 fDebug;

void main()
{
    FragColor = vec4(fDebug * uColor, 1.0f);
} 