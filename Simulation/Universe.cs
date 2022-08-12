using HarmonyLib;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Game;
using VRage.Game.ModAPI.Ingame;
using VRage.ObjectBuilders;

namespace SETestEnv
{
    internal class InputPipeline
    {
        static Queue<string> messages = new Queue<string>();
        
        public static bool HasValue() => messages.Count > 0;
        public static void Push(string msg) => messages.Enqueue(msg);
        public static string Pop() => messages.Dequeue();
    }

    public class Universe : IEventPipeline, ISimulationElement
    {
        List<ISimulationElement> Update1List = new List<ISimulationElement>();
        List<ISimulationElement> Update10List = new List<ISimulationElement>();
        List<ISimulationElement> Update100List = new List<ISimulationElement>();

        static long age = 0;

        static Universe()
        {
            PatchStorage();

            InitObjectBuilders();

            string test = null;
        }

        private static void InitObjectBuilders()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                MyObjectBuilderSerializer.RegisterFromAssembly(assembly);
                MyObjectBuilderType.RegisterFromAssembly(assembly, true);
                //MyXmlSerializerManager.RegisterFromAssembly(assembly);
                MyDefinitionManagerBase.RegisterTypesFromAssembly(assembly);
            }
        }

        private static void PatchStorage()
        {
            Harmony harmony = new Harmony("SETestEnv");
            harmony.Patch(AccessTools.PropertySetter(typeof(MyGridProgram), nameof(MyGridProgram.Storage)),
                postfix: new HarmonyMethod(typeof(MyGridProgramSetStorage), nameof(MyGridProgramSetStorage.SetStorage_postfix)));
        }

        public static void Echo(string msg)
        {
            ConsoleColor fg = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Echo: " + msg);
            Console.ForegroundColor = fg;
        }

        public List<IMyCubeGrid> CubeGrids { get; } = new List<IMyCubeGrid>();

        public void RegisterCubeGrid(TestCubeGrid cubeGrid)
        {
            CubeGrids.Add(cubeGrid);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Grid {cubeGrid.CustomName} added, {cubeGrid.Blocks.Count} block(s) inside");
        }

        public void Start()
        {
            if (CubeGrids.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Universe is empty");
            }

            SimulationLoop();
        }

        private void SimulationLoop()
        {
            universe.SimStart();

            bool stop = false;

            string arg;
            ConsoleKey cmd;

            while (!stop)
            {
                if (ReadInput(out cmd, out arg))
                {
                    switch (cmd)
                    {
                        case ConsoleKey.S:
                            universe.SimSave();
                            continue;
                        case ConsoleKey.Q:
                            stop = true;
                            continue;
                    }
                }

                if (arg != "")
                {
                    InputPipeline.Push(arg);
                }

                age += 1;

                universe.BeforeSimStep();
                universe.SimStep(UpdateType.None);
                universe.AfterSimStep();
            }

            universe.SimSave();
            universe.SimEnd();

            Console.ReadKey();
        }

        private bool ReadInput(out ConsoleKey cmd, out string arg)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;

            cmd = default(ConsoleKey);
            arg = "";
            var key = Console.ReadKey(true);
            if (key.Modifiers == ConsoleModifiers.Control)
            {

                cmd = key.Key;
                return true;
            }
            else if (key.Key == ConsoleKey.Enter)
            {
                arg = "";
                Console.WriteLine();
            }
            else
            {
                Console.Write(key.KeyChar);
                arg = key.KeyChar + Console.ReadLine();
            }

            return false;
        }

        ISimulationElement universe => this;
        IEventPipeline pipeline => this;

        void ISimulationElement.SimStart() => pipeline.Broadcast(new SimulationEvent("Start", x => x.SimStart()));
        void ISimulationElement.BeforeSimStep() => pipeline.Broadcast(new SimulationEvent("BeforeStep", x => x.BeforeSimStep()));
        void ISimulationElement.SimStep(UpdateType updateType) => pipeline.Broadcast(new SimulationEvent("Step", x => x.SimStep(updateType)));
        void ISimulationElement.AfterSimStep() => pipeline.Broadcast(new SimulationEvent("AfterStep", x => x.AfterSimStep()));
        void ISimulationElement.SimEnd() => pipeline.Broadcast(new SimulationEvent("End", x => x.SimEnd()));
        void ISimulationElement.SimSave() => pipeline.Broadcast(new SimulationEvent("Save", x => x.SimSave()));
       
        void IEventPipeline.Broadcast(SimulationEvent @event)
        {
            CubeGrids.BroadcastEvent(@event);
        }

    }
}
