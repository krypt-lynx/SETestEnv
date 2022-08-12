using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SETestEnv
{
    internal interface ISimulationElement
    {
        void SimStart() { }
        void SimEnd();

        void BeforeSimStep();
        void SimStep(UpdateType updateType);
        void AfterSimStep();

        void SimSave();
    }

    internal static class SimulationElement
    {
        public static void ApplyEvent(this ISimulationElement element, SimulationEvent @event) {
            @event.Action(element);
        }
    }
}
