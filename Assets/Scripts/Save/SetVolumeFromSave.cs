using System;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class SetVolumeFromSave : MonoBehaviour
    {
        public float multiplier = 1.0f;
        private void OnEnable()
        {
            SaveManager.OnApplySettings += ApplyVolume;
        }

        private void OnDisable()
        {
            SaveManager.OnApplySettings -= ApplyVolume;
        }

        void ApplyVolume()
        {
            AudioListener.volume = SaveManager.Instance.data.volume * multiplier;
        }
    }
}
