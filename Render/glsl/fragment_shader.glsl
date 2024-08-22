#version 400 core

in vec2 TexCoords;

out vec4 outputColor;

uniform sampler2D ourTexture;

void main()
{
    // outputColor = vec4(1,1,1,1);
    // outputColor = vec4(TexCoords*8, 1, 1);
    outputColor = texture(ourTexture, TexCoords);
} 
