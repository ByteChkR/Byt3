﻿using System;

namespace Byt3.OpenFL.Common.Instructions
{
    public static class RandomInstructionHelper
    {
        private static readonly Random Rnd = new Random();

        /// <summary>
        /// A function used as RandomFunc of type byte>
        /// </summary>
        /// <returns>a random byte</returns>
        public static byte Randombytesource()
        {
            return (byte) Rnd.Next(0, 256);
        }
    }
}