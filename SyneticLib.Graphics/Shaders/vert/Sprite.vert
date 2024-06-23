#version 450 core

layout (location = 0) in vec2 vPos;
layout (location = 2) in vec2 vUV0;

uniform vec4 uDst;
uniform vec4 uSrc;

out vec2 fUV;

void main()
{
    fUV = vUV0 * uSrc.zw + uSrc.xy;
    gl_Position = vec4(vPos * uDst.zw + uDst.xy, 0, 1.0);
}