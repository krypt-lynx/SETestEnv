using Sandbox.Common.ObjectBuilders;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SETestEnv
{
    public class TestUserControllableGun : TestFunctionalBlock<MyObjectBuilder_UserControllableGun>, IMyUserControllableGun
    {
        public TestUserControllableGun(string subtype = "TestUserControllableGun") : base(subtype) { }

        public bool IsShooting => false;

        public bool Shoot { get; set; } = false;

        public void ShootOnce()
        {
            throw new NotImplementedException();
        }
    }
}
