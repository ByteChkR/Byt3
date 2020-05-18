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