using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SETestEnv
{
    internal class SimulationEvent
    {
        public string Name;
        public Action<ISimulationElement> Action;

        protected SimulationEvent(string name)
        {
            Name = name;
        }

        public SimulationEvent(string name, Action<ISimulationElement> action) : this(name)
        {
            Action = action;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    internal class SimulationEvent<T> : SimulationEvent
    {
        public T Result { get; private set; }
        public SimulationEvent(string name, Func<ISimulationElement, T> func, T seed, Func<T, T, T> aggregator) : base(name)
        {
            Result = seed;
            Action = x => Result = aggregator(Result, func(x));
        }

        public override string ToString()
        {
            return Name;
        }
    }

    internal interface IEventPipeline
    {
        void Broadcast(SimulationEvent @event);
    }

    static class EventPipeline
    {

        public static T Broadcast<T>(this IEventPipeline pipeline, SimulationEvent<T> @event)
        {
            pipeline.Broadcast(@event);
            return @event.Result;
        }

        public static void BroadcastEvent(this IEnumerable pipelines, SimulationEvent @event)
        {
            foreach (var obj in pipelines)
            {
                (obj as IEventPipeline)?.Broadcast(@event);
            }
        } 
    }
}
