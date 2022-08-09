using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI.Ingame;
using VRageMath;

namespace SETestEnv
{
    public class TestCubeGrid : IMyCubeGrid, ISimulationElement
    {
        private List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>();
        public List<IMyTerminalBlock> Blocks
        {
            get
            {
                return blocks;
            }
        }

        public void RegisterBlock(TestTerminalBlock block)
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



        public void SimStart()
        {
            foreach (var block in Blocks)
            {
                (block as ISimulationElement)?.SimStart();
            }
        }

        public void BeforeSimStep()
        {
            foreach (var block in Blocks)
            {
                (block as ISimulationElement)?.BeforeSimStep();
            }
        }

        public void SimStep()
        {
            foreach (var block in Blocks)
            {
                (block as ISimulationElement)?.SimStep();
            }
        }

        public void AfterSimStep()
        {
            foreach (var block in Blocks)
            {
                (block as ISimulationElement)?.AfterSimStep();
            }
        }

        public void SimEnd()
        {
            foreach (var block in Blocks)
            {
                (block as ISimulationElement)?.SimEnd();
            }
        }
    }

}
