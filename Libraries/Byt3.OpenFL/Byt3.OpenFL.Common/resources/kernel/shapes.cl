#include utils.cl
#include gconvert.cl int float

float UVSphere(float3 currentPos, float3 center, float radius, float v)
{
	float dist = length(currentPos - center);
	if(dist > radius)
	{
		return 0.0f;
	}
	return v;
}

float UVPoint3D(float3 currentPos, float3 center, float radius)
{
	float dist = length(currentPos - center);
	if(dist > radius)
	{
		return 0.0f;
	}
	return 1.0f - (dist / radius);
}

float UVPoint2D(float2 currentPos, float2 center, float radius)
{
	return UVPoint3D((float3)(currentPos, 0.5f), (float3)(center, 0.5f), radius);
}


float UVCircle(float2 currentPos, float2 center, float radius, float v)
{
	return UVSphere((float3)(currentPos, 0.5f), (float3)(center, 0.5f), radius, v);
}

float UVBox(float3 currentPos, float3 start, float3 bounds, float v)
{
	if(currentPos.x < start.x || currentPos.x > start.x + bounds.x ||
		currentPos.y < start.y || currentPos.y > start.y + bounds.y ||
		currentPos.z < start.z || currentPos.z > start.z + bounds.z)
	{
		return 0.0f;
	}
	return v;
}

float UVRectangle(float2 currentPos, float2 start, float2 bounds, float v)
{
	return UVBox((float3)(currentPos, 0.5f), (float3)(start, 0.0f), (float3)(bounds, 1.0f), v);
}

uchar Rectangle(int idx, int channelCount, int width, int height, float x, float y, float w, float h, float v, float maxValue)
{
	int2 idx2d = Get2DIndex(idx/channelCount, width);
	return (uchar)(UVRectangle(int2TOfloat2(idx2d) / int2TOfloat2((int2)(width, height)),(float2)(x, y), (float2)(w, h), v) * maxValue);
}

uchar Box(int idx, int channelCount, int3 dimensions, float x, float y, float z, float w, float h, float d,float v, float maxValue)
{
	int3 idx3d = Get3DimensionalIndex(dimensions.x, dimensions.y, idx/channelCount);
	return (uchar)(UVBox(int3TOfloat3(idx3d) / int3TOfloat3(dimensions), (float3)(x,y,z), (float3)(w,h,d), v) * maxValue);
}

uchar Point3D(int idx, int channelCount, int3 dimensions, float x, float y, float z, float radius, float maxValue)
{
	int3 idx3d = Get3DimensionalIndex(dimensions.x, dimensions.y, idx/channelCount);
	return (uchar)(UVPoint3D(int3TOfloat3(idx3d) / int3TOfloat3(dimensions),(float3)(x, y, z), radius) * maxValue);
}

uchar Point2D(int idx, int channelCount, int3 dimensions, float x, float y, float z, float radius, float maxValue)
{
	int2 idx2d = Get2DIndex(idx/channelCount, dimensions.x);
	return (uchar)(UVPoint2D(int2TOfloat2(idx2d) / int2TOfloat2((int2)(dimensions.x, dimensions.y)), (float2)(x, y), radius) * maxValue);
}

uchar Sphere(int idx, int channelCount, int3 dimensions, float x, float y, float z, float radius, float v, float maxValue)
{
	int3 idx3d = Get3DimensionalIndex(dimensions.x, dimensions.y, idx/channelCount);
	return (uchar)(UVSphere(int3TOfloat3(idx3d) / int3TOfloat3(dimensions),(float3)(x, y, z), radius, v) * maxValue);
}

uchar Circle(int idx, int channelCount, int width, int height, float x, float y, float radius, float v, float maxValue)
{
	int2 idx2d = Get2DIndex(idx/channelCount, width);
	return (uchar)(UVCircle(int2TOfloat2(idx2d) / int2TOfloat2((int2)(width, height)),(float2)(x, y), radius, v) * maxValue);
}


uchar RectangleC(int idx, int channelCount, int width, int height, float x, float y, float w, float h, float v, float maxValue)
{
	int2 idx2d = Get2DIndex(idx/channelCount, width);
	return (uchar)(clamp(UVRectangle(int2TOfloat2(idx2d) / int2TOfloat2((int2)(width, height)),(float2)(x, y), (float2)(w, h), v) * maxValue, 0.0f, maxValue));
}

uchar BoxC(int idx, int channelCount, int3 dimensions, float x, float y, float z, float w, float h, float d,float v, float maxValue)
{
	int3 idx3d = Get3DimensionalIndex(dimensions.x, dimensions.y, idx/channelCount);
	return (uchar)(clamp(UVBox(int3TOfloat3(idx3d) / int3TOfloat3(dimensions), (float3)(x,y,z), (float3)(w,h,d), v) * maxValue, 0.0f, maxValue));
}

uchar Point3DC(int idx, int channelCount, int3 dimensions, float x, float y, float z, float radius, float maxValue)
{
	int3 idx3d = Get3DimensionalIndex(dimensions.x, dimensions.y, idx/channelCount);
	return (uchar)(clamp(UVPoint3D(int3TOfloat3(idx3d) / int3TOfloat3(dimensions),(float3)(x, y, z), radius) * maxValue, 0.0f, maxValue));
}

uchar Point2DC(int idx, int channelCount, int3 dimensions, float x, float y, float z, float radius, float maxValue)
{
	int2 idx2d = Get2DIndex(idx/channelCount, dimensions.x);
	return (uchar)(clamp(UVPoint2D(int2TOfloat2(idx2d) / int2TOfloat2((int2)(dimensions.x, dimensions.y)), (float2)(x, y), radius) * maxValue, 0.0f, maxValue));
}

uchar SphereC(int idx, int channelCount, int3 dimensions, float x, float y, float z, float radius, float v, float maxValue)
{
	int3 idx3d = Get3DimensionalIndex(dimensions.x, dimensions.y, idx/channelCount);
	float3 idx3df = int3TOfloat3(idx3d);
	return (uchar)(clamp(UVSphere(idx3df / int3TOfloat3(dimensions),(float3)(x, y, z), radius, v) * maxValue, 0.0f, maxValue));
}

uchar CircleC(int idx, int channelCount, int width, int height, float x, float y, float radius, float v, float maxValue)
{
	int2 idx2d = Get2DIndex(idx/channelCount, width);
	return (uchar)(clamp(UVCircle(int2TOfloat2(idx2d) / int2TOfloat2((int2)(width, height)), (float2)(x, y), radius, v) * maxValue, 0.0f, maxValue));
}