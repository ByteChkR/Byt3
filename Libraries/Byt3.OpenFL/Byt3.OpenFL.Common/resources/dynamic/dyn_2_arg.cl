#include helper.cl



__kernel void d#type0_vv(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, float valueA, float valueB)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}
	image[idx] = (uchar)(#type0(ToUV(image[idx], maxValue), valueA, valueB)*maxValue);
}


__kernel void d#type0_tv(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, __global uchar* valueA, float valueB)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}
	image[idx] = (uchar)(#type0(ToUV(image[idx], maxValue), ToUV(valueA[idx], maxValue), valueB)*maxValue);
}


__kernel void d#type0_vt(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, float valueA, __global uchar* valueB)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}
	image[idx] = (uchar)(#type0(ToUV(image[idx], maxValue), valueA, ToUV(valueB[idx], maxValue))*maxValue);
}


__kernel void d#type0_tt(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, __global uchar* valueA, __global uchar* valueB)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}
	image[idx] = (uchar)(#type0(ToUV(image[idx], maxValue), ToUV(valueA[idx], maxValue), ToUV(valueB[idx], maxValue))*maxValue);
}




