#include utils/indexconversion.cle
#include convert/gconvert_all.cle

__kernel void crop(__global uchar* image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, __global uchar* input, float startx, float starty, float startz,float boundsx, float boundsy, float boundsz)
{
	int idx = get_global_id(0);
	int channel = (int)fmod((float)idx, (float)channelCount);
	if(channelEnableState[channel]==0)
	{
		return;
	}


	float3 start = (float3)(startx, starty, startz);
	int3 starti = float3TOint3(start);

	float3 bounds = (float3)(boundsx, boundsy, boundsz);
	int3 boundsi = float3TOint3(bounds);

	int3 idx3d = Get3DimensionalIndex(dimensions.x, dimensions.y, idx / channelCount);
	float3 idx3df = int3TOfloat3(idx3d) / int3TOfloat3(dimensions);

	//Convert idx3df to uv coords in rectangle
	float3 srcScale = (float3)bounds;
	float3 srcIdx3df = start + (idx3df * srcScale);
	int3 srcIdx3d = float3TOint3(srcIdx3df * int3TOfloat3(dimensions));

	int src = GetFlattenedIndex(srcIdx3d.x, srcIdx3d.y, srcIdx3d.z, dimensions.x, dimensions.y);
	//Write value from input at uvcords in rect to image at idx3df
	image[idx] = input[(src * channelCount)+channel];
}
