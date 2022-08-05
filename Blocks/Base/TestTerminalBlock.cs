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
using VRage.Utils;
using System.Diagnostics;
using Sandbox.Common.ObjectBuilders;

namespace SETestEnv
{
    public abstract class TestProp : ITerminalProperty
    {
        public string Id { get; set; }

        abstract public string TypeName { get; }
    }

    public class StubProp<TBlock, T> : TestProp, ITerminalProperty<T> where TBlock : class, IMyCubeBlock
    {
        public T Value { get; set; }
        public Dictionary<TBlock, T> values = new Dictionary<TBlock, T>();

        public StubProp(string id)
        {
            this.Id = id;
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


    public abstract class TestTerminalBlock<T> : TestSlimBlock<T>, IMyTerminalBlock, IMyTextSurfaceProvider where T : MyObjectBuilder_Base
    {
        private Dictionary<string, TestProp> properties = new Dictionary<string, TestProp>();
        public void InitProperty(TestProp prop)
        {
            properties[prop.Id] = prop;
        }

        public void InitProperty<TBlock, TValue>(string id, TValue value) where TBlock : class, IMyCubeBlock
        {
            if (!this.GetType().IsAssignableFrom(typeof(TBlock)))
            {
                throw new Exception("invalid type");
            }

            if (!properties.ContainsKey(id))
            {
                properties[id] = new StubProp<TBlock, T>(id);
            }

            this.SetValue(id, value);
        }

        public TestTerminalBlock(string subtype) : base(subtype)
        {
            InitProperty(new TestProp<IMyTerminalBlock, string>("Name",
                (block) => block.CustomName,
                (block, value) => block.CustomName = value
                ));
        }

        #region IMyTerminalBlock



        public bool CheckConnectionAllowed
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string CustomInfo { get; set; } = "";
        public string CustomName { get; set; } = "";

        public string CustomNameWithFaction
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string DetailedInfo { get; set; }


        public override string DisplayNameText { get { return this.CustomName; } }

        public bool ShowOnHUD
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void GetActions(List<ITerminalAction> resultList, Func<ITerminalAction, bool> collect = null)
        {
            resultList.Clear();
            return;
            throw new NotImplementedException();
        }

        public ITerminalAction GetActionWithName(string name)
        {
            return new TestTerminalAction(name);
        }

        public void GetProperties(List<ITerminalProperty> resultList, Func<ITerminalProperty, bool> collect = null)
        {
            resultList.Clear();
            foreach (var prop in properties.Values)
            {
                if (collect?.Invoke(prop) ?? true)
                {
                    resultList.Add(prop);
                }
            }
        }

        public ITerminalProperty GetProperty(string id)
        {
            return properties[id];
        }

        public MyRelationsBetweenPlayerAndBlock GetUserRelationToOwner(long playerId)
        {
            throw new NotImplementedException();
        }

        public bool HasLocalPlayerAccess()
        {
            throw new NotImplementedException();
        }

        public bool HasPlayerAccess(long playerId)
        {
            throw new NotImplementedException();
        }

        public void SearchActionsOfName(string name, List<ITerminalAction> resultList, Func<ITerminalAction, bool> collect = null)
        {
            throw new NotImplementedException();
        }

        public void SetCustomName(StringBuilder text)
        {
            throw new NotImplementedException();
        }

        public void SetCustomName(string text)
        {
            throw new NotImplementedException();
        }

  
        #endregion

        #region IMySlimBlock
    
   
        public string CustomData { get; set; }

        bool IMyTerminalBlock.ShowOnHUD
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool ShowInTerminal
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool ShowInToolbarConfig
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool ShowInInventory
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool IsSameConstructAs(IMyTerminalBlock other)
        {
            throw new NotImplementedException();
        }

        public bool HasPlayerAccess(long playerId, MyRelationsBetweenPlayerAndBlock defaultNoUser = MyRelationsBetweenPlayerAndBlock.NoOwnership)
        {
            throw new NotImplementedException();
        }

        #endregion


        #region  IMyTextSurfaceProvider

        public TestTextSurfaceProvider surfaceProvider = new TestTextSurfaceProvider();

        public bool UseGenericLcd => surfaceProvider.UseGenericLcd;

        public int SurfaceCount => surfaceProvider.SurfaceCount;

        public IMyTextSurface GetSurface(int index) => surfaceProvider.GetSurface(index);

        #endregion
    }
}