#version 330 core

uniform sampler2D ourTexture;

in vec4 fragColor;
in vec2 TexCoord;

out vec4 color;
  
void main()
{
    //color = fragColor;
	color = texture(ourTexture, TexCoord) * fragColor;
}