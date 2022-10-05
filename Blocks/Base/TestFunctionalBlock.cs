using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using VRage.Game;
using VRage.ObjectBuilders;

namespace SETestEnv
{
    public class TestFunctionalBlock : TestTerminalBlock, IMyFunctionalBlock
    {

        public TestFunctionalBlock(string subtype = null) : base(subtype)
        {
            var prop = new TestProp<IMyFunctionalBlock, bool>("OnOff",
                block => block.Enabled,
                (block, value) => block.Enabled = value);
            InitProperty(prop);
            CreateActionsForProperty(prop);
        }

        //public override Type DefinitionType => typeof(MyObjectBuilder_FunctionalBlock);

        public bool Enabled {
            get; 
            set;
        } = true;

        public void RequestEnable(bool enable)
        {
            Enabled = enable;
        }
    }
}
