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
using Console_2;
using System.IO;
using System.Reflection;

namespace SETestEnv
{

    public class TestCubeGrid : IMyCubeGrid
    {
        private List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>();
        public List<IMyTerminalBlock> Blocks
        {
            get
            {
                return blocks;
            }
        }

        public void RegisterBlock<T>(TestTerminalBlock<T> block) where T : MyObjectBuilder_Base
        {
            blocks.Add(block);
            block.OwnerGrid = this;
        }

        public MyEntityComponentContainer Components
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public long EntityId
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public float GridSize
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public MyCubeSize GridSizeEnum
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsStatic
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Vector3I Max
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Vector3I Min
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public BoundingBoxD WorldAABB
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public BoundingBoxD WorldAABBHr
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public MatrixD WorldMatrix
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public BoundingSphereD WorldVolume
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public BoundingSphereD WorldVolumeHr
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string CustomName { get; set; } = "";

        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string DisplayName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool HasInventory
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int InventoryCount
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool Closed
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool CubeExists(Vector3I pos)
        {
            return true;
            //throw new NotImplementedException();
        }

        public IMySlimBlock GetCubeBlock(Vector3I pos)
        {
         //   try
            {
                if (pos.X < 0)
                    return null;
                if (pos.X >= Blocks.Count)
                    return null;
                return Blocks[pos.X] as IMySlimBlock;
            }
          /*  catch
            {
                return null;
            }*/

        }

        public Vector3D GetPosition()
        {
            throw new NotImplementedException();
        }

        public Vector3D GridIntegerToWorld(Vector3I gridCoords)
        {
            throw new NotImplementedException();
        }

        public Vector3I WorldToGridInteger(Vector3D coords)
        {
            throw new NotImplementedException();
        }

        public IMyInventory GetInventory()
        {
            throw new NotImplementedException();
        }

        public IMyInventory GetInventory(int index)
        {
            throw new NotImplementedException();
        }

        public bool IsSameConstructAs(IMyCubeGrid other)
        {
            throw new NotImplementedException();
        }
    }

    public class TestBlockGroup : IMyBlockGroup
    {
        public string Name { get; set; }
        public List<IMyTerminalBlock> Blocks { get; set; }

        public void GetBlocks(List<IMyTerminalBlock> blocks, Func<IMyTerminalBlock, bool> collect = null)
        {
            throw new NotImplementedException();
        }

        public void GetBlocksOfType<T>(List<T> blocks, Func<T, bool> collect = null) where T : class
        {
            throw new NotImplementedException();
        }

        public void GetBlocksOfType<T>(List<IMyTerminalBlock> blocks, Func<IMyTerminalBlock, bool> collect = null) where T : class
        {
            throw new NotImplementedException();
        }
    }

    public class TestGridTerminalSystem : IMyGridTerminalSystem
    {
        public TestCubeGrid CubeGrid;
#pragma warning disable 0649
        public Dictionary<string, TestBlockGroup> Groups;
#pragma warning restore 0649

        public TestProgrammableBlock ownerBlock = null;

        public TestGridTerminalSystem(TestProgrammableBlock owner, TestCubeGrid cubeGrid) : base()
        {
            CubeGrid = cubeGrid;
            this.ownerBlock = owner;
            CubeGrid.RegisterBlock(ownerBlock);
        }

        public bool CanAccess(IMyTerminalBlock block, MyTerminalAccessScope scope = MyTerminalAccessScope.All)
        {
            throw new NotImplementedException();
        }

        public bool CanAccess(IMyCubeGrid grid, MyTerminalAccessScope scope = MyTerminalAccessScope.All)
        {
            throw new NotImplementedException();
        }

        public void GetBlockGroups(List<IMyBlockGroup> blockGroups, Func<IMyBlockGroup, bool> collect = null)
        {
            blockGroups.Clear();
        }

        public IMyBlockGroup GetBlockGroupWithName(string name)
        {
            return Groups?.ContainsKey(name) ?? false ? Groups[name] : null;
        }

        public void GetBlocks(List<IMyTerminalBlock> blocks)
        {
            blocks.Clear();
            blocks.AddRange(CubeGrid.Blocks);
        }

        public void GetBlocksOfType<T>(List<T> blocks, Func<T, bool> collect = null) where T : class
        {
            blocks.Clear();
            blocks.AddRange(CubeGrid.Blocks.Where(x => x is T).Cast<T>().Where(x => collect?.Invoke(x) ?? true));
        }


        public void GetBlocksOfType<T>(List<IMyTerminalBlock> blocks, Func<IMyTerminalBlock, bool> collect = null) where T : class
        {
            blocks.Clear();
            blocks.AddRange(CubeGrid.Blocks.Where(x => x is T).Where(x => collect(x)));
        }

