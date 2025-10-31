using System.Collections;
using UnityEngine;

namespace Resources.Scripts.NPCs
{
    public class NPCGameEventsTest : GameEventBase
    {
        public IEnumerator GameEvent1()
        {
            Debug.Log("Game Event 1 Triggered");
            yield return new WaitForSeconds(1);
        }
        
        public IEnumerator GameEvent2()
        {
            Debug.Log("Game Event 1 Triggered");
            yield return new WaitForSeconds(1);
        }
    }
}