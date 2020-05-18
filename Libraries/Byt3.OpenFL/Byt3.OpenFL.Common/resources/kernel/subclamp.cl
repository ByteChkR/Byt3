#include utils/indexconversion.cl

__kernel void subc(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, __global uchar* overlay)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}

	float cur = (float)(image[idx] / maxValue);
	float ov = (float)(overlay[idx] / maxValue);
	float val = clamp(cur - ov, 0.0f, 1.0f);
	image[idx] = val * maxValue;
}

__kernel void subvc(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, float value)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}

	float cur = (float)(image[idx]/maxValue);
	float val = clamp(cur - value, 0.0f, 1.0f);
	image[idx] = val * maxValue;
}