using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Game.Components;
using VRage.Game.ModAPI.Ingame;
using VRage.Library.Utils;
using VRageMath;

namespace SETestEnv
{
    public class TestEntity : IMyEntity
    {
        private static MyRandom IdRandom = new MyRandom(123);

        public MyEntityComponentContainer Components => throw new NotImplementedException();

        public void GenNewEntityId()
        {
            EntityId = IdRandom.NextLong() & 72057594037927935L;
        }
        public long EntityId { get; private set; } = IdRandom.NextLong() & 72057594037927935L;

        public string Name => throw new NotImplementedException();

        public string DisplayName => throw new NotImplementedException();

        public bool HasInventory => false;

        public int InventoryCount => 0;

        public bool Closed => false;

        public BoundingBoxD WorldAABB => BoundingBoxD.CreateInvalid();

        public BoundingBoxD WorldAABBHr => BoundingBoxD.CreateInvalid();

        public MatrixD WorldMatrix => throw new NotImplementedException();

        public BoundingSphereD WorldVolume => throw new NotImplementedException();

        public BoundingSphereD WorldVolumeHr => throw new NotImplementedException();

        public IMyInventory GetInventory()
        {
            throw new NotImplementedException();
        }

        public IMyInventory GetInventory(int index)
        {
            throw new NotImplementedException();
        }

        public Vector3D GetPosition()
        {
            throw new NotImplementedException();
        }

        public virtual string DisplayNameText { get; set; }
    }
}
