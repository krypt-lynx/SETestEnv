using Sandbox.Common.ObjectBuilders;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using VRage.Game;
using VRage.Game.ModAPI.Ingame;
using VRage.ObjectBuilders;
using VRageMath;

namespace SETestEnv
{
    public class TestCubeBlock : TestEntity, IMyCubeBlock
    {
        public TestCubeBlock(string subtypeName = null)
        {
            var type = GetObjectBuilderTypeInternal();

            BlockDefinition = new SerializableDefinitionId(type, subtypeName);
        }

        static Dictionary<Type, Type> builderTypes = new Dictionary<Type, Type>();
        private Type GetObjectBuilderTypeInternal()
        {
            var blockType = GetType();
            if (!builderTypes.TryGetValue(blockType, out var builderType))
            {
                builderType = GetObjectBuilderType();
                if (builderType == null)
                {
                    throw new Exception($"Definition type is null for {GetType().FullName}");
                }
                builderTypes[blockType] = builderType;
            }
            return builderType;
        }

        protected Type ResolveObjectBuilderType()
        {
            // general rule Keen's naming rule for object builders:
            // For IMy<Something> interfaces and My<Something> block you have 
            // MyObjectBuilder_<Something> builder in Sandbox.Common.ObjectBuilders namespace

            // But sometimes KeenSWH do not follow its own rules:
            // MyLargeInteriorTurret - MyObjectBuilder_InteriorTurret
            // IMyProgrammableBlock - MyObjectBuilder_MyProgrammableBlock

            // In such case you can override GetObjectBuilderType method and return proper MyObjectBuilder_Base type directly

            // ... or you don't used "Test" prefix for your class

            Type obType = null;

            try
            {
                if (GetType().Name.StartsWith("Test"))
                {
                    var objectBuilders = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.GetName().Name == "SpaceEngineers.ObjectBuilders").FirstOrDefault();

                    var obTypeName = "Sandbox.Common.ObjectBuilders.MyObjectBuilder_" + GetType().Name.Substring(4);

                    obType = objectBuilders.GetType(obTypeName);
                }
            }
            catch { }

            return obType;
        }

        public virtual Type GetObjectBuilderType() => ResolveObjectBuilderType();

        public TestCubeGrid OwnerGrid { get; set; }

        public SerializableDefinitionId BlockDefinition { get; private set; }

        public IMyCubeGrid CubeGrid => OwnerGrid;

        public virtual bool IsFunctional => true;

        public virtual bool IsWorking => IsFunctional;

        public int NumberInGrid => OwnerGrid.IndexOf((IMyTerminalBlock)this);

        public MyBlockOrientation Orientation => MyBlockOrientation.Identity;

        public long OwnerId => 0; // Unowned

        public Vector3I Position
        {
            get
            {
                if (OwnerGrid == null)
                {
                    return Vector3I.Zero;
                }
                return new Vector3I(OwnerGrid.IndexOf((IMyTerminalBlock)this), 0, 0);
            }
        }


        public string DefinitionDisplayNameText => GetType().Name;

        public float DisassembleRatio => throw new NotImplementedException();

        public bool IsBeingHacked => throw new NotImplementedException();

        public Vector3I Max => Position;

        public float Mass => throw new NotImplementedException();

        public Vector3I Min => Position;

        public string GetOwnerFactionTag()
        {
            throw new NotImplementedException();
        }

        public MyRelationsBetweenPlayerAndBlock GetPlayerRelationToOwner()
        {
            throw new NotImplementedException();
        }

        public MyRelationsBetweenPlayerAndBlock GetUserRelationToOwner(long playerId, MyRelationsBetweenPlayerAndBlock defaultNoUser = MyRelationsBetweenPlayerAndBlock.NoOwnership)
        {
            throw new NotImplementedException();
        }

        public void UpdateIsWorking()
        {
            throw new NotImplementedException();
        }

        public void UpdateVisual()
        {
            throw new NotImplementedException();
        }
    }
}
