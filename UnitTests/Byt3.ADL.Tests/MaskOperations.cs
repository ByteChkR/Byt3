﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Byt3.ADL.Tests
{
    [TestClass]
    public class MaskOperations
    {
        [TestMethod]
        public void ADL_Mask_IsContainedInMask_Test()
        {
            var bm1 = new BitMask<char>(1 | 4 | 16);
            var bm2 = new BitMask<char>(4);
            var bm3 = new BitMask<char>(2);


            Assert.IsTrue(BitMask.IsContainedInMask(bm1, bm2, false)); //True
            Assert.IsFalse(BitMask.IsContainedInMask(bm1, bm3, false)); //False
            Assert.IsFalse(BitMask.IsContainedInMask(bm1, bm2 | bm3, true)); //False
        }

        [TestMethod]
        public void ADL_Mask_GetUniqueMaskSet_Test()
        {
            var bm = new BitMask<char>(1 | 2 | 8 | 16 | 64);
            var ret = BitMask.GetUniqueMasksSet(bm);

            Assert.IsTrue(ret.Count == 5);
            Assert.IsTrue(ret.Contains(1));
            Assert.IsTrue(ret.Contains(2));
            Assert.IsTrue(ret.Contains(8));
            Assert.IsTrue(ret.Contains(16));
            Assert.IsTrue(ret.Contains(64));
        }

        [TestMethod]
        public void ADL_Mask_IsUniqueMask_Test()
        {
            var bm = new BitMask<char>(2 | 4);
            var bm1 = new BitMask<char>(8);

            Assert.IsTrue(BitMask.IsUniqueMask(bm1));
            Assert.IsFalse(BitMask.IsUniqueMask(bm));
        }

        [TestMethod]
        public void ADL_Mask_CombineMasks_Test()
        {
            var bm1 = new BitMask<TestEnum>(TestEnum.B | TestEnum.D | TestEnum.F);
            var bm2 = new BitMask<TestEnum>(TestEnum.B | TestEnum.E | TestEnum.G);
            BitMask ret = BitMask.CombineMasks(MaskCombineType.BitAnd, bm1, bm2);
            Assert.IsTrue(ret == 2);
            ret = BitMask.CombineMasks(MaskCombineType.BitOr, bm1, bm2);
            Assert.IsTrue(BitMask.IsContainedInMask(ret, 16, false));
            Assert.IsTrue(BitMask.IsContainedInMask(ret, 2, false));
        }

        [TestMethod]
        public void ADL_Mask_RemoveFlags_Test()
        {
            var bm1 = new BitMask<char>(2 | 8 | 16);
            BitMask<char> ret = BitMask.RemoveFlags(bm1, 2);
            Assert.IsFalse(BitMask.IsContainedInMask(ret, 2, true));
            Assert.IsTrue(BitMask.IsContainedInMask(bm1, 2, true));
        }

        [TestMethod]
        public void ADL_Mask_Constructors_Test()
        {
            var bm = new BitMask(2, 8, 16);
            var bm1 = new BitMask(2 | 8 | 16);


            Assert.AreEqual((int) bm, (int) bm1);
        }

        [TestMethod]
        public void ADL_Mask_FlagOperations_Test()
        {
            var bm = new BitMask(true);
            bm.SetAllFlags(2 | 4);
            Assert.AreEqual((int) bm, 6);

            bm.SetFlag(2 | 4, false);
            bm.SetFlag(16 | 8, true);
            Assert.AreEqual((int) bm, 16 | 8);


            Assert.IsTrue(bm.HasFlag(16, MaskMatchType.MatchOne));
            Assert.IsFalse(bm.HasFlag(24 | 4, MaskMatchType.MatchAll));
            bm.SetAllFlags(0);
            bm.Flip();
            Assert.IsTrue(-1 == bm);

            Assert.IsTrue(BitMask.CombineMasks(MaskCombineType.BitAnd) == 0);
            Assert.IsTrue(BitMask.IsUniqueMask(2));
            Assert.IsFalse(BitMask.IsUniqueMask(3));
            Assert.IsFalse(BitMask.IsUniqueMask(0));
            var gbm = new BitMask<TestEnum>(TestEnum.A, TestEnum.B);
            Assert.IsFalse(BitMask.IsUniqueMask(bm));
            Assert.IsTrue(gbm.HasFlag(TestEnum.A, MaskMatchType.MatchOne));

            gbm.SetFlag(TestEnum.C, true);

            //BitMask<TestEnum> gbm1 = TestEnum.C;

            Assert.IsFalse(gbm.HasFlag(TestEnum.C, MaskMatchType.MatchOne));

            gbm.SetFlag(TestEnum.C, false);

            Assert.IsFalse(gbm.HasFlag(TestEnum.C, MaskMatchType.MatchOne));

            gbm.SetAllFlags(TestEnum.C | TestEnum.A);
            Assert.IsTrue(gbm.HasFlag(TestEnum.C | TestEnum.A, MaskMatchType.MatchAll));
        }

        [Flags]
        private enum TestEnum
        {
            A = 1,
            B = 2,
            C = 4,
            D = 8,
            E = 16,
            F = 32,
            G = 64
        }
    }
}