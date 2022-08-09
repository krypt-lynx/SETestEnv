using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SETestEnv
{
    interface ISimulationElement
    {
        void SimStart();
        void SimEnd();

        void BeforeSimStep();
        void SimStep();
        void AfterSimStep();
    }
}
