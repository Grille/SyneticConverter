#version 450 core

layout (location = 0) in vec2 vPos;
layout (location = 2) in vec2 vUV0;

out vec2 fUV;

void main()
{
    fUV = vec2(vUV0.x, 1-vUV0.y);
    gl_Position = vec4(vPos, 0, 1.0);
}