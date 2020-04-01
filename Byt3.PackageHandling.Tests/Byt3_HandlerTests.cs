using System;
using System.Reflection.Metadata;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byt3.PackageHandling.Tests
{
    [TestClass]
    public class Byt3_HandlerTests
    {
        internal bool ACalled;
        internal bool BCalled;
        internal bool CCalled;
        internal bool DCalled;

        private class A { }

        private class B { }

        private class C_B : B { }

        private class D_C : C_B { }

        private abstract class HandlerHelper<T> : AHandler<T>
        {
            public Byt3_HandlerTests TestInstance;
        }

        private class HandlerA : HandlerHelper<A>
        {
            public override void Handle(A objectToHandle)
            {
                TestInstance.ACalled = true;
            }
        }
        private class HandlerB : HandlerHelper<B>
        {
            public override void Handle(B objectToHandle)
            {
                TestInstance.BCalled = true;
            }
        }
        private class HandlerC : HandlerHelper<C_B>
        {
            public override void Handle(C_B objectToHandle)
            {
                TestInstance.CCalled = true;
            }
        }
        private class HandlerD : HandlerHelper<D_C>
        {
            public override void Handle(D_C objectToHandle)
            {
                TestInstance.DCalled = true;
            }
        }

        private Byt3Handler GetHandler(HandlerLookupType type, AHandler fallback)
        {
            ACalled = BCalled = CCalled = DCalled = false;
            Byt3Handler handler = new Byt3Handler(type, fallback);
            return handler;
        }

        [TestMethod]
        public void HandlerExactOnly_Test()
        {

            Byt3Handler handler = GetHandler(HandlerLookupType.None, null);

            //handler.AddHandler(new HandlerA());
            handler.AddHandler(new HandlerB(){TestInstance = this});
            handler.AddHandler(new HandlerC() { TestInstance = this });
            //handler.AddHandler(new HandlerD());

            Assert.ThrowsException<Exception>(() => handler.Handle(new A()));  //Crash because of Exact only and A is missing
            handler.Handle(new B()); //Works
            handler.Handle(new C_B()); //Works
            Assert.ThrowsException<Exception>(() => handler.Handle(new D_C())); //Crash because no traversal up

            Assert.IsTrue(BCalled && CCalled);
            Assert.IsFalse(ACalled && DCalled);
        }

        [TestMethod]
        public void HandlerTraverseUp_Test()
        {
            Byt3Handler handler = GetHandler(HandlerLookupType.TraverseUp, null);

            handler.AddHandler(new HandlerA() { TestInstance = this });
            handler.AddHandler(new HandlerB() { TestInstance = this });
            //handler.AddHandler(new HandlerC());
            //handler.AddHandler(new HandlerD());


            handler.Handle(new A()); //Works
            handler.Handle(new B()); //Works
            handler.Handle(new C_B()); //Works because traverse up
            handler.Handle(new D_C()); //Works because traverse up
            Assert.IsTrue(ACalled && BCalled);
            Assert.IsFalse(CCalled && DCalled);
        }

        [TestMethod]
        public void HandlerUseFallback_Test()
        {
            Byt3Handler handler = GetHandler(HandlerLookupType.UseFallback, null);

            //handler.AddHandler(new HandlerA());
            //handler.AddHandler(new HandlerB());
            //handler.AddHandler(new HandlerC());
            //handler.AddHandler(new HandlerD());


            handler.Handle(new A()); //Works because of fallback
            handler.Handle(new B()); //Works because of fallback
            handler.Handle(new C_B()); //Works because of fallback
            handler.Handle(new D_C()); //Works because of fallback
            Assert.IsFalse(ACalled && BCalled && CCalled && DCalled);
        }
    }
}
