using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using VRage.Game.ModAPI.Ingame;

namespace SETestEnv
{
    public class TestGridTerminalSystem : IMyGridTerminalSystem
    {
        public TestCubeGrid CubeGrid;

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
            blockGroups.AddRange(CubeGrid.Groups.Values.Where(x => collect?.Invoke(x) ?? true));
        }

        public IMyBlockGroup GetBlockGroupWithName(string name)
        {
            return CubeGrid.Groups.TryGetValue(name, out var group) ? group : null;
        }

        public void GetBlocks(List<IMyTerminalBlock> blocks)
        {
            blocks.Clear();
            blocks.AddRange(CubeGrid.Blocks);
        }

        public void GetBlocksOfType<T>(List<T> blocks, Func<T, bool> collect = null) where T : class
        {
            blocks?.Clear();
            foreach (var block in CubeGrid.Blocks)
            {
                var b = block as T;
                if (b != null &&
                    (collect?.Invoke(b) ?? true))
                {
                    blocks?.Add(b);
                }
            }
        }

        public void GetBlocksOfType<T>(List<IMyTerminalBlock> blocks, Func<IMyTerminalBlock, bool> collect = null) where T : class
        {
            blocks?.Clear();
            foreach (var block in CubeGrid.Blocks)
            {
                if ((block is T) && (collect?.Invoke(block) ?? true))
                {
                    blocks?.Add(block);
                }
            }
        }

        public IMyTerminalBlock GetBlockWithId(long id)
        {
            return CubeGrid.Blocks.FirstOrDefault(x => x.EntityId == id);
        }

        public IMyTerminalBlock GetBlockWithName(string name)
        {
            foreach (var block in CubeGrid.Blocks)
            {
                if (block.CustomName == name)
                {
                    return block;
                }
            }
            return null;
        }

        public void SearchBlocksOfName(string name, List<IMyTerminalBlock> blocks, Func<IMyTerminalBlock, bool> collect = null)
        {
            if (blocks == null)
            {
                return;
            }

            blocks.Clear();
            foreach (var block in CubeGrid.Blocks)
            {
                if (block.CustomName.Contains(name, StringComparison.OrdinalIgnoreCase) &&
                    (collect == null || collect(block)))
                {
                    blocks.Add(block);
                }
            }
        }
    }

}
