using UnityEngine;

namespace RenderOff
{
    public abstract class CameraQualityMethod : IOffMethod
    {
        private int _vSyncCount { get; set; }
        private int _targetFramerate { get; set; }

        protected abstract void Restore();
        protected abstract void Optimize();

        protected abstract int TargetFrameRate { get; }

        public void FocusChanged(bool isFocused)
        {
            if (isFocused)
            {
                Restore();
                QualitySettings.vSyncCount = _vSyncCount;
                Application.targetFrameRate = _targetFramerate;
            }
            else
            {
                _vSyncCount = QualitySettings.vSyncCount;
                _targetFramerate = Application.targetFrameRate;
                Optimize();
                QualitySettings.vSyncCount = 0;
                Application.targetFrameRate = TargetFrameRate;
            }
        }
    }
}
