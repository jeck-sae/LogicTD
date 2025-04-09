using System;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class InteractableButton : Interactable2D
    {
        Action callback;
        public void Initialize(Action callback)
        {
            this.callback = callback;
        }

        protected override void OnCursorSelectStart()
        {
            callback?.Invoke();
        }
    }
}
