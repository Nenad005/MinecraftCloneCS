# version 400 core 

layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec2 aTexInfo;

out vec2 TexCoords;
// float textureSize

uniform mat4 proj;

void main()
{
    TexCoords = vec2((mod(aTexInfo[0], 16) + mod(aTexInfo[1], 2))/16.0, (floor(aTexInfo[0]/16) + floor(aTexInfo[1]/2))/16.0);
    // TexCoords = vec2((mod(aTexInfo[0], 16) + mod(aTexInfo[1], 2))/16.0, (floor(aTexInfo[0]/16) + floor(aTexInfo[1]/2))/16.0);
    gl_Position = proj * vec4(aPosition, 1.0);
}