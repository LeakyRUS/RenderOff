using MelonLoader;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RenderOff
{
    public class Mod : MelonMod
    {
        private List<Camera> _cameras = new List<Camera>();

        private int _vSyncCount { get; set; }
        private int _targetFramerate { get; set; }

        private void FocusChanged(bool isFocused)
        {
            if (isFocused)
            {
                EnableOnFocus();
            }
            else
            {
                _cameras = Camera.allCameras.Where(x => x.enabled).ToList();
                _cameras.ForEach(e => e.enabled = false);
                QualitySettings.vSyncCount = 0;
                Application.targetFrameRate = 1;
            }
        }

        private void EnableOnFocus()
        {
            _cameras.ForEach(e => e.enabled = true);
            _cameras.Clear();
            QualitySettings.vSyncCount = _vSyncCount;
            Application.targetFrameRate = _targetFramerate;
        }

        public override void OnLateInitializeMelon()
        {
            EnableOnFocus();

            Application.focusChanged += FocusChanged;
        }
    }
}
