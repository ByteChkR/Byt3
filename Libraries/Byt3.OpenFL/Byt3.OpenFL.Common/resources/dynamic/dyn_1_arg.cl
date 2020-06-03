#include helper.cl


__kernel void d#type0_v(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, float valueA)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}
	image[idx] = (uchar)(#type0(ToUV(image[idx], maxValue), valueA) * maxValue);
}


__kernel void d#type0_t(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, __global uchar* valueA)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}
	image[idx] = (uchar)(#type0(ToUV(image[idx], maxValue), ToUV(valueA[idx], maxValue)) * maxValue);
}
