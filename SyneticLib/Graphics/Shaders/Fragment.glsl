#version 450 core
out vec4 FragColor;

uniform vec3 uColor;

in vec3 fDebug;
in vec3 fPos;

void main()
{
    vec3 xTangent = dFdx( fPos );
    vec3 yTangent = dFdy( fPos );
    vec3 faceNormal = normalize( cross( xTangent, yTangent ) );

    FragColor = vec4(faceNormal, 1.0f);
} 