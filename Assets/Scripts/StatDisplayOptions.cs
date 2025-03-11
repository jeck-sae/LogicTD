using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace TowerDefense
{
    [Serializable, CreateAssetMenu(fileName = "StatDisplayOptions", menuName = "StatDisplayOptions")]
    public class StatDisplayOptions : ScriptableObject
    {
        private static StatDisplayOptions instance;
        public static StatDisplayOptions Instance
        {
            get
            {
                if (instance == null)
                    instance = (StatDisplayOptions)Resources.Load("Settings/StatDisplayOptions");
                return instance;
            }
        }
    
        public static bool HasInfo(string id) => Instance.stats.Any(x => x.id == id);
        public static bool ShouldShowStat(string id)
        {
            var i = Instance.stats.Find(x => x.id == id);
            return i != null && i.show;
        }
        public static StatDisplayInfo GetInfo(string id) => Instance.stats.Find(x => x.id == id);
    
        public List<StatDisplayInfo> stats;
    
        
    }
    
    [Serializable]
    public class StatDisplayInfo
    {
        public string id;
        public string name;
        [ColorPalette]
        public Color color = Color.white;
        public bool show = true;
    }
}
