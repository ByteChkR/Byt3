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
}