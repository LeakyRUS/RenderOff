using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RenderOff
{
    public class NonBuggyMethod : IOffMethod
    {
        private List<(Camera, Rect)> _camerasAndRenderRects = new List<(Camera, Rect)>();

        private int _vSyncCount { get; set; }
        private int _targetFramerate { get; set; }

        public void FocusChanged(bool isFocused)
        {
            if (isFocused)
            {
                _camerasAndRenderRects.ForEach(x => x.Item1.pixelRect = x.Item2);
                _camerasAndRenderRects.Clear();
                QualitySettings.vSyncCount = _vSyncCount;
                Application.targetFrameRate = _targetFramerate;
            }
            else
            {
                _vSyncCount = QualitySettings.vSyncCount;
                _targetFramerate = Application.targetFrameRate;
                _camerasAndRenderRects = Camera.allCameras.Where(c => c.enabled).Select(x => (x, x.pixelRect)).ToList();
                _camerasAndRenderRects.ForEach(x => x.Item1.pixelRect = new Rect(x.Item2.x, x.Item2.y, 1.0f, 1.0f));
                QualitySettings.vSyncCount = 0;
                Application.targetFrameRate = 5;
            }
        }
    }
}
