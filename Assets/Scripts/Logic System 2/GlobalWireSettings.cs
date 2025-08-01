using Sirenix.OdinInspector;
using UnityEngine;

namespace TowerDefense
{
    public class GlobalWireSettings : Singleton<GlobalWireSettings>
    {
        public float width;
        public float distanceFromWire;
        public float verticalConnectionArea;
        public Color onColor;
        public Color offColor;
    }
}