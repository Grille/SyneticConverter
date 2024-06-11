#version 450 core
uniform sampler2D texture0 = 0;

in vec2 fUV;

out vec4 FragColor;

void main()
{
    FragColor = texture(texture0, fUV);
} 