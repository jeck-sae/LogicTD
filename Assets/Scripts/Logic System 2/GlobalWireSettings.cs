using Sirenix.OdinInspector;
using UnityEngine;

namespace TowerDefense
{
    public class GlobalWireSettings : Singleton<GlobalWireSettings>
    {
        public float width;
        public float distanceFromWire;
        public float verticalConnectionArea;
        [ColorPalette] public Color onColor;
        [ColorPalette] public Color offColor;
    }
}