using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SETestEnv
{
    public class TestTextSurfaceProvider : IMyTextSurfaceProvider, ISimulationElement
    {
        List<IMyTextSurface> surfaces = new List<IMyTextSurface>();

        public void AddSurface(TestTextSurface surface)
        {
            surfaces.Add(surface);
        }       

        public bool UseGenericLcd => false;

        public int SurfaceCount => surfaces.Count;

        public IMyTextSurface GetSurface(int index) => surfaces[index];

        public void SimStart()
        {
            foreach (var surface in surfaces)
            {
                (surface as TestTextSurface).SimStart();
            }
        }

        public void SimEnd()
        {
            foreach (var surface in surfaces)
            {
                (surface as TestTextSurface).SimEnd();
            }
        }

        public void BeforeSimStep()
        {
            foreach (var surface in surfaces)
            {
                (surface as TestTextSurface).BeforeSimStep();
            }
        }

        public void SimStep()
        {
            foreach (var surface in surfaces)
            {
                (surface as TestTextSurface).SimStep();
            }
        }

        public void AfterSimStep()
        {
            foreach (var surface in surfaces)
            {
                (surface as TestTextSurface).AfterSimStep();
            }
        }

        public void SimSave()
        {
            foreach (var surface in surfaces)
            {
                (surface as TestTextSurface).SimSave();
            }
        }
    }
}
