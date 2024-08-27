﻿using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace Bolt
{
    public static class ConsoleWriter
    {
#if UNITY_STANDALONE_WIN
        private static class PInvoke
        {
            public const int STD_OUTPUT_HANDLE = -11;

            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool AttachConsole(uint dwProcessId);

            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool AllocConsole();

            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool FreeConsole();

            [DllImport("kernel32.dll", EntryPoint = "GetStdHandle", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
            public static extern IntPtr GetStdHandle(int nStdHandle);

            [DllImport("kernel32.dll")]
            public static extern bool SetConsoleTitle(string lpConsoleTitle);
        }

        private static TextWriter realOut;
#endif

        public static void Open()
        {
#if UNITY_STANDALONE_WIN
            if (realOut == null)
            {
                realOut = Console.Out;
            }

            var hasConsole = PInvoke.AttachConsole(0x0ffffffff);
            if (hasConsole == false)
            {
                PInvoke.AllocConsole();
            }

            try
            {
                // grab handle ptr
                IntPtr outHandlePtr = PInvoke.GetStdHandle(PInvoke.STD_OUTPUT_HANDLE);

                // we can then create a filestream from this handle
#pragma warning disable 0618
                var fileStream = new FileStream(outHandlePtr, FileAccess.Write);
#pragma warning restore 0618

                // and then create a new stream writer (using ASCII) 
                var stdOut = new StreamWriter(fileStream, Encoding.ASCII);
                stdOut.AutoFlush = true;

                // and force unity to use this
                Console.SetOut(stdOut);
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }
#endif
        }

        public static void Close()
        {
#if UNITY_STANDALONE_WIN
            PInvoke.FreeConsole();

            Console.SetOut(realOut);
            realOut = null;
#endif
        }
    }
}