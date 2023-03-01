using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RenderOff
{
    public class AggressiveMethod : IOffMethod
    {
        private List<Camera> _cameras = new List<Camera>();

        private int _vSyncCount { get; set; }
        private int _targetFramerate { get; set; }

        public void FocusChanged(bool isFocused)
        {
            if (isFocused)
            {
                _cameras.ForEach(e => e.enabled = true);
                _cameras.Clear();
                QualitySettings.vSyncCount = _vSyncCount;
                Application.targetFrameRate = _targetFramerate;
            }
            else
            {
                _vSyncCount = QualitySettings.vSyncCount;
                _targetFramerate = Application.targetFrameRate;
                _cameras = Camera.allCameras.Where(x => x.enabled).ToList();
                _cameras.ForEach(e => e.enabled = false);
                QualitySettings.vSyncCount = 0;
                Application.targetFrameRate = 1;
            }
        }
    }
}
