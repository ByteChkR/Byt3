#include utils.cl

__kernel void mul(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, __global uchar* overlay)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}

	float source = (float)image[idx];
	float overlayFrac = (float)overlay[idx] / 255.0f; //To 0-1 range
	image[idx] = source * overlayFrac;
}

__kernel void mulv(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, float value)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}

	image[idx] *= value;
}

__kernel void mulc(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, __global uchar* overlay)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}

	float source = (float)image[idx];
	float overlayFrac = (float)overlay[idx] / maxValue; //To 0-1 range
	float f = source * overlayFrac;

	float output = clamp(f, 0.0f, maxValue);

	image[idx] = output;
}

__kernel void mulvc(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, float value)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}
	float f = image[idx] * value;
	
	float output = clamp(f, 0.0f, maxValue);

	image[idx] = output;
}