#include helper.cl


__kernel void d#type0(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}
	image[idx] = (uchar)(#type0(ToUV(image[idx], maxValue))*maxValue);
}
