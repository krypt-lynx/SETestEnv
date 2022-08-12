using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private UpdateFrequency updateFrequency = UpdateFrequency.None;

        public void InitNewRun()
        {
            simInstructionCount = 0;
            updateFrequency = updateFrequency & ~UpdateFrequency.Once;
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

        public TimeSpan TimeSinceLastRun
        {
            get
            {
                return new TimeSpan(0, 0, 0, 1);
            }
        }

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
            get
            {
                return updateFrequency;
            }

            set
            {
                updateFrequency = value;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("new UpdateFrequency value: {0}", value);
            }
        }
    }

}
