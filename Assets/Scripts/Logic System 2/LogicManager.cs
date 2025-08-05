using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TowerDefense
{
    public class LogicManager : Singleton<LogicManager>
    {
        private List<LogicInput> inputs;

        private HashSet<GateSlot> alreadyUpdatedThisThread = new ();
        private Queue<(GateSlot current, HashSet<GateSlot> updatedSlots)> queuedThreadStarts =  new ();

        [SerializeField] protected int ticksPerSecond;

        public event Action OnStatesUpdated;
        
        private void Start()
        {
            RefreshNodes();
        }

        private float nextUpdate;
        private void Update()
        {
            if (Time.time > nextUpdate)
            {
                nextUpdate = Time.time + (1f / ticksPerSecond);
                UpdateStates();
            }
        }

        public void RefreshNodes()
        {
            inputs = FindObjectsByType<LogicInput>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).ToList();
        }
        
        public void UpdateStates()
        {
            inputs.ForEach(x => queuedThreadStarts.Enqueue((x.slot, new HashSet<GateSlot>())));

            while (queuedThreadStarts.TryDequeue(out var next))
            {
                var current = next.current;
                alreadyUpdatedThisThread = next.updatedSlots;
                
                while (current)
                {
                    if (alreadyUpdatedThisThread.Contains(current))
                        break;
                    
                    current.UpdateState();
                    alreadyUpdatedThisThread.Add(current);
                    
                    for (int i = 1; i < current.outputs.Count; i++)
                        if(queuedThreadStarts.All(x => x.current != current.outputs[i]))
                            queuedThreadStarts.Enqueue((current.outputs[i],
                                new HashSet<GateSlot>(alreadyUpdatedThisThread)));
                    
                    current = current.outputs.Count == 0 ? null : current.outputs[0];
                }
            }
            OnStatesUpdated?.Invoke();
        }
        
    }
}