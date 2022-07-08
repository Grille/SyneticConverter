#version 450 core
layout (location = 0) in vec3 vPos;
layout (location = 1) in vec3 vDebug;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform vec3 uColor;

out vec3 fPos;
out vec3 fDebug;

void main()
{
    fPos = vPos;
    fDebug = vDebug;
    gl_Position = projection * view * model * vec4(vPos, 1.0);
}