//DIFFUSE TEXTURE VERTEX SHADER
#version 330 // for glsl version (12 is for older versions , say opengl 2.1

uniform	mat4 	mvpMatrix;
uniform vec2	tiling;
uniform vec2	offset;

layout (location = 0)in vec3 vertex;
layout (location = 1)in vec3 normal;
layout (location = 2)in vec2 uv;

out vec2 texCoord; //make sure the texture coord is interpolated

void main( void ){
    gl_Position = mvpMatrix * vec4(vertex, 1.f);
    vec2 _uv = uv * tiling;
	texCoord = _uv + offset;
}
