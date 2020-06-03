float Mod(float input, float valueA)
{
	return fmod(input, valueA);
}

float Add(float input, float valueA)
{
	return input + valueA;
}

float SelfAdd(float input)
{
	return input + input;
}

float ClampRescale(float input, float valueA, float valueB)
{
	
	float dist = valueB - valueA;
	

	if(valueA > input)
	{
		return 0.0f;
	}
	else if(valueB < input)
	{
		return 1.0f;
	}
	else
	{
		return input - valueA / dist;
	}

}

float Clamp(float input, float valueA, float valueB)
{
	return clamp(input, valueA, valueB);
}

float Div(float input, float valueA)
{
	return input / valueA;
}

float Mul(float input, float valueA)
{
	return input * valueA;
}

float SelfMul(float input)
{
	return input * input;
}

float Set(float input, float valueA)
{
	return valueA;
}

float Sub(float input, float valueA)
{
	return input - valueA;
}

float SelfSin(float input)
{
	return sin(input);
}

float Sin(float input, float valueA)
{
	return sin(valueA);
}

float Max(float input, float valueA)
{
	return max(input, valueA);
}

float Min(float input, float valueA)
{
	return min(input, valueA);
}


