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
        public TestProgrammableBlock(string subtype = null) : base(subtype) {
            surfaceProvider.AddSurface(new TestTextSurface(new Vector2(512, 512), new Vector2(512, 320), "Large Display", "CockpitScreen_02", this));
            surfaceProvider.AddSurface(new TestTextSurface(new Vector2(512, 256), new Vector2(512, 204.8f), "Keyboard", "CockpitScreen_01", this));
        }

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
                    new TestIntergridCommunicationSystem(EntityId));
                programLayer.InitializeProgram();
            }
        }

        public void SimStep(UpdateType updateType) { }

        public void SimSave() { }

        public void SimEnd() { }

        public void BeforeSimStep() { }

        public void AfterSimStep() { }

        internal override void Broadcast(SimulationEvent @event)
        {
            base.Broadcast(@event);
            this.ApplyEvent(@event);
            programLayer?.ApplyEvent(@event);
        }

        public bool IsPointOfInterest() => true;
    }

}
