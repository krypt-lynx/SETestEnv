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
    public abstract class TestTerminalBlock : TestSlimBlock, IMyTerminalBlock, IMyTextSurfaceProvider, IEventPipeline
    {
        public TestTerminalBlock(string subtype = null) : base(subtype)
        {
            InitProperty(new TestProp<IMyTerminalBlock, string>("Name",
                (block) => block.CustomName,
                (block, value) => block.CustomName = value
                ));
        }


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
                properties[id] = new DummyProp<TBlock, TValue>(id);
            } 
            else
            {
                Console.WriteLine($"trying to reinit property {id} of {this.GetType()} with value {value}, property is set to this value instead");
                //((ITerminalProperty<TValue>)properties[id]).SetValue(this, value);
            }

            this.SetValue(id, value);
        }

        private Dictionary<string, TestAction> actions = new Dictionary<string, TestAction>();

        public void InitAction<TBlock>(TestAction<TBlock> action) where TBlock : class, IMyCubeBlock
        {
            if (!actions.ContainsKey(action.Id))
            {
                actions[action.Id] = action;
            }
            else
            {
                Console.WriteLine($"reinitializing action {action.Id} of {this.GetType()}");
            }

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
            foreach (var action in actions.Values)
            {
                if (collect?.Invoke(action) ?? true)
                {
                    resultList.Add(action);
                }
            }
        }

        /// <summary>
        /// Returns terminal action with id
        /// </summary>
        /// <param name="name">id</param>
        /// <returns>terminal action</returns>
        public ITerminalAction GetActionWithName(string name)
        {
            return actions[name];
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

        #region IEventPipeline

        void IEventPipeline.Broadcast(SimulationEvent @event) => Broadcast(@event);
        internal virtual void Broadcast(SimulationEvent @event)
        {
            ((IEventPipeline)surfaceProvider).Broadcast(@event);
        }

        #endregion
    }
}