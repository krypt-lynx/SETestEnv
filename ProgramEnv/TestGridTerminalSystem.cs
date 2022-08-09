using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Game.ModAPI.Ingame;

namespace SETestEnv
{
    public class TestGridTerminalSystem : IMyGridTerminalSystem
    {
        public TestCubeGrid CubeGrid;
        public Dictionary<string, TestBlockGroup> Groups = new Dictionary<string, TestBlockGroup>();

        public TestProgrammableBlock ownerBlock = null;

        public TestGridTerminalSystem(TestProgrammableBlock owner, TestCubeGrid cubeGrid) : base()
        {
            CubeGrid = cubeGrid;
            this.ownerBlock = owner;            
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
            return Groups.ContainsKey(name) ? Groups[name] : null;
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

}
