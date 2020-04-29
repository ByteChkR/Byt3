using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Byt3.Utilities.IL
{
    public static class ILTools
    {

        public static T GetConstructor<T>(Type type)
            where T : Delegate
        {
            ConstructorInfo cinfo = type.GetConstructor(new Type[0]);
            DynamicMethod dm = new DynamicMethod(type.Name + "Ctor", type, new Type[0]);
            ILGenerator gen = dm.GetILGenerator();
            gen.Emit(OpCodes.Newobj, cinfo);
            gen.Emit(OpCodes.Ret);
            return (T)dm.CreateDelegate(typeof(T));
        }

        public static T GetConstructor<T>(string type)
            where T : Delegate
        {
            return GetConstructor<T>(Type.GetType(type));
        }

        public static T GetFieldValue<T>(Type type, FieldInfo info)
            where T : Delegate
        {
            DynamicMethod dm = new DynamicMethod(type.Name + "ILFieldValue." + info.Name, info.FieldType, new[] { typeof(object) });
            ILGenerator gen = dm.GetILGenerator();
            gen.Emit(OpCodes.Ldarg_0);
            gen.Emit(OpCodes.Ldfld, info);
            gen.Emit(OpCodes.Ret);
            return (T)dm.CreateDelegate(typeof(T));
        }


        public static T GetPropertySet<T>(Type type, PropertyInfo info)
            where T : Delegate
        {
            return GetMethodDel<T>(type, info.SetMethod);
        }
        public static T GetPropertyGet<T>(Type type, PropertyInfo info)
            where T : Delegate
        {
            return GetMethodDel<T>(type, info.GetMethod);
        }

        public static T GetMethodDel<T>(Type type, MethodInfo info)
        where T: Delegate
        {
            List<Type> args = new List<Type>() { typeof(object) };
            args.AddRange(info.GetParameters().Select(x => x.ParameterType));

            DynamicMethod dm = new DynamicMethod(type.Name + "ILMethodCall." + info.Name, info.ReturnType, args.ToArray());
            ILGenerator gen = dm.GetILGenerator();

            gen.Emit(OpCodes.Ldarg_0);
            for (int i = 1; i < args.Count; i++)
            {
                gen.Emit(OpCodes.Ldarg, i);
            }
            gen.Emit(OpCodes.Call, info);
            gen.Emit(OpCodes.Ret);
            return (T)dm.CreateDelegate(typeof(T));
        }
    }
}