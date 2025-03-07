using Sirenix.OdinInspector;
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