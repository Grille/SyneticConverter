#version 450 core
layout (location = 0) in vec2 vPos;
layout (location = 1) in vec2 vUV;

out vec2 fUV;

void main()
{
    gl_Position = vec4(vPos, 0, 1.0);
}