__kernel void _arrange(__global uchar* image, __global uchar* source, int channelCount, __global uchar* newOrder)
{
	int idx = get_global_id(0);	
	for(int i = 0; i < channelCount; i++)
	{
		int dstChannelIndex = idx * channelCount + i;
		int srcChannelIndex = idx * channelCount + newOrder[i];
		image[dstChannelIndex] = source[srcChannelIndex];
	}
}