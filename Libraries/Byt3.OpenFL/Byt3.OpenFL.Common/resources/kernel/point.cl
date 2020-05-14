#include utils.cl
#include shapes.cl

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

	image[idx] += Point2D(idx, channelCount, dimensions.x, dimensions.y, positionX, positionY, radius, maxValue);

}

__kernel void point3dc(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, float positionX, float positionY, float positionZ, float radius)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}

	image[idx] += Point3DC(idx, channelCount, dimensions, positionX, positionY, positionZ, radius, maxValue);

}

__kernel void point2dc(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, float positionX, float positionY, float radius)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}

	image[idx] += Point2DC(idx, channelCount, dimensions.x, dimensions.y, positionX, positionY, radius, maxValue);

}