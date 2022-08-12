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

    public class Universe: ISimulationElement
    {
        long univereseAge = 0;

        static Universe()
        {
            PatchStorage();

            InitObjectBuilders();
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

                universe.BeforeSimStep();
                universe.SimStep();
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

        void ISimulationElement.SimStart()
        {
            foreach (var grid in CubeGrids)
            {
                (grid as ISimulationElement)?.SimStart();
            }
        }

        void ISimulationElement.BeforeSimStep()
        {
            foreach (var grid in CubeGrids)
            {
                (grid as ISimulationElement)?.BeforeSimStep();
            }
        }

        void ISimulationElement.SimStep()
        {
            foreach (var grid in CubeGrids)
            {
                (grid as ISimulationElement)?.SimStep();
            }
        }

        void ISimulationElement.AfterSimStep()
        {
            foreach (var grid in CubeGrids)
            {
                (grid as ISimulationElement)?.AfterSimStep();
            }
        }

        void ISimulationElement.SimEnd()
        {
            foreach (var grid in CubeGrids)
            {
                (grid as ISimulationElement)?.SimEnd();
            }
        }
        
        void ISimulationElement.SimSave()
        {
            foreach (var grid in CubeGrids)
            {
                (grid as ISimulationElement)?.SimSave();
            }
        }
    }
}
