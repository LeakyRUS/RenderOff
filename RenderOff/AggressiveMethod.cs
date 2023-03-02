using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RenderOff
{
    public class AggressiveMethod : CameraQualityMethod
    {
        private List<Camera> _cameras = new List<Camera>();

        protected override int TargetFrameRate => 1;

        protected override void Optimize()
        {
            _cameras = Camera.allCameras.Where(x => x.enabled).ToList();
            _cameras.ForEach(e => e.enabled = false);
        }

        protected override void Restore()
        {
            _cameras.ForEach(e => e.enabled = true);
            _cameras.Clear();
        }
    }
}
