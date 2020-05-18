
#include utils/indexconversion.cl

__kernel void ror(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}


	int3 idx3d = Get3DimensionalIndex(dimensions.x, dimensions.y, idx / channelCount);
	if(idx3d.x >= dimensions.x / 2 || idx3d.y >= dimensions.y / 2) return;

	int3 o31 = (int3)(dimensions.x - (idx3d.y + 1), idx3d.x, idx3d.z);
	int otherIdx1 = GetFlattenedIndex(o31.x, o31.y, o31.z, dimensions.x, dimensions.y) * channelCount + channel;

	int3 o32 = (int3)(dimensions.x - (o31.y + 1), o31.x, o31.z);
	int otherIdx2 = GetFlattenedIndex(o32.x, o32.y, o32.z, dimensions.x, dimensions.y) * channelCount + channel;

	int3 o33 = (int3)(dimensions.x - (o32.y + 1), o32.x, o32.z);
	int otherIdx3 = GetFlattenedIndex(o33.x, o33.y, o33.z, dimensions.x, dimensions.y) * channelCount + channel;




	uchar temp = image[idx]; //Q1

	image[idx] = image[otherIdx3];
	image[otherIdx3] = image[otherIdx2];
	image[otherIdx2] = image[otherIdx1];
	image[otherIdx1] = temp;

}

__kernel void rol(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}


	int3 idx3d = Get3DimensionalIndex(dimensions.x, dimensions.y, idx / channelCount);
	if(idx3d.x >= dimensions.x / 2 || idx3d.y >= dimensions.y / 2) return;

	int3 o31 = (int3)(dimensions.x - (idx3d.y + 1), idx3d.x, idx3d.z);
	int otherIdx1 = GetFlattenedIndex(o31.x, o31.y, o31.z, dimensions.x, dimensions.y) * channelCount + channel;

	int3 o32 = (int3)(dimensions.x - (o31.y + 1), o31.x, o31.z);
	int otherIdx2 = GetFlattenedIndex(o32.x, o32.y, o32.z, dimensions.x, dimensions.y) * channelCount + channel;

	int3 o33 = (int3)(dimensions.x - (o32.y + 1), o32.x, o32.z);
	int otherIdx3 = GetFlattenedIndex(o33.x, o33.y, o33.z, dimensions.x, dimensions.y) * channelCount + channel;




	uchar temp = image[idx];
	image[idx] = image[otherIdx1];
	image[otherIdx1] = image[otherIdx2];
	image[otherIdx2] = image[otherIdx3];
	image[otherIdx3] = temp;

}
