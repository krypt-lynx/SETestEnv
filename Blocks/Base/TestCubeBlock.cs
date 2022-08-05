using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Game;
using VRage.Game.ModAPI.Ingame;
using VRage.ObjectBuilders;
using VRageMath;

namespace SETestEnv
{
    public class TestCubeBlock<T> : TestEntity, IMyCubeBlock where T : MyObjectBuilder_Base
    {
        public TestCubeBlock(string subtypeName)
        {
            BlockDefinition = new SerializableDefinitionId(typeof(T), subtypeName);
        }

        public TestCubeGrid OwnerGrid { get; set; }

        public SerializableDefinitionId BlockDefinition { get; private set; }

        public IMyCubeGrid CubeGrid => OwnerGrid;

        public float BuildIntegrity { get; set; }

        public float CurrentDamage { get; set; }

        public float MaxIntegrity { get; set; }

        public bool IsFunctional => (BuildIntegrity - CurrentDamage) / MaxIntegrity > 0.5;

        public bool IsWorking { get; set; }

        public int NumberInGrid => OwnerGrid.Blocks.IndexOf((IMyTerminalBlock)this);

        public MyBlockOrientation Orientation => MyBlockOrientation.Identity;

        public long OwnerId => 0; // Unowned

        public Vector3I Position
        {
            get
            {
                return new Vector3I(OwnerGrid.Blocks.IndexOf((IMyTerminalBlock)this), 0, 0);
            }
        }


        public string DefinitionDisplayNameText => throw new NotImplementedException();

        public float DisassembleRatio => throw new NotImplementedException();

        public bool IsBeingHacked => throw new NotImplementedException();

        public Vector3I Max => throw new NotImplementedException();

        public float Mass => throw new NotImplementedException();

        public Vector3I Min => throw new NotImplementedException();

        public string GetOwnerFactionTag()
        {
            throw new NotImplementedException();
        }

        public MyRelationsBetweenPlayerAndBlock GetPlayerRelationToOwner()
        {
            throw new NotImplementedException();
        }

        public MyRelationsBetweenPlayerAndBlock GetUserRelationToOwner(long playerId, MyRelationsBetweenPlayerAndBlock defaultNoUser = MyRelationsBetweenPlayerAndBlock.NoOwnership)
        {
            throw new NotImplementedException();
        }

        public void UpdateIsWorking()
        {
            throw new NotImplementedException();
        }

        public void UpdateVisual()
        {
            throw new NotImplementedException();
        }
    }
}
