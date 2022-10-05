using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI.Ingame;
using VRageMath;

namespace SETestEnv
{
    public class TestCubeGrid : IMyCubeGrid, IEventPipeline
    {
        private List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>();
        public IReadOnlyList<IMyTerminalBlock> Blocks
        {
            get
            {
                return blocks;
            }
        }

        private Dictionary<string, TestBlockGroup> groups = new Dictionary<string, TestBlockGroup>();
        public IReadOnlyDictionary<string, TestBlockGroup> Groups
        {
            get
            {
                return groups;
            }
        }

        public void AddGroup(TestBlockGroup group)
        {
            groups[group.Name] = group;
        }

        public void AddBlock(TestTerminalBlock block)
        {
            blocks.Add(block);
            block.OwnerGrid = this;
        }

        public int IndexOf(IMyTerminalBlock block)
        {
            return blocks.IndexOf(block);
        }

        public void RemoveBlock(IMyTerminalBlock block)
        {
            ((TestTerminalBlock)block).OwnerGrid = null;
            blocks.Remove(block);
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
                return 0; // todo:
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
                return MyCubeSize.Large; // todo:
                // throw new NotImplementedException();
            }
        }

        public bool IsStatic
        {
            get
            {
                return false; // todo:
            }
        }

        public Vector3I Max
        {
            get
            {
                var x = Blocks.Aggregate(int.MinValue, (acc, b) => Math.Max(acc, b.Position.X));
                var y = Blocks.Aggregate(int.MinValue, (acc, b) => Math.Max(acc, b.Position.Y));
                var z = Blocks.Aggregate(int.MinValue, (acc, b) => Math.Max(acc, b.Position.Z));
                return new Vector3I(x, y, z);
            }
        }

        public Vector3I Min
        {
            get
            {
                var x = Blocks.Aggregate(int.MaxValue, (acc, b) => Math.Min(acc, b.Position.X));
                var y = Blocks.Aggregate(int.MaxValue, (acc, b) => Math.Min(acc, b.Position.Y));
                var z = Blocks.Aggregate(int.MaxValue, (acc, b) => Math.Min(acc, b.Position.Z));
                return new Vector3I(x, y, z);
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

        public BoundingSphereD WorldVolume => new BoundingSphereD(Vector3D.Zero, 10);

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
            return GetCubeBlock(pos) != null;
        }

        public IMySlimBlock GetCubeBlock(Vector3I pos)
        {
            if (pos.X < 0)
                return null;
            if (pos.X >= Blocks.Count)
                return null;
            return Blocks[pos.X] as IMySlimBlock;
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

        void IEventPipeline.Broadcast(SimulationEvent @event) => Blocks.BroadcastEvent(@event);
        
    }

}
