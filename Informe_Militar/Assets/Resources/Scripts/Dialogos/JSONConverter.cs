using System.IO;
using Resources.Scripts;
using UnityEngine;

public class JSONConverter : MonoBehaviour
{
    public static Story parseJson(string idDialogo)
    {
        string idioma = "ES";

        Story story = new Story();
        
        TextAsset jsonDialogo = UnityEngine.Resources.Load<TextAsset>("JSON/"+idioma+"/JSONDialogos/"+idDialogo);
        JsonUtility.FromJsonOverwrite(jsonDialogo.text, story);

        return story;
    }

    public static Story parseFromJson(string json)
    {
        Story story = new Story();

        JsonUtility.FromJsonOverwrite(json, story);

        return story;
    }

    public static string rewriteJson(Story story)
    {
        string idioma = "ES";
        
        string json = JsonUtility.ToJson(story);
        
        File.WriteAllText(Application.dataPath+"/Resources/JSON/"+idioma+"/JSONDialogos/"+story.name+".json", json);

        return json;
    }
}
