using Sandbox.ModAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Game.ModAPI.Ingame;

namespace SETestEnv
{
    public abstract class TestProp : ITerminalProperty
    {
        public string Id { get; set; }

        abstract public string TypeName { get; }
    }

    public class DummyProp<TBlock, T> : TestProp, ITerminalProperty<T> where TBlock : class, IMyCubeBlock
    {
        public T Value { get; set; }
        public Dictionary<TBlock, T> values = new Dictionary<TBlock, T>();

        public DummyProp(string id)
        {
            Id = id;
        }

        public override string TypeName => typeof(T).Name;

        public T GetValue(IMyCubeBlock block)
        {
            values.TryGetValue(block as TBlock, out var value);
            return value;
        }

        public void SetValue(IMyCubeBlock block, T value)
        {
            values[block as TBlock] = value;
        }

        public T GetDefaultValue(IMyCubeBlock block)
        {
            throw new NotImplementedException();
        }

        public T GetMininum(IMyCubeBlock block)
        {
            throw new NotImplementedException();
        }

        public T GetMinimum(IMyCubeBlock block)
        {
            throw new NotImplementedException();
        }

        public T GetMaximum(IMyCubeBlock block)
        {
            throw new NotImplementedException();
        }
    }

    public class TestProp<TBlock, T> : TestProp, ITerminalProperty<T> where TBlock : class, IMyCubeBlock
    {
        Func<TBlock, T> getter;
        Action<TBlock, T> setter;

        public TestProp(string id, Func<TBlock, T> getter, Action<TBlock, T> setter)
        {
            this.Id = id;
            this.getter = getter;
            this.setter = setter;
        }

        public override string TypeName => typeof(T).Name;

        public T GetValue(IMyCubeBlock block)
        {
            return getter(block as TBlock);
        }

        public void SetValue(IMyCubeBlock block, T value)
        {
            setter(block as TBlock, value);
        }

        public T GetDefaultValue(IMyCubeBlock block)
        {
            throw new NotImplementedException();
        }

        public T GetMininum(IMyCubeBlock block)
        {
            throw new NotImplementedException();
        }

        public T GetMinimum(IMyCubeBlock block)
        {
            throw new NotImplementedException();
        }

        public T GetMaximum(IMyCubeBlock block)
        {
            throw new NotImplementedException();
        }
    }
}
