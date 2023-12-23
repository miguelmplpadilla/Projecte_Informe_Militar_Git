using System.Collections.Generic;
using System.IO;
using Resources.Scripts;
using SFB;
using UnityEngine;

public class OpenTextAsset : MonoBehaviour
{
    private DialogeController _dialogeController;
    
    private void Start()
    {
        _dialogeController = GameObject.Find("TextoNpc").GetComponent<DialogeController>();
    }

    public void startDialogueSystem()
    {
        string json = getJsonFromFileBrowser();
        Story story = JSONConverter.parseFromJson(json);

        Dictionary<string, Passage> d = new Dictionary<string, Passage>();

        foreach (var passage in story.passages)
        {
            d.Add(passage.name, passage);
        }

        _dialogeController.startDialoge(story.passages[0], d, gameObject, story, "");
    }

    private string getJsonFromFileBrowser()
    {
        string json = "";
        var paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", "json", false);

        json = File.ReadAllText(paths[0]);
        
        return json;
    }
}