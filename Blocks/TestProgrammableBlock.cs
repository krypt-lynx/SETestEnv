using System;

using System.Text;
using System.Collections;
using System.Collections.Generic;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.ObjectBuilders;
using VRageMath;

namespace SETestEnv
{
    public class TestProgrammableBlock : TestFunctionalBlock<MyObjectBuilder_Battery>, IMyProgrammableBlock
    {
        public TestProgrammableBlock(string subtype = "TestProgrammableBlock") : base(subtype) { }

        public bool IsRunning { get; set; }
        public string TerminalRunArgument { get; set; }

        public bool TryRun(string argument)
        {
            throw new NotImplementedException();
        }
    }

}
