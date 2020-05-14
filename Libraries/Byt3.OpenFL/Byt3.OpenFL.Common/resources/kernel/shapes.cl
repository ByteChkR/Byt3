#include utils.cl

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
	return UVPoint3D((float3)(currentPos.x, currentPos.y, 0.5f), (float3)(center.x, center.y, 0.5f), radius);
}


float UVCircle(float2 currentPos, float2 center, float radius, float v)
{
	return UVSphere((float3)(currentPos.x, currentPos.y, 0.5f), (float3)(center.x, center.y, 0.5f), radius, v);
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
	return UVBox((float3)(currentPos.x, currentPos.y, 0.5f), (float3)(start.x, start.y, 0.0f), (float3)(bounds.x, bounds.y, 1.0f), v);
}

uchar Rectangle(int idx, int channelCount, int width, int height, float x, float y, float w, float h, float v, float maxValue)
{
	int2 idx2d = Get2DIndex(idx/channelCount, width);
	return (uchar)(UVRectangle((float2)((float)idx2d.x, (float)idx2d.y) / (float2)((float)width, (float)height),(float2)(x, y), (float2)(w, h), v) * maxValue);
}

uchar Box(int idx, int channelCount, int3 dimensions, float x, float y, float z, float w, float h, float d,float v, float maxValue)
{
	int3 idx3d = Get3DimensionalIndex(dimensions.x, dimensions.y, idx/channelCount);
	return (uchar)(UVBox((float3)((float)idx3d.x, (float)idx3d.y, (float)idx3d.z) / (float3)((float)dimensions.x, (float)dimensions.y, (float)dimensions.z), (float3)(x,y,z), (float3)(w,h,d), v) * maxValue);
}

uchar Point3D(int idx, int channelCount, int3 dimensions, float x, float y, float z, float radius, float maxValue)
{
	int3 idx3d = Get3DimensionalIndex(dimensions.x, dimensions.y, idx/channelCount);
	return (uchar)(UVPoint3D((float3)((float)idx3d.x, (float)idx3d.y, (float)idx3d.z) / (float3)((float)dimensions.x, (float)dimensions.y, (float)dimensions.z),(float3)(x, y, z), radius) * maxValue);
}

uchar Point2D(int idx, int channelCount, int3 dimensions, float x, float y, float z, float radius, float maxValue)
{
	int2 idx2d = Get2DIndex(idx/channelCount, dimensions.x);
	return (uchar)(UVPoint2D((float2)((float)idx2d.x, (float)idx2d.y) / (float2)((float)dimensions.x, (float)dimensions.y), (float2)(x, y), radius) * maxValue);
}

uchar Sphere(int idx, int channelCount, int3 dimensions, float x, float y, float z, float radius, float v, float maxValue)
{
	int3 idx3d = Get3DimensionalIndex(dimensions.x, dimensions.y, idx/channelCount);
	return (uchar)(UVSphere((float3)((float)idx3d.x, (float)idx3d.y, (float)idx3d.z) / (float3)((float)dimensions.x, (float)dimensions.y, (float)dimensions.z),(float3)(x, y, z), radius, v) * maxValue);
}

uchar Circle(int idx, int channelCount, int width, int height, float x, float y, float radius, float v, float maxValue)
{
	int2 idx2d = Get2DIndex(idx/channelCount, width);
	return (uchar)(UVCircle((float2)((float)idx2d.x, (float)idx2d.y) / (float2)((float)width, (float)height),(float2)(x, y), radius, v) * maxValue);
}


uchar RectangleC(int idx, int channelCount, int width, int height, float x, float y, float w, float h, float v, float maxValue)
{
	int2 idx2d = Get2DIndex(idx/channelCount, width);
	return (uchar)(clamp(UVRectangle((float2)((float)idx2d.x, (float)idx2d.y) / (float2)((float)width, (float)height),(float2)(x, y), (float2)(w, h), v) * maxValue, 0.0f, maxValue));
}

uchar BoxC(int idx, int channelCount, int3 dimensions, float x, float y, float z, float w, float h, float d,float v, float maxValue)
{
	int3 idx3d = Get3DimensionalIndex(dimensions.x, dimensions.y, idx/channelCount);
	return (uchar)(clamp(UVBox((float3)((float)idx3d.x, (float)idx3d.y, (float)idx3d.z) / (float3)((float)dimensions.x, (float)dimensions.y, (float)dimensions.z), (float3)(x,y,z), (float3)(w,h,d), v) * maxValue, 0.0f, maxValue));
}

uchar Point3DC(int idx, int channelCount, int3 dimensions, float x, float y, float z, float radius, float maxValue)
{
	int3 idx3d = Get3DimensionalIndex(dimensions.x, dimensions.y, idx/channelCount);
	return (uchar)(clamp(UVPoint3D((float3)((float)idx3d.x, (float)idx3d.y, (float)idx3d.z) / (float3)((float)dimensions.x, (float)dimensions.y, (float)dimensions.z),(float3)(x, y, z), radius) * maxValue, 0.0f, maxValue));
}

uchar Point2DC(int idx, int channelCount, int3 dimensions, float x, float y, float z, float radius, float maxValue)
{
	int2 idx2d = Get2DIndex(idx/channelCount, dimensions.x);
	return (uchar)(clamp(UVPoint2D((float2)((float)idx2d.x, (float)idx2d.y) / (float2)((float)dimensions.x, (float)dimensions.y), (float2)(x, y), radius) * maxValue, 0.0f, maxValue));
}

uchar SphereC(int idx, int channelCount, int3 dimensions, float x, float y, float z, float radius, float v, float maxValue)
{
	int3 idx3d = Get3DimensionalIndex(dimensions.x, dimensions.y, idx/channelCount);
	return (uchar)(clamp(UVSphere((float3)((float)idx3d.x, (float)idx3d.y, (float)idx3d.z) / (float3)((float)dimensions.x, (float)dimensions.y, (float)dimensions.z),(float3)(x, y, z), radius, v) * maxValue, 0.0f, maxValue));
}

uchar CircleC(int idx, int channelCount, int width, int height, float x, float y, float radius, float v, float maxValue)
{
	int2 idx2d = Get2DIndex(idx/channelCount, width);
	return (uchar)(clamp(UVCircle((float2)((float)idx2d.x, (float)idx2d.y) / (float2)((float)width, (float)height),(float2)(x, y), radius, v) * maxValue, 0.0f, maxValue));
}