        public IMyTerminalBlock GetBlockWithId(long id)
        {
            return null;
        }

        public IMyTerminalBlock GetBlockWithName(string name)
        {
            return CubeGrid.Blocks.Where(x => x.CustomName == name).FirstOrDefault();
        }

        public void SearchBlocksOfName(string name, List<IMyTerminalBlock> blocks, Func<IMyTerminalBlock, bool> collect = null)
        {
            throw new NotImplementedException();
        }
    }


    public class TestGridProgramRuntimeInfo : IMyGridProgramRuntimeInfo
    {
        private int simInstructionCount = 0;

        public void InitNewRun()
        {
            simInstructionCount = 0;
            updateFrequency = updateFrequency & ~UpdateFrequency.Once;
        }

        public int CurrentInstructionCount
        {
            get
            {
                simInstructionCount += 10;
                return simInstructionCount;
            }
        }

        public int CurrentMethodCallCount
        {
            get
            {
                return 0;
            }
        }

        public double LastRunTimeMs
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int MaxInstructionCount
        {
            get
            {
                return 50000;
            }
        }

        public int MaxMethodCallCount
        {
            get
            {
                return 50000;
            }
        }

        public TimeSpan TimeSinceLastRun
        {
            get
            {
                return new TimeSpan(0, 0, 0, 1);
            }
        }

        public int MaxCallChainDepth
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int CurrentCallChainDepth
        {
            get
            {
                throw new NotImplementedException();
            }
        }


        private UpdateFrequency updateFrequency = UpdateFrequency.None;
        public UpdateFrequency UpdateFrequency
        {
            get
            {
                return updateFrequency;
            }

            set
            {
                updateFrequency = value;
                Console2.ForegroundColor = ConsoleColor.Yellow;
                Console2.WriteLine("new UpdateFrequency value: {0}", value);
            }
        }
    }


    public abstract class TestGridProgram : MyGridProgram
    {
        static TestGridProgram()
        {
            // Initializing internal SE states to make MyDefinitionId.TryParse work
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                MyObjectBuilderSerializer.RegisterFromAssembly(assembly);
                MyObjectBuilderType.RegisterFromAssembly(assembly, true);
                //MyXmlSerializerManager.RegisterFromAssembly(assembly);
                MyDefinitionManagerBase.RegisterTypesFromAssembly(assembly);
            }
        }

        static string appRoot = System.AppDomain.CurrentDomain.BaseDirectory;
        const string StorageFile = "Storage.txt";

        public TestGridTerminalSystem TestGridTerminalSystem
        {
            get
            {
                return this.GridTerminalSystem as TestGridTerminalSystem;
            }
        }

        public void SetStorage(string value)
        {
            Storage = value;
        }

        public void SaveStorage()
        {
            Console2.ForegroundColor = ConsoleColor.Yellow;
            Console2.WriteLine("saved Storage value:\n{0}", Storage);
            string path = Path.Combine(appRoot, StorageFile);
            System.IO.File.WriteAllText(path, Storage);
        }

        public void LoadStorage()
        {
            string path = Path.Combine(appRoot, StorageFile);
            if (File.Exists(path))
            {
                Storage = File.ReadAllText(path);
            }
            else
            {
                Storage = "";
            }
            Console2.ForegroundColor = ConsoleColor.Yellow;
            Console2.WriteLine("loaded Storage value:\n{0}", Storage);
        }

        // todo: regression
        /*
        public override string Storage
        {
            get
            {
                string path = Path.Combine(appRoot, StorageFile);
                if (File.Exists(path))
                {
                    return File.ReadAllText(path);
                }
                else
                {
                    return "";
                }
            }

            protected set
            {
                Console2.ForegroundColor = ConsoleColor.Yellow;
                Console2.WriteLine("new Storage value:\n{0}", value);
                string path = Path.Combine(appRoot, StorageFile);
                System.IO.File.WriteAllText(path, value);
            }
        }
        */

        public static TestProgrammableBlock FutureOwner = null;
        public static TestCubeGrid FutureCubeGrid = null;
        //  public static string FutureStorage = "";


        private static Dictionary<string, MyObjectBuilderType> m_typeByName = new Dictionary<string, MyObjectBuilderType>(500);
        private static ushort m_idCounter;
        private static Dictionary<MyRuntimeObjectBuilderId, MyObjectBuilderType> m_typeById = new Dictionary<MyRuntimeObjectBuilderId, MyObjectBuilderType>(500, MyRuntimeObjectBuilderId.Comparer);
        private static Dictionary<MyObjectBuilderType, MyRuntimeObjectBuilderId> m_idByType = new Dictionary<MyObjectBuilderType, MyRuntimeObjectBuilderId>(500, MyObjectBuilderType.Comparer);
        // Token: 0x04000900 RID: 2304
        private static Dictionary<string, MyObjectBuilderType> m_typeByLegacyName = new Dictionary<string, MyObjectBuilderType>(500);

