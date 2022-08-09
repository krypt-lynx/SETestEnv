using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using IModApiGridProgram = Sandbox.ModAPI.IMyGridProgram;
namespace SETestEnv
{
    public abstract class DeferredProgram
    {
        protected Action<MyGridProgram> Configurator;
        public DeferredProgram(Action<MyGridProgram> configurator = null)
        {
            Configurator = configurator;
        }

        public abstract MyGridProgram Create(Action<MyGridProgram> initializer);
    }

    public class DeferredProgram<T>: DeferredProgram where T : MyGridProgram
    {

        public DeferredProgram(Action<MyGridProgram> configurator = null) : base(configurator) { }

        override public MyGridProgram Create(Action<MyGridProgram> initializer)
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
        public TestIntergridCommunicationSystem IGS;
        public IMyProgrammableBlock Owner;
        public TestGridProgramRuntimeInfo Runtime;


        private MyGridProgram program;

        public ProgramLayer(
            DeferredProgram deferedProgram,
            IMyProgrammableBlock Owner,
            Action<string> Echo,
            TestGridTerminalSystem GTS,
            TestIntergridCommunicationSystem IGS)
        {
            this.deferedProgram = deferedProgram;
            this.Echo = Echo;
            this.GTS = GTS;
            this.IGS = IGS;
            this.Owner = Owner;
        }

        public void InitializeProgram()
        {
            Runtime = new TestGridProgramRuntimeInfo();

            program = deferedProgram.Create(program =>
            {
                ((IModApiGridProgram)program).Me = Owner;
                ((IModApiGridProgram)program).Echo = Echo;
                ((IModApiGridProgram)program).Runtime = Runtime;
                ((IModApiGridProgram)program).GridTerminalSystem = GTS;
                ((IModApiGridProgram)program).IGC_ContextGetter = () => IGS;
            });
        }


        public void RunMain(string argument, UpdateType? updateType = null)
        {
            UpdateType resolvedUpdateType = ResolveUpdateType(argument, updateType);

            Runtime.InitNewRun();

            Stopwatch stopwatch = Stopwatch.StartNew();
            ((IModApiGridProgram)program).Main(argument, resolvedUpdateType);

            stopwatch.Stop();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("execution finished in ");
            if (stopwatch.Elapsed.TotalMilliseconds >= 3)
            {
                //BufConsole.ForegroundColor = ConsoleColor.Red;
            }
            Console.Write($"{stopwatch.Elapsed.TotalMilliseconds / 1000f:0.#####}");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(" s");

            Runtime.LastRunTimeMs = stopwatch.Elapsed.TotalMilliseconds;
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

        internal void Stop()
        {
            ((IModApiGridProgram)program).Save();
        }
    }

}
