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

uniform mat2x4 uMat0;
uniform mat2x4 uMat1;
uniform mat2x4 uMat2;

out vec3 fPos;
out vec3 fNorm;
out vec2 fUV0;
out vec2 fUV1;
out vec3 fBlend;

out vec2 fUVMat0;
out vec2 fUVMat1;
out vec2 fUVMat2;

out vec4 fLightColor;

vec2 UVFromMatrix(in mat2x4 matrix, in vec3 position){
    float u = (position.x * matrix[0].x) + (position.y * matrix[0].y) + (position.z * matrix[0].z) + matrix[0].w;
    float f = (position.x * matrix[1].x) + (position.y * matrix[1].y) + (position.z * matrix[1].z) + matrix[1].w;
    return vec2(u, f);
}

void main()
{
    fPos = vPos;
    fNorm = vNorm;
    fUV0 = vUV0;
    fUV1 = vUV1;
    fBlend = vBlend;

    fUVMat0 = UVFromMatrix(uMat0, vPos);
    fUVMat1 = UVFromMatrix(uMat1, vPos);
    fUVMat2 = UVFromMatrix(uMat2, vPos);

    fLightColor = vec4(vColor, vShadow);

    gl_Position = uProjection * uView * uModel * vec4(vPos.x, vPos.y, vPos.z, 1.0);
}