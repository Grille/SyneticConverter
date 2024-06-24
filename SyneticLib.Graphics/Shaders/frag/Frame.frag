#version 450 core
uniform sampler2D Diffuse; 
uniform sampler2D Normal; 
uniform sampler2D Light; 


in vec2 fUV;

out vec4 FragColor;

float blend(float value){
    return smoothstep(0.4375, 0.5625, value);
}

void main()
{
    vec4 diffuse = texture(Diffuse, fUV);
    vec4 normal = texture(Normal, fUV);
    vec4 light = texture(Light, fUV);

    vec3 sunLocation = vec3(0.1,0.6,0.3);
    vec3 sunColor = vec3(0.69,0.61,0.56);
    float shadowColor = blend(1-light.a);

    float diff = max(0, dot(normal.rgb, sunLocation)) * 1.8;

    vec3 clight = sunColor*diff*shadowColor+light.rgb;


    FragColor = diffuse * vec4(clight,1);
} 