        internal static void RegisterLegacyName(MyObjectBuilderType type, string legacyName)
        {
            m_typeByLegacyName.Add(legacyName, type);
        }

        private static void RegisterFromAssembly(Assembly assembly, bool registerLegacyNames = false)
        {
            if (assembly == null)
            {
                return;
            }
            Type typeFromHandle = typeof(MyObjectBuilder_Base);
            Type[] types = assembly.GetTypes();
            Array.Sort<Type>(types, VRage.Reflection.FullyQualifiedNameComparer.Default);
            foreach (Type type in types)
            {
                if (typeFromHandle.IsAssignableFrom(type) && !m_typeByName.ContainsKey(type.Name))
                {
                    MyObjectBuilderType myObjectBuilderType = new MyObjectBuilderType(type);
                    MyRuntimeObjectBuilderId myRuntimeObjectBuilderId = new MyRuntimeObjectBuilderId(m_idCounter += 1);
                    m_typeById.Add(myRuntimeObjectBuilderId, myObjectBuilderType);
                    m_idByType.Add(myObjectBuilderType, myRuntimeObjectBuilderId);
                    m_typeByName.Add(type.Name, myObjectBuilderType);
                    if (registerLegacyNames && type.Name.StartsWith("MyObjectBuilder_"))
                    {
                        RegisterLegacyName(myObjectBuilderType, type.Name.Substring("MyObjectBuilder_".Length));
                    }
                    object[] customAttributes = type.GetCustomAttributes(typeof(MyObjectBuilderDefinitionAttribute), true);
                    if (customAttributes.Length != 0)
                    {
                        MyObjectBuilderDefinitionAttribute myObjectBuilderDefinitionAttribute = (MyObjectBuilderDefinitionAttribute)customAttributes[0];
                        if (!string.IsNullOrEmpty(myObjectBuilderDefinitionAttribute.LegacyName))
                        {
                            RegisterLegacyName(myObjectBuilderType, myObjectBuilderDefinitionAttribute.LegacyName);
                        }
                    }
                }
            }
        }

        public TestGridProgram()
        {
            RegisterFromAssembly(Assembly.GetAssembly(typeof(TestGridProgram)));
            MyObjectBuilderType.RegisterFromAssembly(Assembly.GetAssembly(typeof(TestGridProgram)));

            LoadStorage();

            this.Echo = EchoImpl;
#pragma warning disable 618
            // this.ElapsedTime = new TimeSpan(0);
#pragma warning restore 618
            this.GridTerminalSystem = new TestGridTerminalSystem(FutureOwner, FutureCubeGrid);

            this.Runtime = new TestGridProgramRuntimeInfo();
            this.Me = FutureOwner;

           // this.Storage = System.IO.File.ReadAllText(@"D:\Storage.txt");
        }


        private void EchoImpl(string str)
        {
            ConsoleColor fg = Console2.ForegroundColor;
            Console2.ForegroundColor = ConsoleColor.White;
            Console2.WriteLine("Echo: " + str);
            Console2.ForegroundColor = fg;
        }

        public abstract void RunMain(string argument);

    }

    class TestTerminalProperty<T> : ITerminalProperty<T>
    {
        private string id;
        private T value;

        public TestTerminalProperty(string id, T value) : base()
        {
            this.id = id;
            this.value = value;
        }

        public string Id
        {
            get
            {
                return id;
            }
        }

        public string TypeName
        {
            get
            {
                return "System.Single"; // todo
            }
        }

        public T GetDefaultValue(IMyCubeBlock block)
        {
            throw new NotImplementedException();
        }

        public T GetMaximum(IMyCubeBlock block)
        {
            throw new NotImplementedException();
        }

        public T GetMinimum(IMyCubeBlock block)
        {
            throw new NotImplementedException();
        }

        public T GetMininum(IMyCubeBlock block)
        {
            throw new NotImplementedException();
        }

        public T GetValue(IMyCubeBlock block)
        {
            return value;
        }

        public void SetValue(IMyCubeBlock block, T value)
        {
            this.value = value;
        }
    }

    class TestTerminalAction : ITerminalAction
    {
        public TestTerminalAction(string name)
        {
            throw new NotImplementedException();
        }

        public string Icon
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Id
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public StringBuilder Name
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Apply(IMyCubeBlock block)
        {

        }

        public void Apply(IMyCubeBlock block, ListReader<TerminalActionParameter> terminalActionParameters)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(IMyCubeBlock block)
        {
            throw new NotImplementedException();
        }

        public void WriteValue(IMyCubeBlock block, StringBuilder appendTo)
        {
            throw new NotImplementedException();
        }
    }

}