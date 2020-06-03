__kernel void crush(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, float tones)
{

	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}

	float uvV = (float)image[idx] / maxValue;
	float newV = floor(tones * uvV) / tones;


	image[idx] = (uchar)(newV * maxValue);

}