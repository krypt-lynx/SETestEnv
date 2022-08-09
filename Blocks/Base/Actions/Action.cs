using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Collections;
using VRage.Game.ModAPI.Ingame;

namespace SETestEnv
{
    public abstract class TestAction : ITerminalAction
    {
        public string Id { get; protected set; }
        public string Icon { get; protected set; }
        public StringBuilder Name { get; protected set; }

        protected Action action;

        public abstract void Apply(IMyCubeBlock block);
        public abstract void Apply(IMyCubeBlock block, ListReader<TerminalActionParameter> terminalActionParameters);


        public abstract bool IsEnabled(IMyCubeBlock block);

        public abstract void WriteValue(IMyCubeBlock block, StringBuilder appendTo);
    }

    public class TestAction<TBlock> : TestAction where TBlock : class, IMyCubeBlock
    {
        Action<TBlock, ListReader<TerminalActionParameter>> _action;

        public TestAction(string id, string name, string icon, Action<TBlock, ListReader<TerminalActionParameter>> action) : this(id, new StringBuilder(name), icon, action) { }

        public TestAction(string id, StringBuilder name, string icon, Action<TBlock, ListReader<TerminalActionParameter>> action)
        {
            Id = id;
            Name = name;
            Icon = icon;
            _action = action;
        }

        public override void Apply(IMyCubeBlock block)
        {
            Apply(block, null);
        }

        public override void Apply(IMyCubeBlock block, ListReader<TerminalActionParameter> terminalActionParameters)
        {
            var tblock = block as TBlock;
            if (tblock != null)
            {
                _action?.Invoke(tblock, terminalActionParameters);
            }
            else
            {
                throw new Exception($"block {block} not a {typeof(TBlock)}");
            }
        }

        public override bool IsEnabled(IMyCubeBlock block)
        {
            throw new NotImplementedException();
        }

        public override void WriteValue(IMyCubeBlock block, StringBuilder appendTo)
        {
            throw new NotImplementedException();
        }
    }
}
