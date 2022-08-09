using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.ObjectBuilders;
using VRageMath;
using VRage.Collections;
using System.Diagnostics;
using BufferizedConsole;
using System.IO;
using System.Reflection;
using HarmonyLib;

namespace SETestEnv
{

    
    public abstract class TestGridProgram : MyGridProgram
    {
        static TestGridProgram()
        {
            // Initializing internal SE states to make MyDefinitionId.TryParse work

            PatchStorage();
        }

        private static void PatchStorage()
        {
            Harmony harmony = new Harmony(nameof(TestGridProgram));
            harmony.Patch(AccessTools.PropertySetter(typeof(MyGridProgram), nameof(MyGridProgram.Storage)),
                postfix: new HarmonyMethod(typeof(TestGridProgramSetStorage), nameof(TestGridProgramSetStorage.SetStorage_postfix)));
        }


        static string appRoot = AppDomain.CurrentDomain.BaseDirectory;
        const string StorageFile = "Storage.txt";

        public TestGridTerminalSystem TestGridTerminalSystem => this.GridTerminalSystem as TestGridTerminalSystem;
        public TestGridProgramRuntimeInfo TestRuntime => this.Runtime as TestGridProgramRuntimeInfo;

        public void StorageDidChanged() // invoked by Harmony patch
        {
            SaveStorage();
        }

        public void SetStorage(string value)
        {
            Storage = value;
        }

        public void SaveStorage()
        {
            BufConsole.ForegroundColor = ConsoleColor.Yellow;
            BufConsole.WriteLine("saved Storage value:\n{0}", Storage);
            string path = Path.Combine(appRoot, StorageFile);
            System.IO.File.WriteAllText(path, Storage);
        }

        public void LoadStorage()
        {
            string path = Path.Combine(appRoot, StorageFile);
            TestGridProgramSetStorage.IgnoreNext();
            if (File.Exists(path))
            {
                Storage = File.ReadAllText(path);
            }
            else
            {
                Storage = "";
            }
            BufConsole.ForegroundColor = ConsoleColor.Yellow;
            BufConsole.WriteLine("loaded Storage value:\n{0}", Storage);
        }

        public static TestProgrammableBlock FutureOwner = null;
        public static TestCubeGrid FutureCubeGrid = null;

        // it looks like it may be possible to use MyIntergridCommunicationContext (except it is an internal class)
        IMyIntergridCommunicationSystem igc = new TestIntergridCommunicationSystem(FutureOwner.EntityId);

        public TestGridProgram()
        {
            LoadStorage();

            this.Echo = EchoImpl;

            this.GridTerminalSystem = new TestGridTerminalSystem(FutureOwner, FutureCubeGrid);

            this.Runtime = new TestGridProgramRuntimeInfo();
            this.Me = FutureOwner;

            ((Sandbox.ModAPI.IMyGridProgram)this).IGC_ContextGetter = () => igc;
        }

        private void EchoImpl(string str)
        {
            ConsoleColor fg = BufConsole.ForegroundColor;
            BufConsole.ForegroundColor = ConsoleColor.White;
            BufConsole.WriteLine("Echo: " + str);
            BufConsole.ForegroundColor = fg;
        }

        public virtual void RunMain(string argument, UpdateType? updateType = null)
        {
            UpdateType resolvedUpdateType = ResolveUpdateType(argument, updateType);

            TestRuntime.InitNewRun();

            Stopwatch stopwatch = Stopwatch.StartNew();
            InvokeMain(argument, resolvedUpdateType);
            stopwatch.Stop();

            BufConsole.ForegroundColor = ConsoleColor.Yellow;
            BufConsole.Write("execution finished in ");
            if (stopwatch.Elapsed.TotalMilliseconds >= 3)
            {
                //BufConsole.ForegroundColor = ConsoleColor.Red;
            }
            BufConsole.Write($"{stopwatch.Elapsed.TotalMilliseconds / 1000f:0.#####}");
            BufConsole.ForegroundColor = ConsoleColor.Yellow;
            BufConsole.WriteLine(" s");

            TestRuntime.LastRunTimeMs = stopwatch.Elapsed.TotalMilliseconds;
        }

        private UpdateType ResolveUpdateType(string argument, UpdateType? updateType)
        {
            UpdateType resolvedUpdateType = UpdateType.None;

            if (updateType.HasValue)
            {
                resolvedUpdateType = updateType.Value;
            }
            else
            {
                if (argument.Length > 0)
                {
                    resolvedUpdateType = UpdateType.Terminal;
                }
                else
                {
                    if (Runtime.UpdateFrequency.Contains(UpdateFrequency.Once))
                        resolvedUpdateType |= UpdateType.Once;
                    if (Runtime.UpdateFrequency.Contains(UpdateFrequency.Update1))
                        resolvedUpdateType |= UpdateType.Update1;
                    if (Runtime.UpdateFrequency.Contains(UpdateFrequency.Update10))
                        resolvedUpdateType |= UpdateType.Update10;
                    if (Runtime.UpdateFrequency.Contains(UpdateFrequency.Update100))
                        resolvedUpdateType |= UpdateType.Update100;
                }
            }

            return resolvedUpdateType;
        }

        private void InvokeMain(string argument, UpdateType updateType)
        {
            ((Sandbox.ModAPI.IMyGridProgram)this).Main(argument, updateType);
        }
    }    
}