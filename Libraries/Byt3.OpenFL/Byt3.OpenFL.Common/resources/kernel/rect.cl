#include utils/indexconversion.cle
#include shapes/shapes.cle



__kernel void rect(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, float x, float y, float w, float h, float v)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}

	image[idx] = Rectangle(idx, channelCount, dimensions.x, dimensions.y, x, y, w, h, v, maxValue);
}

__kernel void rect1(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, float x, float y, float w, float h)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}

	image[idx] = Rectangle(idx, channelCount, dimensions.x, dimensions.y, x, y, w, h, 1, maxValue);
}

__kernel void rectc(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, float x, float y, float w, float h, float v)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}

	uchar retv = RectangleC(idx, channelCount, dimensions.x, dimensions.y, x, y, w, h, v, maxValue);
	float ret = clamp((float)(retv + image[idx]), 0.0f, maxValue);
	image[idx] += ret;
}

__kernel void rect1c(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, float x, float y, float w, float h)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}

	uchar retv = RectangleC(idx, channelCount, dimensions.x, dimensions.y, x, y, w, h, 1, maxValue);
	float ret = clamp((float)(retv + image[idx]), 0.0f, maxValue);
	image[idx] += ret;
}

__kernel void box(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, float x, float y, float z, float w, float h, float d, float v)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}

	image[idx] = Box(idx, channelCount, dimensions, x, y, z, w, h, d, v, maxValue);
}

__kernel void box1(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, float x, float y, float z, float w, float h, float d, float v)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}

	image[idx] = Box(idx, channelCount, dimensions, x, y, z, w, h, d, 1, maxValue);
}

__kernel void boxc(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, float x, float y, float z, float w, float h, float d, float v)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}

	uchar retv = BoxC(idx, channelCount, dimensions, x, y, z, w, h, d, v, maxValue);
	float ret = clamp((float)(retv + image[idx]), 0.0f, maxValue);
	image[idx] += ret;
}

__kernel void box1c(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, float x, float y, float z, float w, float h, float d, float v)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}

	uchar retv = BoxC(idx, channelCount, dimensions, x, y, z, w, h, d, 1, maxValue);
	float ret = clamp((float)(retv + image[idx]), 0.0f, maxValue);
	image[idx] += ret;
}