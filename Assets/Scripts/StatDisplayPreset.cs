using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable, CreateAssetMenu(fileName = "StatDisplayOptions", menuName = "StatDisplayOptions")]
public class StatDisplayPreset : ScriptableObject
{
    private static StatDisplayPreset instance;
    public static StatDisplayPreset Instance
    {
        get
        {
            if (instance == null)
                instance = (StatDisplayPreset)Resources.Load("StatDisplayOptions");
            return instance;
        }
    }

    public static bool HasInfo(string id) => Instance.stats.Any(x => x.id == id);
    public static StatDisplayInfo GetInfo(string id) => Instance.stats.FirstOrDefault(x => x.id == id);

    public List<StatDisplayInfo> stats;

    [Serializable]
    public class StatDisplayInfo
    {
        public string id;
        public string name;
        public Color color;
    }
}
