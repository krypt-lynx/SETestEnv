using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Game.ModAPI.Ingame;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace SETestEnv
{
    public class TestSlimBlock : TestCubeBlock, IMySlimBlock
    {
        public TestSlimBlock(string subtype = null) : base(subtype) { }

        public float AccumulatedDamage => throw new NotImplementedException();

        public float BuildLevelRatio => throw new NotImplementedException();

        public float DamageRatio => 2f - BuildIntegrity / MaxIntegrity;

        public IMyCubeBlock FatBlock => this;

        public bool HasDeformation => false;

        public bool IsDestroyed => throw new NotImplementedException();

        public bool IsFullIntegrity => BuildIntegrity - CurrentDamage == MaxIntegrity;

        public bool IsFullyDismounted => throw new NotImplementedException();

        public float MaxDeformation => 0;

        public bool ShowParts => throw new NotImplementedException();

        public bool StockpileAllocated => throw new NotImplementedException();

        public bool StockpileEmpty => throw new NotImplementedException();

        public Vector3 ColorMaskHSV => throw new NotImplementedException();

        public MyStringHash SkinSubtypeId => throw new NotImplementedException();

        public float BuildIntegrity { get; set; }

        public float CurrentDamage { get; set; }

        public float MaxIntegrity { get; set; }

        public override bool IsFunctional => (BuildIntegrity - CurrentDamage) / MaxIntegrity > 0.5;


        public void GetMissingComponents(Dictionary<string, int> addToDictionary)
        {
            throw new NotImplementedException();
        }
    }
}
