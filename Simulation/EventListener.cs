using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SETestEnv
{
    internal class EventListener : ISimulationElement
    {
#pragma warning disable CS0649 
        public Action onAfterSimStep;
        public Action onBeforeSimStep;
        public Action onSimEnd;
        public Action onSimSave;
        public Action onSimStart;
        public Action<UpdateType> onSimStep;
        public Func<bool> onIsPointOfInterest;
#pragma warning restore CS0649

        public void AfterSimStep() => onAfterSimStep?.Invoke();
        public void BeforeSimStep() => onBeforeSimStep?.Invoke();
        public void SimEnd() => onSimEnd?.Invoke();
        public void SimSave() => onSimSave?.Invoke();
        public void SimStart() => onSimStart?.Invoke();
        public void SimStep(UpdateType updateType) => onSimStep?.Invoke(updateType);
        public bool IsPointOfInterest() => onIsPointOfInterest?.Invoke() ?? false;
    }
}
