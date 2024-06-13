#version 450 core
layout (location = 0) in vec3 vPos;
layout (location = 1) in vec3 vNorm;
layout (location = 2) in vec2 vUV0;
layout (location = 3) in vec2 vUV1;
layout (location = 4) in vec3 vBlend;
layout (location = 5) in vec3 vColor;
layout (location = 6) in float vShadow;

uniform mat4 uModel;
uniform mat4 uView;
uniform mat4 uProjection;
uniform vec3 uColor;

uniform mat2x4 uMat0;
uniform mat2x4 uMat1;
uniform mat2x4 uMat2;

out vec3 fPos;
out vec3 fNorm;
out vec2 fUV0;
out vec2 fUV1;
out vec3 fBlend;

out vec2 fUV0Mat0;
out vec2 fUV0Mat1;
out vec2 fUV0Mat2;

out vec4 fLightColor;

void main()
{
    fPos = vPos;
    fNorm = vNorm;
    fUV0 = vUV0;
    fUV1 = vUV1;
    fBlend = vBlend;

    fLightColor = vec4(vColor, vShadow);

    gl_Position = uProjection * uView * uModel * vec4(vPos.x, vPos.y, vPos.z, 1.0);
}