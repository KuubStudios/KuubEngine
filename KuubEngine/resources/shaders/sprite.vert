﻿#version 130

in vec2 in_pos;
in vec3 in_color;
in vec2 in_texcoord;

out vec3 color;
out vec2 texcoord;

void main() {
    gl_Position = vec4(in_pos, 0.0, 1.0);
    color = in_color;
    texcoord = in_texcoord;
}