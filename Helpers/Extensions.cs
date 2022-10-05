using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SETestEnv
{
    public static class EnumContains
    {
        public static bool Contains(this UpdateFrequency frequency, UpdateFrequency value)
        {
            return (frequency & value) != 0;
        }
    }

    public static class WeakReferenceTarget
    {
        public static T GetTarget<T>(this WeakReference<T> reference) where T : class
        {
            if (reference.TryGetTarget(out var target))
            {
                return target;
            }
            return null;
        }
    }
}
