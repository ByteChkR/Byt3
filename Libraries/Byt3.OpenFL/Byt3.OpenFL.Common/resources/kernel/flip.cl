#include utils/indexconversion.cl

__kernel void flip_x(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}


	int3 idx3d = Get3DimensionalIndex(dimensions.x, dimensions.y, idx / channelCount);
	if(idx3d.x > dimensions.x/2)return;
	int otherIdx = GetFlattenedIndex(dimensions.x - idx3d.x, idx3d.y, idx3d.z, dimensions.x, dimensions.y) * channelCount + channel;

	uchar temp = image[idx];
	image[idx] = image[otherIdx];
	image[otherIdx] = temp;
}


__kernel void flip_y(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}


	int3 idx3d = Get3DimensionalIndex(dimensions.x, dimensions.y, idx / channelCount);
	if(idx3d.y > dimensions.y/2)return;
	int otherIdx = GetFlattenedIndex(idx3d.x, dimensions.y - idx3d.y, idx3d.z, dimensions.x, dimensions.y) * channelCount + channel;

	uchar temp = image[idx];
	image[idx] = image[otherIdx];
	image[otherIdx] = temp;
}


__kernel void flip_z(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}


	int3 idx3d = Get3DimensionalIndex(dimensions.x, dimensions.y, idx / channelCount);
	if(idx3d.z > dimensions.z/2)return;
	int otherIdx = GetFlattenedIndex(idx3d.x, idx3d.y, dimensions.z - idx3d.z, dimensions.x, dimensions.y) * channelCount + channel;

	uchar temp = image[idx];
	image[idx] = image[otherIdx];
	image[otherIdx] = temp;
}