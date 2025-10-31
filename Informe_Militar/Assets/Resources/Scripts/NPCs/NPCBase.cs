using System.Collections;
using UnityEngine;

namespace Resources.Scripts.NPCs
{
    [RequireComponent(typeof(GameEventBase))]
    public class NPCBase : InteractBaseController
    {
        public DialogueCreator dialogue;
        public SpeakerData npcData;
        public GameEventBase gameEvent;
        
        protected override IEnumerator Inter()
        {
            DialogeController.instance.SetData(dialogue, npcData, gameEvent);
            yield return null;
        }
    }
}