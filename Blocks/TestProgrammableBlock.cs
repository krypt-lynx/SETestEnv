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
using Sandbox.Common.ObjectBuilders;

namespace SETestEnv
{
    public class TestProgrammableBlock : TestFunctionalBlock, IMyProgrammableBlock, ISimulationElement
    {
        public TestProgrammableBlock(string subtype = null) : base(subtype) { }

        public override Type GetObjectBuilderType()
        {
            return typeof(MyObjectBuilder_MyProgrammableBlock);
        }

        public bool IsRunning { get; set; }
        public string TerminalRunArgument { get; set; }

        public bool TryRun(string argument)
        {
            if (programLayer != null)
            {
                programLayer.RunMain(argument, UpdateType.Script);
                return true;
            }
            else
            {
                return false;
            }
        }

        public DeferredProgram Program;

        ProgramLayer programLayer = null;

        public void SimStart()
        {
            if (Program != null)
            {
                programLayer = new ProgramLayer(Program,
                    this,
                    Universe.Echo,
                    new TestGridTerminalSystem(this, (TestCubeGrid)CubeGrid),
                    new TestIntergridCommunicationSystem(this.EntityId));
                programLayer.InitializeProgram();
            }
        }

        public void SimStep(UpdateType updateType) {
            if (programLayer != null)
            {
                string arg = "";
                if (InputPipeline.HasValue())
                {
                    arg = InputPipeline.Pop();
                }
                programLayer.RunMain(arg);
            }
        }

        public void SimSave()
        {
            programLayer?.Save();
        }

        public void SimEnd() { }

        public void BeforeSimStep() { }

        public void AfterSimStep() { }

        internal override void Broadcast(SimulationEvent @event)
        {
            base.Broadcast(@event);
            this.ApplyEvent(@event);
        }
    }

}
