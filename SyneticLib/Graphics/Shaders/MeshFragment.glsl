#version 450 core
uniform vec3 uColorDiffuse;

in vec3 fDebug;
in vec3 fPos;

out vec4 FragColor;

void main()
{
    vec3 xTangent = dFdx( fPos );
    vec3 yTangent = dFdy( fPos );
    vec3 faceNormal = normalize( cross( xTangent, yTangent ) );

    FragColor = vec4(faceNormal, 1.0f);
} 