using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class EffectDisplayer : MonoBehaviour
    {
        EffectHandler effectHandler;

        List<Effect> activeEffects = new List<Effect>();

        [SerializeField] List<SpriteRenderer> renderers;

        private void Awake()
        {
            effectHandler = GetComponentInParent<EffectHandler>();
            effectHandler.OnEffectAdded += NewEffect;
            effectHandler.OnEffectRemoved += RemovedEffect;
            UpdateGFX();
        }

        protected void NewEffect(Effect effect)
        {
            activeEffects.Add(effect);
            UpdateGFX();
        }

        protected void RemovedEffect(Effect effect) 
        { 
            activeEffects.Remove(effect);
            UpdateGFX();
        }

        protected void UpdateGFX()
        {
            renderers.ForEach(x => x.gameObject.SetActive(false));

            int j = 0;
            for (int i = 0; i < activeEffects.Count; i++) 
            {
                if (EffectDisplaySettings.HasInfo(activeEffects[i].Type))
                    SetNextEffectColor(EffectDisplaySettings.GetInfo(activeEffects[i].Type).color);
            }

            void SetNextEffectColor(Color color)
            {
                renderers[j].gameObject.SetActive(true);
                renderers[j].color = color;
                j++;
            }
        }
    }
}