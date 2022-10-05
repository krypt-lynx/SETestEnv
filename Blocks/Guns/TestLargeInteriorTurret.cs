using Sandbox.Common.ObjectBuilders;
using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SETestEnv
{
    
    public class TestLargeInteriorTurret : TestLargeTurretBase, IMyLargeInteriorTurret
    {
        public TestLargeInteriorTurret(string subtype = null) : base(subtype) { }

        public override Type GetObjectBuilderType()
        {
            return typeof(MyObjectBuilder_InteriorTurret);
        }
    }
}
