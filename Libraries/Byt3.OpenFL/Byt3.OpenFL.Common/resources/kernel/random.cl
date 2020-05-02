uchar random(uint2 *state)
{
    const float invMaxInt = 1.0f/4294967296.0f;
    uint x = (*state).x * 17 + (*state).y * 13123;
    (*state).x = (x<<13) ^ x;
    (*state).y ^= (x<<7);

    uint tmp = (x * (x * x * 15731 + 74323) + 871483);

    return tmp * invMaxInt * 255.0f;
}