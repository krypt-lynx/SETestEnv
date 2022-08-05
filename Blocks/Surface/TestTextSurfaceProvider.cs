using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SETestEnv
{
    public class TestTextSurfaceProvider : IMyTextSurfaceProvider
    {
        List<IMyTextSurface> surfaces = new List<IMyTextSurface>();

        public void AddSurface(IMyTextSurface surface)
        {
            surfaces.Add(surface);
        }       

        public bool UseGenericLcd => false;

        public int SurfaceCount => surfaces.Count;

        public IMyTextSurface GetSurface(int index) => surfaces[index];
    }
}
