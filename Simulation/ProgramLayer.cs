using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using IModApiGridProgram = Sandbox.ModAPI.IMyGridProgram;

namespace SETestEnv
{
    public class MyGridProgramSetStorage
    {
        public static void SetStorage_postfix(MyGridProgram __instance) =>
            (__instance.Runtime as TestGridProgramRuntimeInfo)?.ProgramLayer.StorageDidChanged();        
    }

    public abstract class DeferredProgram
    {
        protected Action<IModApiGridProgram> Configurator;
        public DeferredProgram(Action<IModApiGridProgram> configurator = null)
        {
            Configurator = configurator;
        }

        public abstract MyGridProgram Create(Action<IModApiGridProgram> initializer);
    }

    public class DeferredProgram<T>: DeferredProgram where T : MyGridProgram
    {
        public DeferredProgram(Action<IModApiGridProgram> customConfigurator = null) : base(customConfigurator) { }

        override public MyGridProgram Create(Action<IModApiGridProgram> initializer)
        {
            var program = (T)FormatterServices.GetUninitializedObject(typeof(T));
            ConstructorInfo constructor = typeof(T).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);

            initializer?.Invoke(program);
            Configurator?.Invoke(program);

            constructor.Invoke(program, null);

            return program;
        }
    }

    public class ProgramLayer 
    {
        private DeferredProgram deferedProgram;

        public Action<string> Echo;
        public TestGridTerminalSystem GTS;
        public TestIntergridCommunicationSystem IGC;
        public IMyProgrammableBlock Owner;
        public TestGridProgramRuntimeInfo Runtime;

        private IModApiGridProgram program;

        public ProgramLayer(
            DeferredProgram deferedProgram,
            IMyProgrammableBlock Owner,
            Action<string> Echo,
            TestGridTerminalSystem GTS,
            TestIntergridCommunicationSystem IGC)
        {
            this.deferedProgram = deferedProgram;
            this.Echo = Echo;
            this.GTS = GTS;
            this.IGC = IGC;
            this.Owner = Owner;
        }

        #region MyGridProgram lifecycle

        public void InitializeProgram()
        {
            Runtime = new TestGridProgramRuntimeInfo(this);


            program = deferedProgram.Create(program =>
            {
                program.Me = Owner;
                program.Echo = Echo;
                program.Runtime = Runtime;
                program.GridTerminalSystem = GTS;
                program.IGC_ContextGetter = () => IGC;
                program.Storage = LoadStorage();
            });
        }

        public void RunMain(string argument, UpdateType? updateType = null)
        {
            UpdateType resolvedUpdateType = ResolveUpdateType(argument, updateType);

            Runtime.InitNewRun();

            Stopwatch stopwatch = Stopwatch.StartNew();
            program.Main(argument, resolvedUpdateType);
            stopwatch.Stop();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("execution finished in ");
            if (stopwatch.Elapsed.TotalMilliseconds >= 5)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            Console.Write($"{stopwatch.Elapsed.TotalMilliseconds / 1000f:0.#####}");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(" s");

            Runtime.LastRunTimeMs = stopwatch.Elapsed.TotalMilliseconds;
        }

        internal void Save()
        {
            program.Save();
        }

        #endregion

        #region Storage handling

        static string appRoot = AppDomain.CurrentDomain.BaseDirectory;
        const string StorageFileFormat = "Storage_{0}.txt";
        string StorageFile => string.Format(StorageFileFormat, Owner.EntityId);
        // invoked by harmony patch
        public void StorageDidChanged() 
        {
            if (program != null)
            {
                SaveStorage(program.Storage);
            }
        }

        public void SaveStorage(string storage)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("saved Storage value:\n{0}", storage);
            string path = Path.Combine(appRoot, StorageFile);
            System.IO.File.WriteAllText(path, storage);
        }

        public string LoadStorage()
        {
            string path = Path.Combine(appRoot, StorageFile);
            string storage = "";
            if (File.Exists(path))
            {
                storage = File.ReadAllText(path);
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("loaded Storage value:\n{0}", storage);
            return storage;
        }

        #endregion


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

    }

}
