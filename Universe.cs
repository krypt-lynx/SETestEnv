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
    public class TestGridProgramSetStorage
    {
        public static void IgnoreNext()
        {
            ignoreNext = true;
        }
        public static bool ignoreNext = false;
        public static void SetStorage_postfix(MyGridProgram __instance)
        {
            if (ignoreNext)
            {
                ignoreNext = false;
                return;
            }
            var testProgram = __instance as TestGridProgram;
            testProgram?.StorageDidChanged();
        }
    }

    internal class InputPipeline
    {
        static Queue<string> messages = new Queue<string>();
        
        public static bool HasValue() => messages.Count > 0;
        public static void Push(string msg) => messages.Enqueue(msg);
        public static string Pop() => messages.Dequeue();
    }

    public class Universe: ISimulationElement
    {
        static Universe()
        {
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

        public static void Echo(string msg)
        {
            Console.WriteLine("Echo: " + msg);
        }

        private void SimulationLoop()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            
            SimStart();

            string arg = Console.ReadLine();
            while (arg != "q")
            {
                if (arg != "")
                {
                    InputPipeline.Push(arg);
                }
                BeforeSimStep();
                SimStep();
                AfterSimStep();

                arg = Console.ReadLine();
            }

            SimEnd();

            Console.ReadLine();
        }

        public void Start()
        {
            SimulationLoop();
        }

        public void SimStart()
        {
            foreach (var grid in CubeGrids)
            {
                (grid as ISimulationElement)?.SimStart();
            }
        }

        public void BeforeSimStep()
        {
            foreach (var grid in CubeGrids)
            {
                (grid as ISimulationElement)?.BeforeSimStep();
            }
        }

        public void RegisterCubeGrid(TestCubeGrid cubeGrid)
        {
            CubeGrids.Add(cubeGrid);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Grid {cubeGrid.CustomName} added, {cubeGrid.Blocks.Count} block(s) inside");
        }

        public void SimStep()
        {
            foreach (var grid in CubeGrids)
            {
                (grid as ISimulationElement)?.SimStep();
            }
        }

        public void AfterSimStep()
        {
            foreach (var grid in CubeGrids)
            {
                (grid as ISimulationElement)?.AfterSimStep();
            }
        }

        public void SimEnd()
        {
            foreach (var grid in CubeGrids)
            {
                (grid as ISimulationElement)?.SimEnd();
            }
        }

        public List<IMyCubeGrid> CubeGrids { get; } = new List<IMyCubeGrid>();
    }
}
