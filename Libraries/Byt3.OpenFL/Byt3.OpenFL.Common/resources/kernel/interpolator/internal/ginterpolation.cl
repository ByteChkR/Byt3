#type0 #type1Mix(#type0 a, #type0 b, #type0 weightB)
{
    return a * (1 - weightB) + b * weightB;
}

#type0 #type1Slerp(#type0 t)
{
	return t * t;
}
//#include gint2erpolation.cl

#type0 #type1Flip(#type0 t)
{
	return 1 - t;
}

#type0 #type1Scale(#type0 t, #type0 scale)
{
	return t * scale;
}

#type0 #type1SmoothStart(#type0 t, int smoothness)
{
	#type0 ret = t;
	for(int i = 0; i < smoothness; i++)
	{
		ret *= t;
	}

	return ret;
}

#type0 #type1SmoothStartf(#type0 t, float smoothness)
{
	int ipow = (int)smoothness;
	return #type1Mix(#type1SmoothStart(t, ipow), #type1SmoothStart(t, ipow + 1), smoothness - ipow);
}

#type0 #type1SmoothStop(#type0 t, int smoothness)
{
	return #type1Flip(#type1SmoothStart(#type1Flip(t), smoothness));
}


#type0 #type1SmoothStopf(#type0 t, float smoothness)
{
	int ipow = (int)smoothness;
	return #type1Mix(#type1SmoothStop(t, ipow), #type1SmoothStop(t, ipow + 1), smoothness - ipow);
}


#type0 #type1SmoothStep(#type0 t, float smoothnessStart, float smoothnessStop)
{
	return #type1Mix(#type1SmoothStart(t, smoothnessStart), #type1SmoothStop(t, smoothnessStop), t);
}

#type0 #type1Arch2(#type0 t)
{
	return #type1Flip(t) * t;
}

#type0 #type1SmoothStopArch3(#type0 t)
{
	return #type1Scale(#type1Arch2(t), #type1Flip(t));
}

#type0 #type1SmoothStartArch3(#type0 t)
{
	return #type1Scale(#type1Arch2(t), t);
}

#type0 #type1SmoothStepArch4(#type0 t)
{
	return #type1Scale(#type1SmoothStartArch3(t), 1 - t);
}

#type0 #type1BellCurve(#type0 t, float smoothness)
{
	return #type1Scale(#type1SmoothStart(t, smoothness), #type1SmoothStop(t, smoothness));
}

#type0 #type1NormalizedBezier(#type0 b, #type0 c, #type0 t)
{
	#type0 s = #type1Flip(t);
	#type0 t2 = #type1Scale(t, t);
	#type0 s2 = #type1Scale(s, s);
	#type0 t3 = #type1Scale(t2, t);
	return 3.0f * b* s2 * t + 3.0f * c * s * t2 * t3;
}