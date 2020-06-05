#include shapes/shapes_f.cle

__kernel void cut_box(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, __global uchar* input, float startx, float starty, float startz,float boundsx, float boundsy, float boundsz)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}	
	image[idx] = input[idx] * (1 - Boxf(idx, channelCount, dimensions, startx, starty, startz, boundsx, boundsy, boundsz, 1));

}

__kernel void cut_rect(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, __global uchar* input, float startx, float starty, float boundsx, float boundsy)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}	
	image[idx] = input[idx] * (1 - Rectanglef(idx, channelCount, dimensions.x, dimensions.y, startx, starty, boundsx, boundsy, 1));

}

__kernel void cut_sphere(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, __global uchar* input, float startx, float starty, float startz, float radius)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}	
	image[idx] = input[idx] * (1 - Spheref(idx, channelCount, dimensions, startx, starty, startz, radius, 1));
}

__kernel void cut_circle(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, __global uchar* input, float startx, float starty, float radius)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}	
	image[idx] = input[idx] * (1 - Circlef(idx, channelCount, dimensions.x, dimensions.y, startx, starty, radius, 1));

}


__kernel void cut_point2d(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, __global uchar* input, float startx, float starty, float radius)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}	
	image[idx] = input[idx] * (1 - Point2Df(idx, channelCount, dimensions, startx, starty, radius));

}

__kernel void cut_point3d(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, __global uchar* input, float startx, float starty, float startz, float radius)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}	
	image[idx] = input[idx] * (1 - Point3Df(idx, channelCount, dimensions, startx, starty, startz, radius));

}