﻿#version 450 core
uniform sampler2D uTexture0;

in vec2 fUV;

out vec4 FragColor;

void main()
{
    FragColor = texture(uTexture0, fUV);
} 