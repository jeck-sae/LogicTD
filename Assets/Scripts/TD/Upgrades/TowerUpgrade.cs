using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Unity.VisualScripting;
using UnityEngine;

namespace TowerDefense
{
    [Serializable]
    public class TowerUpgrade
    {
        public int cost;
        public string targetTower = "_";
        [TextArea] public string description = "_";
        [OdinSerialize, ShowInInspector] public List<IUpgrade> upgrade = new();
    }
}