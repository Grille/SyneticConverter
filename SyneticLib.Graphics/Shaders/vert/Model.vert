#version 450 core

layout (location = 0) in vec3 vPos;
layout (location = 1) in vec3 vNorm;
layout (location = 2) in vec2 vUV0;
layout (location = 3) in vec2 vUV1;
layout (location = 4) in vec3 vBlend;

uniform mat4 uModel;
uniform mat4 uView;
uniform mat4 uProjection;
uniform vec3 uColor;

out vec3 fPos;
out vec3 fNorm;
out vec2 fUV0;
out vec2 fUV1;
out vec3 fBlend;

void main()
{
    fPos = vPos;
    fNorm = vNorm;
    fUV0 = vUV0;
    fUV1 = vUV1;
    fBlend = vBlend;

    gl_Position = uProjection * uView * uModel * vec4(vPos, 1.0);
}