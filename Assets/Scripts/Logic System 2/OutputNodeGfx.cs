using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TowerDefense
{
    public class OutputNodeGfx : MonoBehaviour
    {
        private GateSlot slot;
        private SpriteRenderer sr;

        [ColorPalette, SerializeField] Color onColor;
        [ColorPalette, SerializeField] Color offColor;
        
        private void Start()
        {
            slot = GetComponent<GateSlot>();
            sr = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            sr.color = slot.state ? onColor : offColor;
        }
    }
}
