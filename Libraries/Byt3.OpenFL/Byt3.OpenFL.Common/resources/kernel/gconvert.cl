//#type0 = input type
//#type1 = return type

#if GCONVERT_ALL
	#undefine GCONVERT_ALL
	#define GCONVERT_ONE
	#include gconvert.cl char
	#include gconvert.cl uchar
	#include gconvert.cl short
	#include gconvert.cl ushort
	#include gconvert.cl int
	#include gconvert.cl uint
	#include gconvert.cl long
	#include gconvert.cl ulong
	#include gconvert.cl float
	#include gconvert.cl double
	#include gconvert.cl half

#elseif GCONVERT_ONE
	#undefine GCONVERT_ONE
	#include gconvert.cl #type0 char
	#include gconvert.cl #type0 uchar
	#include gconvert.cl #type0 short
	#include gconvert.cl #type0 ushort
	#include gconvert.cl #type0 int
	#include gconvert.cl #type0 uint
	#include gconvert.cl #type0 long
	#include gconvert.cl #type0 ulong
	#include gconvert.cl #type0 float
	#include gconvert.cl #type0 double
	#include gconvert.cl #type0 half
	
#else
//#include _gen_convert.cl uchar float <- Will import all conversions from (type0)1/2/3/4/8/15 to (type1)1/2/3/4/8/15

#type12 #type02TO#type12(#type02 value)
{
	return (#type12)((#type1)value.s0, (#type1)value.s1);
}

#type13 #type03TO#type13(#type03 value)
{
	return (#type13)((#type1)value.s0, (#type1)value.s1, (#type1)value.s2);
}

#type14 #type04TO#type14(#type04 value)
{
	return (#type14)((#type1)value.s0, (#type1)value.s1, (#type1)value.s2, (#type1)value.s3);
}

#type18 #type08TO#type18(#type08 value)
{
	return (#type18)((#type1)value.s0, (#type1)value.s1, (#type1)value.s2, (#type1)value.s3, (#type1)value.s4, (#type1)value.s5, (#type1)value.s6, (#type1)value.s7);
}

#type116 #type016TO#type116(#type016 value)
{
	return (#type116)((#type1)value.s0, (#type1)value.s1, (#type1)value.s2, (#type1)value.s3, (#type1)value.s4, (#type1)value.s5, (#type1)value.s6, (#type1)value.s7, 
		(#type1)value.s8, (#type1)value.s9, (#type1)value.sA, (#type1)value.sB, (#type1)value.sC, (#type1)value.sD, (#type1)value.sE, (#type1)value.sF);
}


#endif