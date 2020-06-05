#include utils/indexconversion.cle
#include convert/gconvert_all.cle

uchar GetSmoothNoise(__global uchar* image, int idx, int channel, int width, int height, int depth, int samplePeriod, float sampleFrequency)
{
	int index = idx / 4;
	int3 index3D = Get3DimensionalIndex(width, height, index);
	float3 index3Df = int3TOfloat3(index3D);


	int3 sample_0 = (index3D / (int3)(samplePeriod)) * (int3)samplePeriod;
	int3 sample_1 = float3TOint3(fmod(int3TOfloat3(sample_0) + (float3)(samplePeriod), (float3)(width, height, depth)));
	float3 blend = (index3Df - int3TOfloat3(sample_0)) * (float3)(sampleFrequency);


	int w0h0d0 = GetFlattenedIndex(sample_0.x, sample_0.y, sample_0.z, width, height) * 4 + channel;
	int w1h0d0 = GetFlattenedIndex(sample_1.x, sample_0.y, sample_0.z, width, height) * 4 + channel;
	int w0h1d0 = GetFlattenedIndex(sample_0.x, sample_1.y, sample_0.z, width, height) * 4 + channel;
	int w1h1d0 = GetFlattenedIndex(sample_1.x, sample_1.y, sample_0.z, width, height) * 4 + channel;
	int w0h0d1 = GetFlattenedIndex(sample_0.x, sample_0.y, sample_1.z, width, height) * 4 + channel;
	int w1h0d1 = GetFlattenedIndex(sample_1.x, sample_0.y, sample_1.z, width, height) * 4 + channel;
	int w0h1d1 = GetFlattenedIndex(sample_0.x, sample_1.y, sample_1.z, width, height) * 4 + channel;
	int w1h1d1 = GetFlattenedIndex(sample_1.x, sample_1.y, sample_1.z, width, height) * 4 + channel;

	float top0 = Lerp(image[w0h0d0], image[w1h0d0], blend.x);
	float top1 = Lerp(image[w0h0d1], image[w1h0d1], blend.x);
	float bottom0 = Lerp(image[w0h1d0], image[w1h1d0], blend.x);
	float bottom1 = Lerp(image[w0h1d1], image[w1h1d1], blend.x);
	float top = Lerp(top0, top1, blend.z);
	float bottom = Lerp(bottom0, bottom1, blend.z);

	return (uchar)(Lerp(top, bottom, blend.y));


}


__kernel void smooth(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, int octaves)
{


	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}

	int samplePeriod = 1 << octaves;
	float sampleFrequency = 1.0f / samplePeriod;

	image[idx] = GetSmoothNoise(image, idx, channel, dimensions.x, dimensions.y, dimensions.z, samplePeriod, sampleFrequency);
}