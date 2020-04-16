using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Byt3.ADL.Tests
{
    public class MaskOperations
    {
        [Test]
        public void ADL_Mask_IsContainedInMask_Test()
        {
            BitMask<char> bm1 = new BitMask<char>(1 | 4 | 16);
            BitMask<char> bm2 = new BitMask<char>(4);
            BitMask<char> bm3 = new BitMask<char>(2);


            Assert.True(BitMask.IsContainedInMask(bm1, bm2, false)); //True
            Assert.False(BitMask.IsContainedInMask(bm1, bm3, false)); //False
            Assert.False(BitMask.IsContainedInMask(bm1, bm2 | bm3, true)); //False
        }

        [Test]
        public void ADL_Mask_GetUniqueMaskSet_Test()
        {
            BitMask<char> bm = new BitMask<char>(1 | 2 | 8 | 16 | 64);
            List<int> ret = BitMask.GetUniqueMasksSet(bm);

            Assert.AreEqual(5, ret.Count);
            Assert.Contains(1, ret);
            Assert.Contains(2, ret);
            Assert.Contains(8, ret);
            Assert.Contains(16, ret);
            Assert.Contains(64, ret);
        }

        [Test]
        public void ADL_Mask_IsUniqueMask_Test()
        {
            BitMask<char> bm = new BitMask<char>(2 | 4);
            BitMask<char> bm1 = new BitMask<char>(8);

            Assert.True(BitMask.IsUniqueMask(bm1));
            Assert.False(BitMask.IsUniqueMask(bm));
        }

        [Test]
        public void ADL_Mask_CombineMasks_Test()
        {
            BitMask<TestEnum> bm1 = new BitMask<TestEnum>(TestEnum.B | TestEnum.D | TestEnum.F);
            BitMask<TestEnum> bm2 = new BitMask<TestEnum>(TestEnum.B | TestEnum.E | TestEnum.G);
            BitMask ret = BitMask.CombineMasks(MaskCombineType.BitAnd, bm1, bm2);
            Assert.True(ret == 2);
            ret = BitMask.CombineMasks(MaskCombineType.BitOr, bm1, bm2);
            Assert.True(BitMask.IsContainedInMask(ret, 16, false));
            Assert.True(BitMask.IsContainedInMask(ret, 2, false));
        }

        [Test]
        public void ADL_Mask_RemoveFlags_Test()
        {
            BitMask<char> bm1 = new BitMask<char>(2 | 8 | 16);
            BitMask<char> ret = BitMask.RemoveFlags(bm1, 2);
            Assert.False(BitMask.IsContainedInMask(ret, 2, true));
            Assert.True(BitMask.IsContainedInMask(bm1, 2, true));
        }

        [Test]
        public void ADL_Mask_Constructors_Test()
        {
            BitMask bm = new BitMask(2, 8, 16);
            BitMask bm1 = new BitMask(2 | 8 | 16);


            Assert.AreEqual((int) bm, (int) bm1);
        }

        [Test]
        public void ADL_Mask_FlagOperations_Test()
        {
            BitMask bm = new BitMask(true);
            bm.SetAllFlags(2 | 4);
            Assert.AreEqual(6, (int) bm);

            bm.SetFlag(2 | 4, false);
            bm.SetFlag(16 | 8, true);
            Assert.AreEqual((int) bm, 16 | 8);


            Assert.True(bm.HasFlag(16, MaskMatchType.MatchOne));
            Assert.False(bm.HasFlag(24 | 4, MaskMatchType.MatchAll));
            bm.SetAllFlags(0);
            bm.Flip();
            Assert.True(-1 == bm);

            Assert.True(BitMask.CombineMasks(MaskCombineType.BitAnd) == 0);
            Assert.True(BitMask.IsUniqueMask(2));
            Assert.False(BitMask.IsUniqueMask(3));
            Assert.False(BitMask.IsUniqueMask(0));
            BitMask<TestEnum> gbm = new BitMask<TestEnum>(TestEnum.A, TestEnum.B);
            Assert.False(BitMask.IsUniqueMask(bm));
            Assert.True(gbm.HasFlag(TestEnum.A, MaskMatchType.MatchOne));

            gbm.SetFlag(TestEnum.C, true);

            //BitMask<TestEnum> gbm1 = TestEnum.C;

            Assert.False(gbm.HasFlag(TestEnum.C, MaskMatchType.MatchOne));

            gbm.SetFlag(TestEnum.C, false);

            Assert.False(gbm.HasFlag(TestEnum.C, MaskMatchType.MatchOne));

            gbm.SetAllFlags(TestEnum.C | TestEnum.A);
            Assert.True(gbm.HasFlag(TestEnum.C | TestEnum.A, MaskMatchType.MatchAll));
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