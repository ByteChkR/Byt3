#include utils/indexconversion.cle
#include convert/gconvert_all.cle

float GetWorleyDistance(float3 point, float3 worleypoint, float pmax)
{
	float value=pmax;
	//Loop from -1 to 1 in 3 steps per axis(Enables having "seamless" textures)
	for(float z = -1.0; z < 1.1; z+=1.0)
	{
		for(float y = -1.0; y < 1.1; y+=1.0)
		{
			for(float x = -1.0; x < 1.1; x+=1.0)
			{
				//Create Delta Vector from point to worley point(including the offset for "seamless")
				float3 delta = worleypoint-point+(float3)(x, y, z);

				float dist = length(delta);
				if(dist < value){ //We want the value to change when we are closer to a point than we were before(only the closest point is deciding the value of the pixel)
					value=dist;
				}
			}
		}
	}
	return value;
}

__kernel void worley(__global uchar *image, int3 dimensions, int channelCount, float maxValue, __global uchar* channelEnableState, __global uchar* arrayPositions, int poscount,  float max_distance)
{
	int idx = get_global_id(0); //Index of the current pixel value beeing processed
	int pixelIndex = idx / 4; //The "index" of the actual pixel(all rgba values of all pixels divided by 4)
	int channel = (int)fmod((float)idx, (float)channelCount); //Channel that we are currently operating on
	if(channelEnableState[channel]==0)
	{
		return; //Return if we choose to ignore the channel
	}


	int3 idx3d = Get3DimensionalIndex(dimensions.x, dimensions.y, pixelIndex); //Get the 3D Index of the Pixel
	float3 idx3dNorm = int3TOfloat3(idx3d) / int3TOfloat3(dimensions); //The same thing but in UV Space


	float value = max_distance;
	for(int i = 0; i < poscount; i++)
	{
		float3 position = (float3)(arrayPositions[i] / maxValue, arrayPositions[i + 1] / maxValue, arrayPositions[i + 2] / maxValue); //Construct point from random bytes in arrayPositions
		value = GetWorleyDistance(idx3dNorm, position, value); //Compute the Worley distance for the created point passing the value of the last point as pmax and the current 3d index of the pixel as the point
	}

	float result = clamp(value / max_distance, 0.0f, 1.0f); //Ensure staying within UV Bounds
	

	image[idx] = (uchar)(clamp(result * maxValue, 0.0f, maxValue)); //Converting the UV Bounds to 0-255 range for bytes

}