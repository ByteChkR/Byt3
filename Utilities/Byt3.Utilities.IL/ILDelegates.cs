namespace Byt3.Utilities.IL
{
    public static class ILDelegates
    {
        public delegate object TypeConstructor();


        #region Method Delegates Void

        public delegate object MethodDel_v(object instance);

        public delegate void MethodDel_v<T1>(object instance, T1 arg1);

        public delegate void MethodDel_v<T1, in T2>(object instance, T1 arg1, T2 arg2);

        public delegate void MethodDel_v<T1, in T2, in T3>(object instance, T1 arg1, T2 arg2, T3 arg3);

        public delegate void MethodDel_v<T1, in T2, in T3, in T4>(object instance, T1 arg1, T2 arg2, T3 arg3, T4 arg4);

        public delegate void
            MethodDel_v<T1, in T2, in T3, in T4, in T5>(
                object stance, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);

        public delegate void
            MethodDel_v<T1, in T2, in T3, in T4, in T5, in T6>(
                object stance, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);

        public delegate void
            MethodDel_v<T1, in T2, in T3, in T4, in T5, in T6, in T7>(
                object stance, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);

        public delegate void
            MethodDel_v<T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8>(
                object stance, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8);

        public delegate void
            MethodDel_v<T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9>(
                object stance, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9);

        public delegate void
            MethodDel_v<T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10>(
                object stance, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9,
                T10 arg10);

        public delegate void
            MethodDel_v<T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11>(
                object stance, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9,
                T10 arg10, T11 arg11);

        public delegate void
            MethodDel_v<T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12>(
                object stance, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9,
                T10 arg10, T11 arg11, T12 arg12);

        public delegate void
            MethodDel_v<T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13>(
                object stance, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9,
                T10 arg10, T11 arg11, T12 arg12, T13 arg13);

        public delegate void
            MethodDel_v<T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13,
                in T14>(
                object stance, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9,
                T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14);

        public delegate void
            MethodDel_v<T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13,
                in T14, in T15>(
                object stance, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9,
                T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15);

        public delegate void
            MethodDel_v<T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13,
                in T14, in T15, in T16>(
                object stance, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9,
                T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16);

        public delegate void
            MethodDel_v<T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13,
                in T14, in T15, in T16, in T17>(
                object stance, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9,
                T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17);

        public delegate void
            MethodDel_v<T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13,
                in T14, in T15, in T16, in T17, in T18>(
                object stance, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9,
                T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18);

        public delegate void
            MethodDel_v<T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13,
                in T14, in T15, in T16, in T17, in T18, in T19>(
                object stance, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9,
                T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18,
                T19 arg19);

        public delegate void
            MethodDel_v<T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13,
                in T14, in T15, in T16, in T17, in T18, in T19, in T20>(
                object stance, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9,
                T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18,
                T19 arg19, T20 arg20);

        #endregion

        #region Method Delegates

        public delegate object MethodDel(object instance);

        public delegate TOut MethodDel<out TOut>(object instance);

        public delegate TOut MethodDel<out TOut, in T1>(object instance, T1 arg1);

        public delegate TOut MethodDel<out TOut, in T1, in T2>(object instance, T1 arg1, T2 arg2);

        public delegate TOut MethodDel<out TOut, in T1, in T2, in T3>(object instance, T1 arg1, T2 arg2, T3 arg3);

        public delegate TOut MethodDel<out TOut, in T1, in T2, in T3, in T4>(object instance, T1 arg1, T2 arg2, T3 arg3,
            T4 arg4);

        public delegate TOut
            MethodDel<out TOut, in T1, in T2, in T3, in T4, in T5>(
                object instance, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);

        public delegate TOut
            MethodDel<out TOut, in T1, in T2, in T3, in T4, in T5, in T6>(
                object instance, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);

        public delegate TOut
            MethodDel<out TOut, in T1, in T2, in T3, in T4, in T5, in T6, in T7>(
                object instance, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);

        public delegate TOut
            MethodDel<out TOut, in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8>(
                object instance, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8);

        public delegate TOut
            MethodDel<out TOut, in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9>(
                object instance, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9);

        public delegate TOut
            MethodDel<out TOut, in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10>(
                object instance, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9,
                T10 arg10);

        public delegate TOut
            MethodDel<out TOut, in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11>(
                object instance, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9,
                T10 arg10, T11 arg11);

        public delegate TOut
            MethodDel<out TOut, in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12>(
                object stance, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9,
                T10 arg10, T11 arg11, T12 arg12);

        public delegate TOut
            MethodDel<out TOut, in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12,
                in T13>(
                object stance, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9,
                T10 arg10, T11 arg11, T12 arg12, T13 arg13);

        public delegate TOut
            MethodDel<out TOut, in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12,
                in T13, in T14>(
                object stance, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9,
                T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14);

        public delegate TOut
            MethodDel<out TOut, in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12,
                in T13, in T14, in T15>(
                object stance, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9,
                T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15);

        public delegate TOut
            MethodDel<out TOut, in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12,
                in T13, in T14, in T15, in T16>(
                object stance, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9,
                T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16);

        public delegate TOut
            MethodDel<out TOut, in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12,
                in T13, in T14, in T15, in T16, in T17>(
                object stance, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9,
                T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17);

        public delegate TOut
            MethodDel<out TOut, in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12,
                in T13, in T14, in T15, in T16, in T17, in T18>(
                object stance, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9,
                T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18);

        public delegate TOut
            MethodDel<out TOut, in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12,
                in T13, in T14, in T15, in T16, in T17, in T18, in T19>(
                object stance, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9,
                T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18,
                T19 arg19);

        public delegate TOut
            MethodDel<out TOut, in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12,
                in T13, in T14, in T15, in T16, in T17, in T18, in T19, in T20>(
                object stance, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9,
                T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18,
                T19 arg19, T20 arg20);

        #endregion
    }
}