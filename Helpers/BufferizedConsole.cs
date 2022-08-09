using ConsoleClassLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BufferizedConsole
{
    public static class BufConsole
    {
        static BufConsole()
        {
           // Console.CancelKeyPress += (sender, args) => CancelKeyPress?.Invoke(sender, args);

            defaultHandle = NativeConsole.GetStdHandle(NativeConsole.STD_OUTPUT_HANDLE);
            currentHandle = defaultHandle;


            NativeConsole.CONSOLE_SCREEN_BUFFER_INFO info;
            NativeConsole.GetConsoleScreenBufferInfo(currentHandle, out info);


            foregroundColor = (ConsoleColor)(info.wAttributes & 0x0F);
            backgroundColor = (ConsoleColor)((info.wAttributes & 0xF0) >> 4);

            bufferSize = new NativeConsole.COORD(80, 35);
        }

        static ConsoleColor backgroundColor;
        static ConsoleColor foregroundColor;
        static NativeConsole.COORD bufferSize;
        static IntPtr[] buffers;
        static int currentBuffer;
        static IntPtr currentHandle;
        static IntPtr defaultHandle;

        public static ConsoleColor BackgroundColor
        {
            get
            {
                return backgroundColor;
            }
            set
            {
                backgroundColor = value;
                NativeConsole.SetConsoleTextAttribute(currentHandle, (ushort)((ushort)foregroundColor + ((ushort)backgroundColor << 4)));
            }
        }

        public static void CreateBuffers()
        {
            buffers = new IntPtr[] {
                CreateBuffer(),
                CreateBuffer()
            };

            currentBuffer = 0;

            currentHandle = buffers[currentBuffer];
        }

        private static IntPtr CreateBuffer()
        {
            var buffer = NativeConsole.CreateConsoleScreenBuffer(
                                NativeConsole.GENERIC_READ | NativeConsole.GENERIC_WRITE,
                                1, IntPtr.Zero, NativeConsole.CONSOLE_TEXTMODE_BUFFER, IntPtr.Zero);
            NativeConsole.CONSOLE_SCREEN_BUFFER_INFO_EX info = NativeConsole.CONSOLE_SCREEN_BUFFER_INFO_EX.Create();
            NativeConsole.GetConsoleScreenBufferInfoEx(defaultHandle, ref info);
            info.dwSize = bufferSize;
            NativeConsole.SetConsoleScreenBufferInfoEx(buffer, info);

            return buffer;
        }

        public static void SwitchBuffer()
        {
            NativeConsole.SetConsoleActiveScreenBuffer(buffers[currentBuffer]);

            currentBuffer = (currentBuffer + 1) % 2;
            currentHandle = buffers[currentBuffer];

            NativeConsole.CHAR_INFO[] ci = new NativeConsole.CHAR_INFO[bufferSize.X * bufferSize.Y];
            NativeConsole.SMALL_RECT rect = new NativeConsole.SMALL_RECT(0, 0, (short)(bufferSize.X - 1), (short)(bufferSize.Y - 1));
            NativeConsole.WriteConsoleOutput(currentHandle, ci, bufferSize, new NativeConsole.COORD(), ref rect);

            NativeConsole.SetConsoleCursorPosition(currentHandle, new NativeConsole.COORD(0, 0));
        }

        public static int BufferHeight { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public static int BufferWidth { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public static bool CapsLock => Console.CapsLock;
        public static int CursorLeft {
            get => Console.CursorLeft;
            set => Console.CursorLeft = value;
        }
        public static int CursorSize => Console.CursorSize;
        public static int CursorTop => Console.CursorTop;
        public static bool CursorVisible => Console.CursorVisible;

        public static TextWriter Error => Console.Error;
        public static ConsoleColor ForegroundColor
        {
            get => foregroundColor;            
            set
            {
                foregroundColor = value;
                NativeConsole.SetConsoleTextAttribute(currentHandle, (ushort)((ushort)foregroundColor + ((ushort)backgroundColor << 4)));
            }
        }

        public static TextReader In => Console.In;
        public static Encoding InputEncoding => Console.InputEncoding;
        public static bool IsErrorRedirected => Console.IsErrorRedirected;
        public static bool IsInputRedirected => Console.IsInputRedirected;
        public static bool IsOutputRedirected => Console.IsOutputRedirected;
        public static bool KeyAvailable => Console.KeyAvailable;
        public static int LargestWindowHeight => Console.LargestWindowHeight;
        public static int LargestWindowWidth => Console.LargestWindowWidth;
        public static bool NumberLock => Console.NumberLock;
        public static TextWriter Out => Console.Out;
        public static Encoding OutputEncoding => Console.OutputEncoding;
        public static string Title => Console.Title;
        public static bool TreatControlCAsInput => Console.TreatControlCAsInput;
        public static int WindowHeight
        {
            get => Console.WindowHeight;
            set => Console.WindowHeight = value;
        }

        public static int WindowLeft
        {
            get => Console.WindowLeft;
            set => Console.WindowLeft = value;
        }

        public static int WindowTop
        {
            get => Console.WindowTop;
            set => Console.WindowTop = value;
        }

        public static int WindowWidth {
            get => Console.WindowWidth;
            set => Console.WindowWidth = value;
        }

#pragma warning disable 0067
        //public static event ConsoleCancelEventHandler CancelKeyPress;
#pragma warning restore 0067

        public static void Beep() => Console.Beep();
        public static void Beep(int frequency, int duration) => Console.Beep(frequency, duration);
        public static void Clear() => Console.Clear();
        public static void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft, int targetTop) => throw new NotImplementedException();
        public static void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft, int targetTop, char sourceChar, ConsoleColor sourceForeColor, ConsoleColor sourceBackColor) => throw new NotImplementedException();
        public static Stream OpenStandardError() => Console.OpenStandardError();
        public static Stream OpenStandardError(int bufferSize) => Console.OpenStandardError(bufferSize);
        public static Stream OpenStandardInput() => Console.OpenStandardInput();
        public static Stream OpenStandardInput(int bufferSize) => Console.OpenStandardInput(bufferSize);
        public static Stream OpenStandardOutput() => Console.OpenStandardOutput();
        public static Stream OpenStandardOutput(int bufferSize) => Console.OpenStandardOutput(bufferSize);
        public static int Read() => throw new NotImplementedException();
        public static ConsoleKeyInfo ReadKey()
        {
            NativeConsole.INPUT_RECORD[] keyInfo = new NativeConsole.INPUT_RECORD[1];

            uint nRead;
            while (NativeConsole.ReadConsoleInput(NativeConsole.GetStdHandle(NativeConsole.STD_INPUT_HANDLE), keyInfo, 1, out nRead) && nRead != 0)
            {
                if (keyInfo[0].EventType == NativeConsole.InputEventType.Key &&
                    keyInfo[0].KeyEvent.bKeyDown &&
                    keyInfo[0].KeyEvent.UnicodeChar != 0)
                {
                    var test1 = new ConsoleKeyInfo(keyInfo[0].KeyEvent.UnicodeChar,
                        (ConsoleKey)keyInfo[0].KeyEvent.wVirtualKeyCode,
                        (keyInfo[0].KeyEvent.dwControlKeyState & NativeConsole.ControlKeyStates.ShiftPressed) != 0,
                        (keyInfo[0].KeyEvent.dwControlKeyState & (NativeConsole.ControlKeyStates.LeftAltPressed | NativeConsole.ControlKeyStates.RightAltPressed)) != 0,
                        (keyInfo[0].KeyEvent.dwControlKeyState & (NativeConsole.ControlKeyStates.LeftCtrlPressed | NativeConsole.ControlKeyStates.RightAltPressed)) != 0
                        );
                    return test1;
                }
            }

            throw new Exception();
        }

        public static ConsoleKeyInfo ReadKey(bool intercept) => Console.ReadKey(intercept);
        public static string ReadLine() => Console.ReadLine();
        public static void ResetColor() => Console.ResetColor();
        public static void SetBufferSize(int width, int height) => throw new NotImplementedException();
        public static void SetCursorPosition(int left, int top) => throw new NotImplementedException();
        public static void SetError(TextWriter newError) => Console.SetError(newError);
        public static void SetIn(TextReader newIn) => Console.SetIn(newIn);
        public static void SetOut(TextWriter newOut) => Console.SetOut(newOut);
        public static void SetWindowPosition(int left, int top) => Console.SetWindowPosition(left, top);
        public static void SetWindowSize(int width, int height) => Console.SetWindowSize(width, height);
        public static void Write(long value) => Write(value.ToString(Out.FormatProvider));

        public static void Write(string value)
        {
            uint written;
            NativeConsole.WriteConsoleW(currentHandle, value, value.Length, out written, IntPtr.Zero);
        }

        public static void Write(ulong value) => Write(value.ToString(Out.FormatProvider));
        public static void Write(uint value) => Write(value.ToString(Out.FormatProvider));
        public static void Write(object value)
        {
            Write(value.ToString());
        }
        public static void Write(float value) => Write(value.ToString(Out.FormatProvider));
        public static void Write(decimal value) => Write(value.ToString(Out.FormatProvider));
        public static void Write(double value) => Write(value.ToString(Out.FormatProvider));
        public static void Write(char[] buffer) => throw new NotImplementedException();
        public static void Write(char value) => Write(value.ToString(Out.FormatProvider));
        public static void Write(bool value) => Write(value.ToString(Out.FormatProvider));
        public static void Write(int value) => Write(value.ToString(Out.FormatProvider));
        public static void Write(string format, object arg0) => Write(string.Format(format, arg0));
        public static void Write(string format, params object[] arg) => Write(string.Format(format, arg));
        public static void Write(string format, object arg0, object arg1) => Write(string.Format(format, arg0, arg1));
        public static void Write(char[] buffer, int index, int count) => new NotImplementedException();
        public static void Write(string format, object arg0, object arg1, object arg2) => Write(string.Format(format, arg0, arg1, arg2));
        public static void Write(string format, object arg0, object arg1, object arg2, object arg3) => Write(string.Format(format, arg0, arg1, arg2, arg3));
        public static void WriteLine() => Write("\r\n");
        public static void WriteLine(bool value) { Write(value); WriteLine(); }
        public static void WriteLine(float value) { Write(value); WriteLine(); }
        public static void WriteLine(int value) { Write(value); WriteLine(); }
        public static void WriteLine(uint value) { Write(value); WriteLine(); }
        public static void WriteLine(long value) { Write(value); WriteLine(); }
        public static void WriteLine(ulong value) { Write(value); WriteLine(); }
        public static void WriteLine(object value) { Write(value); WriteLine(); }
        public static void WriteLine(string value) { Write(value); WriteLine(); }
        public static void WriteLine(double value) { Write(value); WriteLine(); }
        public static void WriteLine(decimal value) { Write(value); WriteLine(); }
        public static void WriteLine(char[] buffer) { Write(buffer); WriteLine(); }
        public static void WriteLine(char value) { Write(value); WriteLine(); }
        public static void WriteLine(string format, params object[] arg) { Write(format, arg); WriteLine(); }
        public static void WriteLine(string format, object arg0) { Write(format, arg0); WriteLine(); }

        public static void WriteLine(string format, object arg0, object arg1) { Write(format, arg0, arg1); WriteLine(); }
        public static void WriteLine(char[] buffer, int index, int count) { Write(buffer, index, count); WriteLine(); }
        public static void WriteLine(string format, object arg0, object arg1, object arg2) { Write(format, arg0, arg1, arg2); WriteLine(); }
        public static void WriteLine(string format, object arg0, object arg1, object arg2, object arg3) { Write(format, arg0, arg1, arg2, arg3); WriteLine(); }
    }
}
