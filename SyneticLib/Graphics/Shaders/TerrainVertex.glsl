#version 450 core
layout (location = 0) in vec3 vPos;
layout (location = 1) in vec3 vDebug;

uniform mat4 uView;
uniform mat4 uProjection;
uniform vec3 uColor;

out vec3 fPos;
out vec3 fDebug;

void main()
{
    fPos = vPos;
    fDebug = vDebug;
    gl_Position = uProjection * uView * vec4(vPos, 1.0);
}