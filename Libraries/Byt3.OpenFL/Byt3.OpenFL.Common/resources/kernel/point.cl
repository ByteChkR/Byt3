#include utils/indexconversion.cle
#include shapes/shapes.cle

__kernel void point3d(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, float positionX, float positionY, float positionZ, float radius)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}

	image[idx] += Point3D(idx, channelCount, dimensions, positionX, positionY, positionZ, radius, maxValue);

}

__kernel void point2d(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, float positionX, float positionY, float radius)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}

	image[idx] += Point2D(idx, channelCount, dimensions, positionX, positionY, radius, maxValue);

}

__kernel void point3dc(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, float positionX, float positionY, float positionZ, float radius)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}

	uchar v = Point3DC(idx, channelCount, dimensions, positionX, positionY, positionZ, radius, maxValue);
	float ret = clamp((float)(v + image[idx]), 0.0f, maxValue);
	image[idx] += ret;
}

__kernel void point2dc(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, float positionX, float positionY, float radius)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}

	uchar v = Point2DC(idx, channelCount, dimensions, positionX, positionY, radius, maxValue);
	float ret = clamp((float)(v + image[idx]), 0.0f, maxValue);
	image[idx] += ret;

}