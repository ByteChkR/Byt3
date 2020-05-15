#type0 #type0Mix(#type0 a, #type0 b, #type0 weightB)
{
    return a * (1 - weightB) + b * weightB;
}

#type0 #type0Slerp(#type0 t)
{
	return t * t;
}
//#include gint2erpolation.cl

#type0 #type0Flip(#type0 t)
{
	return 1 - t;
}

#type0 #type0Scale(#type0 t, #type0 scale)
{
	return t * scale;
}

#type0 #type0SmoothStart(#type0 t, int smoothness)
{
	#type0 ret = t;
	for(int i = 0; i < smoothness; i++)
	{
		ret *= t;
	}

	return ret;
}

#type0 #type0SmoothStartf(#type0 t, float smoothness)
{
	int ipow = (int)smoothness;
	return #type0Mix(#type0SmoothStart(t, ipow), #type0SmoothStart(t, ipow + 1), smoothness - ipow);
}

#type0 #type0SmoothStop(#type0 t, int smoothness)
{
	return #type0Flip(#type0SmoothStart(#type0Flip(t), smoothness));
}


#type0 #type0SmoothStopf(#type0 t, float smoothness)
{
	int ipow = (int)smoothness;
	return #type0Mix(#type0SmoothStop(t, ipow), #type0SmoothStop(t, ipow + 1), smoothness - ipow);
}


#type0 #type0SmoothStep(#type0 t, float smoothnessStart, float smoothnessStop)
{
	return #type0Mix(#type0SmoothStart(t, smoothnessStart), #type0SmoothStop(t, smoothnessStop), t);
}

#type0 #type0Arch2(#type0 t)
{
	return #type0Flip(t) * t;
}

#type0 #type0SmoothStopArch3(#type0 t)
{
	return #type0Scale(#type0Arch2(t), #type0Flip(t));
}

#type0 #type0SmoothStartArch3(#type0 t)
{
	return #type0Scale(#type0Arch2(t), t);
}

#type0 #type0SmoothStepArch4(#type0 t)
{
	return #type0Scale(#type0SmoothStartArch3(t), 1 - t);
}

#type0 #type0BellCurve(#type0 t, float smoothness)
{
	return #type0Scale(#type0SmoothStart(t, smoothness), #type0SmoothStop(t, smoothness));
}

#type0 #type0NormalizedBezier(#type0 b, #type0 c, #type0 t)
{
	#type0 s = #type0Flip(t);
	#type0 t2 = #type0Scale(t, t);
	#type0 s2 = #type0Scale(s, s);
	#type0 t3 = #type0Scale(t2, t);
	return 3.0f * b* s2 * t + 3.0f * c * s * t2 * t3;
}