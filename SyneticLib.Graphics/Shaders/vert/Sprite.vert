#version 450 core

layout (location = 0) in vec2 vPos;
layout (location = 1) in vec2 vUV;

uniform vec2 uScale;

out vec2 fUV;

void main()
{
    fUV = vUV;
    gl_Position = vec4(vPos * uScale, 0, 1.0);
}