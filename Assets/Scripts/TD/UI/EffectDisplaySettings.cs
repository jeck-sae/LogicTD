using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace TowerDefense
{
    [Serializable, CreateAssetMenu(fileName = "EffectDisplaySettings", menuName = "EffectDisplaySettings")]
    public class EffectDisplaySettings : ScriptableObject
    {
        private static EffectDisplaySettings instance;
        public static EffectDisplaySettings Instance
        {
            get
            {
                if (instance == null)
                    instance = (EffectDisplaySettings)Resources.Load("Settings/EffectDisplaySettings");
                return instance;
            }
        }


        public static bool HasInfo(EffectType id) => Instance.effectInfo.Any(x => x.type == id);
        public static EffectSettings GetInfo(EffectType id) => Instance.effectInfo.Find(x => x.type == id);

        public List<EffectSettings> effectInfo;


    }

    [Serializable]
    public class EffectSettings
    {
        public EffectType type;
        public string id;
        [ColorPalette]
        public Color color = Color.white;
    }
}
