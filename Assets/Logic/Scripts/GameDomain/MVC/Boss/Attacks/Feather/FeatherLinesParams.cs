using System;

namespace Logic.Scripts.GameDomain.MVC.Boss.Attacks.Feather
{
    public enum FeatherAxisMode { X, Z, XZ, Diagonal }

    [Serializable]
    public struct FeatherLinesParams
    {
        public int featherCount;
        public FeatherAxisMode axisMode;
        public float width;
        public float margin;
        public float forceBase;
        public float forcePerMeter;
        public float forcePerDebuff;
    }
}


