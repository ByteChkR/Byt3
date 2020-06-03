#include interpolator/finterpolation.cl
#include utils/indexconversion.cl

__kernel void gradient_v(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, float top, float bottom)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}

	int3 idx3d = Get3DimensionalIndex(dimensions.x, dimensions.y, idx / channelCount);


	image[idx] = (uchar)(floatMix(top, bottom, (float)idx3d.x / (float)dimensions.x)*maxValue);
}


__kernel void gradient_h(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, float left, float right)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}

	int3 idx3d = Get3DimensionalIndex(dimensions.x, dimensions.y, idx / channelCount);


	image[idx] = (uchar)(floatMix(left, right, (float)idx3d.y / (float)dimensions.y)*maxValue);
}


__kernel void gradient_d(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, float front, float back)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}

	int3 idx3d = Get3DimensionalIndex(dimensions.x, dimensions.y, idx / channelCount);


	image[idx] = (uchar)(floatMix(front, back, (float)idx3d.z / (float)dimensions.z)*maxValue);
}