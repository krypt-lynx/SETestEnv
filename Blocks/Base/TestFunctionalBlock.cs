using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Game;
using VRage.ObjectBuilders;

namespace SETestEnv
{
    public class TestFunctionalBlock : TestTerminalBlock, IMyFunctionalBlock
    {
        public TestFunctionalBlock(string subtype = null) : base(subtype)
        {
            InitProperty(new TestProp<IMyFunctionalBlock, bool>("OnOff",
                block => block.Enabled,
                (block, value) => block.Enabled = value));

            InitAction(new TestAction<IMyFunctionalBlock>("OnOff", "switch", null, (block, args) => block.Enabled = !block.Enabled));
            InitAction(new TestAction<IMyFunctionalBlock>("OnOff_On", "on", null, (block, args) => block.Enabled = true));
            InitAction(new TestAction<IMyFunctionalBlock>("OnOff_Off", "off", null, (block, args) => block.Enabled = false));
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
