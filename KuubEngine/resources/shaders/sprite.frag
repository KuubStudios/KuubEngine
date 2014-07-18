#version 130

uniform sampler2D tex;

in vec3 color;
in vec2 texcoord;

out vec4 out_color;

void main () {
    out_color = texture(tex, texcoord) * vec4(color, 1.0);
}