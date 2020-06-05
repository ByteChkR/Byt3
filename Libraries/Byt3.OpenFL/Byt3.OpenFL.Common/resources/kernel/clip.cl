#include shapes/shapes_f.cle

__kernel void clip_box(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, __global uchar* input, float startx, float starty, float startz,float boundsx, float boundsy, float boundsz)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}	
	image[idx] = input[idx] * Boxf(idx, channelCount, dimensions, startx, starty, startz, boundsx, boundsy, boundsz, 1);

}

__kernel void clip_rect(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, __global uchar* input, float startx, float starty, float boundsx, float boundsy)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}	
	image[idx] = input[idx] * Rectanglef(idx, channelCount, dimensions.x, dimensions.y, startx, starty, boundsx, boundsy, 1);

}

__kernel void clip_sphere(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, __global uchar* input, float startx, float starty, float startz, float radius)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}	
	image[idx] = input[idx] * Spheref(idx, channelCount, dimensions, startx, starty, startz, radius, 1);
}

__kernel void clip_circle(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, __global uchar* input, float startx, float starty, float radius)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}	
	image[idx] = input[idx] * Circlef(idx, channelCount, dimensions.x, dimensions.y, startx, starty, radius, 1);

}


__kernel void clip_point2d(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, __global uchar* input, float startx, float starty, float radius)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}	
	image[idx] = input[idx] * Point2Df(idx, channelCount, dimensions, startx, starty, radius);

}

__kernel void clip_point3d(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, __global uchar* input, float startx, float starty, float startz, float radius)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}	
	image[idx] = input[idx] * Point3Df(idx, channelCount, dimensions, startx, starty, startz, radius);

}