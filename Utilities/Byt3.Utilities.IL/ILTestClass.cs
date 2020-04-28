using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Byt3.Utilities.IL
{
    internal class ILTestClass
    {
        public ILTestClass()
        {
            AssemblyName am = new AssemblyName();
            am.Name = "TestAssembly";
            AssemblyBuilder ab = AssemblyBuilder.DefineDynamicAssembly(am, AssemblyBuilderAccess.Run);
            ModuleBuilder mb = ab.DefineDynamicModule("TestModule");
            TypeBuilder tb = mb.DefineType("TestType", TypeAttributes.Public);

            MethodBuilder metb = tb.DefineMethod("TestMethod", MethodAttributes.Public |
                                                               MethodAttributes.Static);
            ILGenerator il = metb.GetILGenerator();

            il.EmitWriteLine("Hello from IL Assembly");
            MethodInfo answer = typeof(ILTestClass).GetMethod("Answer");
            il.EmitCall(OpCodes.Call, answer, null);
            il.Emit(OpCodes.Ret);

            TypeInfo ti = tb.CreateTypeInfo();

            MethodInfo mi = ti.GetDeclaredMethod("TestMethod");
            mi.Invoke(null, null);
        }


        

        public static void Answer()
        {
            Console.WriteLine("Hi from Assembly Generator");
        }
    }

    public static class ILTools
    {
        public delegate object TypeConstructor();

        public static TypeConstructor GetConstructor(Type type)
        {
            ConstructorInfo cinfo = type.GetConstructor(new Type[0]);
            DynamicMethod dm = new DynamicMethod(type.Name + "Ctor", type, new Type[0]);
            ILGenerator gen = dm.GetILGenerator();
            gen.Emit(OpCodes.Newobj, cinfo);
            gen.Emit(OpCodes.Ret);

            return (TypeConstructor)dm.CreateDelegate(typeof(TypeConstructor));
        }

        public static TypeConstructor GetConstructor(string type)
        {
            return GetConstructor(Type.GetType(type));
        }


    }
}