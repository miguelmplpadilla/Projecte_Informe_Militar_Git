using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Resources.Scripts.NPCs
{
    public abstract class GameEventBase : MonoBehaviour
    {
        public UnityEvent primaryEnd;
        public UnityEvent secondaryEnd;
        
        private void Start()
        {
            EventBus<StartTriggerEvent>.Register(new EventBinding<StartTriggerEvent>(TriggerEvent, gameObject));
        }
        
        private void OnDestroy()
        {
            EventBus<StartTriggerEvent>.Deregister(new EventBinding<StartTriggerEvent>(TriggerEvent, gameObject));
        }

        private void TriggerEvent(StartTriggerEvent s)
        {
            if (!s.obj.Equals(gameObject)) return;
            
            StartCoroutine(StartEvent(s.index));
        }

        private IEnumerator StartEvent(int index)
        {
            yield return StartCoroutine("GameEvent" + index);
            EventBus<OnEndGameEvent>.Raise(new OnEndGameEvent());
        }
    }
    
    public class StartTriggerEvent : IEvent
    {
        public GameObject obj;
        public int index;
    }
}