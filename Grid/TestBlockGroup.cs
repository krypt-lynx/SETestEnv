using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SETestEnv
{
    public class TestBlockGroup : IMyBlockGroup
    {
        public string Name { get; set; }
        List<IMyTerminalBlock> Blocks { get; set; } = new List<IMyTerminalBlock>();

        public TestBlockGroup(string name)
        {
            Name = name;
        }

        public void AddBlock(IMyTerminalBlock block)
        {
            Blocks.Add(block);
        }

        public void GetBlocks(List<IMyTerminalBlock> blocks, Func<IMyTerminalBlock, bool> collect = null)
        {
            blocks.Clear();
            foreach (var block in Blocks)
            {
                if (collect?.Invoke(block) ?? true)
                {
                    blocks.Add(block);
                }
            }
        }

        public void GetBlocksOfType<T>(List<T> blocks, Func<T, bool> collect = null) where T : class
        {
            blocks.Clear();
            foreach (var block in Blocks)
            {
                var tblock = block as T;
                if (tblock != null &&
                    (collect?.Invoke(tblock) ?? true))
                {
                    blocks.Add(tblock);
                }
            }
        }

        public void GetBlocksOfType<T>(List<IMyTerminalBlock> blocks, Func<IMyTerminalBlock, bool> collect = null) where T : class
        {
            blocks.Clear();
            foreach (var block in Blocks)
            {
                if (block is T &&
                    (collect?.Invoke(block) ?? true))
                {
                    blocks.Add(block);
                }
            }
        }
    }
}
