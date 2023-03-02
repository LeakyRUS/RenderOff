using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RenderOff
{
    public class NonBuggyMethod : CameraQualityMethod
    {
        private List<(Camera, Rect)> _camerasAndRenderRects = new List<(Camera, Rect)>();

        protected override int TargetFrameRate => 5;

        protected override void Optimize()
        {
            _camerasAndRenderRects = Camera.allCameras.Where(c => c.enabled).Select(x => (x, x.pixelRect)).ToList();
            _camerasAndRenderRects.ForEach(x => x.Item1.pixelRect = new Rect(x.Item2.x, x.Item2.y, 1.0f, 1.0f));
        }

        protected override void Restore()
        {
            _camerasAndRenderRects.ForEach(x => x.Item1.pixelRect = x.Item2);
            _camerasAndRenderRects.Clear();
        }
    }
}
