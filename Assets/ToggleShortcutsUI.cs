using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class ToggleShortcutsUI : MonoBehaviour
    {
        public void Toggle()
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}
