using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class TestGateSlot : MonoBehaviour, ITowerSlot
    {
        [SerializeField] private Tower tower;
        
        public Tower Tower => tower;

        public bool CanPlace()
        {
            return !tower;
        }

        public void PlaceTower(Tower t)
        {
            if (!CanPlace())
                return;
            tower = t;
            t.gameObject.SetActive(true);
            t.gameObject.transform.position = transform.position;
            t.gameObject.transform.parent = transform;
            t.SetSlot(this);
            name = $"Gate Node [" + t.towerName + "]";
        }

        public void RemoveTower()
        {
            tower = null;
            name = "Empty Gate Node";
        }
    }
}
