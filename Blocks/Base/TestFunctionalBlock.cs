using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.ObjectBuilders;

namespace SETestEnv
{
    public class TestFunctionalBlock<T> : TestTerminalBlock<T>, IMyFunctionalBlock where T : MyObjectBuilder_Base
    {
        public TestFunctionalBlock(string subtype) : base(subtype)
        {
            InitProperty(new TestProp<IMyFunctionalBlock, bool>("OnOff",
                block => block.Enabled,
                (block, value) => block.Enabled = value));
        }

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
