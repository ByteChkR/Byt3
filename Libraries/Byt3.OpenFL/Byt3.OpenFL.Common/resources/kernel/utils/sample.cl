#include indexconversion.cl
#include gconvert.cl float int
#include gconvert.cl int float

uchar Samplei(__global uchar* image, int idx)
{
    return image[idx];
}

uchar Samplei3(__global uchar* image, int3 idx3d, int3 imageDims)
{
    return Samplei(image, GetFlattenedIndex(idx3d.x, idx3d.y, idx3d.z, imageDims.x, imageDims.y));
}

uchar Sample3(__global uchar* image, float3 uv, int3 imageDims)
{
    return Samplei3(image, float3TOint3(uv * int3TOfloat3(imageDims)), imageDims);
}

uchar Sample(__global uchar* image, float uv, int size)
{
    return Samplei(image, (int)(uv*size));
}