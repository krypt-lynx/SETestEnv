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
    public class TestSlimBlock<T> : TestCubeBlock<T>, IMySlimBlock where T : MyObjectBuilder_Base
    {
        public TestSlimBlock(string subtype) : base(subtype) { }

        public float AccumulatedDamage => throw new NotImplementedException();

        public float BuildLevelRatio => throw new NotImplementedException();

        public float DamageRatio => throw new NotImplementedException();

        public IMyCubeBlock FatBlock => this;

        public bool HasDeformation => throw new NotImplementedException();

        public bool IsDestroyed => throw new NotImplementedException();

        public bool IsFullIntegrity => throw new NotImplementedException();

        public bool IsFullyDismounted => throw new NotImplementedException();

        public float MaxDeformation => throw new NotImplementedException();

        public bool ShowParts => throw new NotImplementedException();

        public bool StockpileAllocated => throw new NotImplementedException();

        public bool StockpileEmpty => throw new NotImplementedException();

        public Vector3 ColorMaskHSV => throw new NotImplementedException();

        public MyStringHash SkinSubtypeId => throw new NotImplementedException();

        public void GetMissingComponents(Dictionary<string, int> addToDictionary)
        {
            throw new NotImplementedException();
        }
    }
}
