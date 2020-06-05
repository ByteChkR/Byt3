#include utils/indexconversion.cle
#include shapes/shapes.cle

__kernel void sphere(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, float positionX, float positionY, float positionZ, float radius, float value)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}

	image[idx] += Sphere(idx, channelCount, dimensions, positionX, positionY, positionZ, radius, value, maxValue);
}

__kernel void circle(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, float positionX, float positionY, float radius, float value)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}

	image[idx] += Circle(idx, channelCount, dimensions.x, dimensions.y, positionX, positionY, radius, value, maxValue);
}

__kernel void sphere1(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, float positionX, float positionY, float positionZ, float radius)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}

	image[idx] += Sphere(idx, channelCount, dimensions, positionX, positionY, positionZ, radius, 1, maxValue);
}

__kernel void circle1(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, float positionX, float positionY, float radius)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}

	image[idx] += Circle(idx, channelCount, dimensions.x, dimensions.y, positionX, positionY, radius, 1, maxValue);
}

__kernel void spherec(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, float positionX, float positionY, float positionZ, float radius, float value)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}

	uchar v = SphereC(idx, channelCount, dimensions, positionX, positionY, positionZ, radius, value, maxValue);
	float ret = clamp((float)(v + image[idx]), 0.0f, maxValue);
	image[idx] += ret;
}

__kernel void circlec(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, float positionX, float positionY, float radius, float value)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}

	uchar v = CircleC(idx, channelCount, dimensions.x, dimensions.y, positionX, positionY, radius, value, maxValue);
	float ret = clamp((float)(v + image[idx]), 0.0f, maxValue);
	image[idx] += ret;
}

__kernel void sphere1c(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, float positionX, float positionY, float positionZ, float radius)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}

	uchar v = SphereC(idx, channelCount, dimensions, positionX, positionY, positionZ, radius, 1, maxValue);
	float ret = clamp((float)(v + image[idx]), 0.0f, maxValue);
	image[idx] += ret;
}

__kernel void circle1c(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, float positionX, float positionY, float radius)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}

	uchar v = CircleC(idx, channelCount, dimensions.x, dimensions.y, positionX, positionY, radius, 1, maxValue);

	float ret = clamp((float)(v + image[idx]), 0.0f, maxValue);
	image[idx] += ret;
}