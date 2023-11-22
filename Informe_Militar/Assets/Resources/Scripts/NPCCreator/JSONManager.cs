using System;
using System.IO;
using SFB;
using UnityEditor;
using UnityEngine;

namespace Resources.Scripts
{
    public class JSONManager : MonoBehaviour
    {
        private NPCAnimatorController _npcAnimatorController;
        
        private void Start()
        {
            _npcAnimatorController = GameObject.Find("NPC").GetComponent<NPCAnimatorController>();
        }

        public void saveJSON()
        {
            Character character = _npcAnimatorController.character;
            
            var path = EditorUtility.SaveFilePanel(
                "Guardar json del personaje",
                "",
                "Personaje" + ".json",
                "json");

            if (path.Length != 0)
            {
                string text = JsonUtility.ToJson(character);
                if (!text.Equals(""))
                    File.WriteAllText(path, text);
            }
        }
        
        public void loadJSON()
        {
            string json = "";
            var paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", "json", false);

            json = File.ReadAllText(paths[0]);

            Character character = new Character();

            JsonUtility.FromJsonOverwrite(json, character);

            _npcAnimatorController.character = character;
        }

        public static Character GetCharacterFromJSON()
        {
            string json = "";
            var paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", "json", false);

            json = File.ReadAllText(paths[0]);

            Character character = new Character();

            JsonUtility.FromJsonOverwrite(json, character);

            return character;
        }
    }
}