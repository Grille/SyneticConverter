#version 450 core
uniform sampler2D uTexture0;

uniform vec4 uColor;

in vec2 fUV;

out vec4 FragColor;

void main()
{

    vec4 color = vec4(uColor.rgb, uColor.a * texture(uTexture0, fUV).r);
    if (color.a == 0)
        discard;
    FragColor = color;
} 