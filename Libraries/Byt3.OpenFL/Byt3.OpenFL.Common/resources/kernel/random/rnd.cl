#include random.cle

__kernel void rnd_gpu(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState)
{
	int idx = get_global_id(0);
	int useed = idx/channelCount;
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}
    int2 rndstate = (int2)(useed, channel);

    image[idx] = random(&rndstate);

}