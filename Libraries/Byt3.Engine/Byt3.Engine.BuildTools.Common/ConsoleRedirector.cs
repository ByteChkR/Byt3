﻿using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Byt3.Engine.BuildTools.Common
{
    /// <summary>
    /// Redirects Console output of another application
    /// </summary>
    public class ConsoleRedirector
    {
        private TextReader _cEOut;
        private TextReader _cOut;
        private Action<string> _del;
        private Thread _errThread;
        private Process _proc;
        private Thread _thread;
        private readonly object _lockObj = new object();
        private bool _quitFlag;


        public static ConsoleRedirector CreateRedirector(StreamReader consoleOut, StreamReader errorConsoleOut,
            Process proc, Action<string> del = null)
        {
            ConsoleRedirector gcr = new ConsoleRedirector
            {
                _proc = proc,
                _cEOut = errorConsoleOut,
                _cOut = consoleOut,
                _del = del
            };
            return gcr;
        }

        public void StartThreads()
        {
            _quitFlag = false;
            if (_thread == null)
            {
                _thread = new Thread(() => Start(_cOut));
            }
            else
            {
                _thread.Abort();
            }

            if (_errThread == null)
            {
                _errThread = new Thread(() => Start(_cEOut));
            }
            else
            {
                _errThread.Abort();
            }

            _errThread.Start();
            _thread.Start();
        }

        public void StopThreads()
        {
            lock (_lockObj)
            {
                _quitFlag = true;
            }
        }

        public string GetRemainingLogs()
        {
            string ret = "";
            if (!(_cOut as StreamReader).EndOfStream)
            {
                ret += _cOut.ReadToEnd();
            }

            if (!(_cEOut as StreamReader).EndOfStream)
            {
                ret += _cEOut.ReadToEnd();
            }

            return ret;
        }

        public void Start(TextReader cout)
        {
            string txt = "";
            while (!_proc.HasExited)
            {
                lock (_lockObj)
                {
                    if (_quitFlag)
                    {
                        Console.WriteLine("Quitting From Loop");
                        return;
                    }
                }

                txt = cout.ReadLine();
                if (txt != "")
                {
                    _del?.Invoke(txt);
                }
            }
        }
    }
}