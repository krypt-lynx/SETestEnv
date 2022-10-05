using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Sandbox.ModAPI.Ingame;
using VRageMath;

namespace SETestEnv
{
    public class TestLargeTurretBase : TestUserControllableGun, IMyLargeTurretBase
    {
        public TestLargeTurretBase(string subtype = null) : base(subtype) { }

        public bool IsUnderControl => throw new NotImplementedException();

        public bool CanControl => throw new NotImplementedException();

        public float Range { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool IsAimed => throw new NotImplementedException();

        public bool HasTarget => throw new NotImplementedException();

        public float Elevation { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float Azimuth { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool EnableIdleRotation { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool AIEnabled => throw new NotImplementedException();

        public bool TargetMeteors { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool TargetMissiles { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool TargetSmallGrids { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool TargetLargeGrids { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool TargetCharacters { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool TargetStations { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool TargetNeutrals { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool TargetEnemies { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public MyDetectedEntityInfo GetTargetedEntity()
        {
            throw new NotImplementedException();
        }

        public string GetTargetingGroup()
        {
            throw new NotImplementedException();
        }

        public List<string> GetTargetingGroups()
        {
            throw new NotImplementedException();
        }

        public void ResetTargetingToDefault()
        {
            throw new NotImplementedException();
        }

        public void SetManualAzimuthAndElevation(float azimuth, float elevation)
        {
            throw new NotImplementedException();
        }

        public void SetTarget(Vector3D pos)
        {
            throw new NotImplementedException();
        }

        public void SetTargetingGroup(string groupSubtypeId)
        {
            throw new NotImplementedException();
        }

        public void SyncAzimuth()
        {
            throw new NotImplementedException();
        }

        public void SyncElevation()
        {
            throw new NotImplementedException();
        }

        public void SyncEnableIdleRotation()
        {
            throw new NotImplementedException();
        }

        public void TrackTarget(Vector3D pos, Vector3 velocity)
        {
            throw new NotImplementedException();
        }
    }
}
