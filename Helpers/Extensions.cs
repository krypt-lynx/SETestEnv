using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SETestEnv
{
    public static class EnumContains
    {
        public static bool Contains(this UpdateFrequency frequency, UpdateFrequency value)
        {
            return (frequency & value) != 0;
        }
    }
}
