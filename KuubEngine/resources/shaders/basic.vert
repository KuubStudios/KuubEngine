#version 130

in vec3 pos;
out vec4 posColor;

void main () {
    gl_Position = vec4(pos, 1);

    posColor = clamp(gl_Position, 0, 1);
}