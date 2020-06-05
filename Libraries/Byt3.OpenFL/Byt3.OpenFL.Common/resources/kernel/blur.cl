#include utils/indexconversion.cle

__kernel void blur_x(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, __global uchar* source, float strength)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}

	int3 idx3d = Get3DimensionalIndex(dimensions.x, dimensions.y, idx / channelCount);

	int avg = 0;
	int startidx = max(idx3d.x - (int)strength, 0);
	int endidx = min(idx3d.x + (int)strength, dimensions.x);
	for(int i = startidx; i < endidx; i++)
	{
		int curidx = GetFlattenedIndex(i, idx3d.y, idx3d.z, dimensions.x, dimensions.y) * channelCount + channel;
		avg += source[curidx];
	}
	//avg /= strength * 2;

	image[idx] = (uchar)(avg / (strength * 2));
}

__kernel void blur_y(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, __global uchar* source, float strength)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}

	int3 idx3d = Get3DimensionalIndex(dimensions.x, dimensions.y, idx / channelCount);

	int avg = 0;
	int startidx = max(idx3d.y - (int)strength, 0);
	int endidx = min(idx3d.y + (int)strength, dimensions.y);
	for(int i = startidx; i < endidx; i++)
	{
		int curidx = GetFlattenedIndex(idx3d.x, i, idx3d.z, dimensions.x, dimensions.y) * channelCount + channel;
		avg += source[curidx];
	}
	//avg /= strength * 2;

	image[idx] = (uchar)(avg / (strength * 2));
}

__kernel void blur_z(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, __global uchar* source, float strength)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}

	int3 idx3d = Get3DimensionalIndex(dimensions.x, dimensions.y, idx / channelCount);

	int avg = 0;
	int startidx = max(idx3d.z - (int)strength, 0);
	int endidx = min(idx3d.z + (int)strength, dimensions.z);
	for(int i = startidx; i < endidx; i++)
	{
		int curidx = GetFlattenedIndex(idx3d.x, idx3d.y, i, dimensions.x, dimensions.y) * channelCount + channel;
		avg += source[curidx];
	}
	//avg /= strength * 2;

	image[idx] = (uchar)(avg / (strength * 2));
}

__kernel void blur_xy(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, __global uchar* source, float strength)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}

	int3 idx3d = Get3DimensionalIndex(dimensions.x, dimensions.y, idx / channelCount);

	int avg = 0;
	int Ystartidx = max(idx3d.y - (int)strength, 0);
	int Yendidx = min(idx3d.y + (int)strength, dimensions.y);
	int Xstartidx = max(idx3d.x - (int)strength, 0);
	int Xendidx = min(idx3d.x + (int)strength, dimensions.x);
	for(int y = Ystartidx; y < Yendidx; y++)
	{
		for(int x = Xstartidx; x < Xendidx; x++)
		{
			int curidx = GetFlattenedIndex(x, y, idx3d.z, dimensions.x, dimensions.y) * channelCount + channel;
			avg += source[curidx];
		}
	}
	//avg /= strength * 2;

	image[idx] = (uchar)(avg / (strength * 2 * strength * 2));
}

__kernel void blur_yz(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, __global uchar* source, float strength)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}

	int3 idx3d = Get3DimensionalIndex(dimensions.x, dimensions.y, idx / channelCount);

	int avg = 0;
	int Zstartidx = max(idx3d.z - (int)strength, 0);
	int Zendidx = min(idx3d.z + (int)strength, dimensions.z);
	int Ystartidx = max(idx3d.y - (int)strength, 0);
	int Yendidx = min(idx3d.y + (int)strength, dimensions.y);
	for(int z = Zstartidx; z < Zendidx; z++)
	{
		for(int y = Ystartidx; y < Yendidx; y++)
		{
			int curidx = GetFlattenedIndex(idx3d.x, y, z, dimensions.x, dimensions.y) * channelCount + channel;
			avg += source[curidx];
		}
	}
	//avg /= strength * 2;

	image[idx] = (uchar)(avg / (strength * 2 * strength * 2));
}

__kernel void blur_xz(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, __global uchar* source, float strength)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}

	int3 idx3d = Get3DimensionalIndex(dimensions.x, dimensions.y, idx / channelCount);

	int avg = 0;
	int Zstartidx = max(idx3d.z - (int)strength, 0);
	int Zendidx = min(idx3d.z + (int)strength, dimensions.z);
	int Xstartidx = max(idx3d.x - (int)strength, 0);
	int Xendidx = min(idx3d.x + (int)strength, dimensions.x);
	for(int z = Zstartidx; z < Zendidx; z++)
	{
		for(int x = Xstartidx; x < Xendidx; x++)
		{
			int curidx = GetFlattenedIndex(x, idx3d.y, z, dimensions.x, dimensions.y) * channelCount + channel;
			avg += source[curidx];
		}
	}
	//avg /= strength * 2;

	image[idx] = (uchar)(avg / (strength * 2 * strength * 2));
}

__kernel void blur_xyz(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, __global uchar* source, float strength)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}

	int3 idx3d = Get3DimensionalIndex(dimensions.x, dimensions.y, idx / channelCount);

	int avg = 0;
	int Zstartidx = max(idx3d.z - (int)strength, 0);
	int Zendidx = min(idx3d.z + (int)strength, dimensions.z);
	int Xstartidx = max(idx3d.x - (int)strength, 0);
	int Xendidx = min(idx3d.x + (int)strength, dimensions.x);
	int Ystartidx = max(idx3d.y - (int)strength, 0);
	int Yendidx = min(idx3d.y + (int)strength, dimensions.y);
	for(int z = Zstartidx; z < Zendidx; z++)
	{
		for(int x = Xstartidx; x < Xendidx; x++)
		{
			for(int y = Ystartidx; y < Yendidx; y++)
			{
				int curidx = GetFlattenedIndex(x, y, z, dimensions.x, dimensions.y) * channelCount + channel;
				avg += source[curidx];
			}
		}
	}

	image[idx] = (uchar)(avg / (strength * 2 * strength * 2 * strength * 2));
}