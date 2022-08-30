#version 450 core
out vec4 FragColor;

uniform sampler2D texture0; 
uniform sampler2D texture1; 
uniform sampler2D texture2; 

uniform vec3 uColor;

in vec3 fPos;
in vec3 fNorm;
in vec2 fUV;
in vec3 fDebug;

void main()
{
    vec3 xTangent = dFdx( fPos );
    vec3 yTangent = dFdy( fPos );
    vec3 faceNormal = normalize( cross( xTangent, yTangent ) );

    FragColor = vec4(faceNormal, 1.0f);
} 