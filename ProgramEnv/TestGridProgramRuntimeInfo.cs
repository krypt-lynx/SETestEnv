using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SETestEnv
{
    public class TestGridProgramRuntimeInfo : IMyGridProgramRuntimeInfo
    {
        public ProgramLayer ProgramLayer;

        public TestGridProgramRuntimeInfo(ProgramLayer programLayer)
        {
            ProgramLayer = programLayer;
        }

        private int simInstructionCount = 0;

        public void InitNewRun()
        {
            simInstructionCount = 0;
        }

        public int CurrentInstructionCount
        {
            get
            {
                simInstructionCount += 10;
                return simInstructionCount;
            }
        }

        public int CurrentMethodCallCount => 0;

        public double LastRunTimeMs { get; set; }

        public int MaxInstructionCount => 50000;

        public int MaxMethodCallCount => 50000;

        public TimeSpan TimeSinceLastRun => ProgramLayer.TimeSinceLastRun;

        public int MaxCallChainDepth
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int CurrentCallChainDepth
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public UpdateFrequency UpdateFrequency
        {
            get => ProgramLayer.UpdateFrequency;
            set => ProgramLayer.UpdateFrequency = value;
        }
    }

}
