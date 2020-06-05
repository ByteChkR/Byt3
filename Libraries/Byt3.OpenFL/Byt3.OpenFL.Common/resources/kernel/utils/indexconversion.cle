uchar Lerp(uchar a, uchar b, float weightB)
{   
    float w = clamp(weightB, 0.0f, 1.0f);
    //return (uchar)floatMix(a, b, weightB);
    return (uchar)(a * (1 - w) + b * w);
}


int GetFlattenedIndex(int x, int y, int z, int width, int height)
{
    return (z * width * height) + (y * width) + x;
}

int3 Get3DimensionalIndex(int width, int height, int index)
{
    int d1, d2, d3;
    d3 = index / (width * height);
    int i = index - (d3 * width * height);
    d2 = i / width;
    d1 = (int)fmod((float)i, (float)width);
    return (int3)( d1, d2, d3 );
}

int2 Get2DIndex(int index, int width)
{
    int x = (int)fmod((float)index,(float)width);
    int y = index / width;
    int2 ret = (int2)(x, y);
    return ret;
}