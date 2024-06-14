#version 450 core

layout (location = 0) in vec2 vPos;
layout (location = 2) in vec2 vUV0;

uniform vec2 uOffset;
uniform vec2 uScale;

out vec2 fUV;

void main()
{
    fUV = vUV0;
    gl_Position = vec4(vPos * uScale + uOffset, 0, 1.0);
}