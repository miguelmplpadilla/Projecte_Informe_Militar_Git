using System.IO;
using Resources.Scripts;
using Resources.Scripts.UI.Idiomas;
using UnityEngine;

public class JSONConverter : MonoBehaviour
{
    public static Story parseJson(string idDialogo)
    {
        string idioma = IdiomaController.getLanguage();

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
        string idioma = IdiomaController.getLanguage();
        
        string json = JsonUtility.ToJson(story);
        
        File.WriteAllText(Application.dataPath+"/Resources/JSON/"+idioma+"/JSONDialogos/"+story.name+".json", json);

        return json;
    }

    public static string getText(string textType, string idText)
    {
        string idioma = IdiomaController.getLanguage();

        Root texts = new Root();
        
        TextAsset jsonDialogo = UnityEngine.Resources.Load<TextAsset>("JSON/"+idioma+"/JSONInterface/"+textType);
        JsonUtility.FromJsonOverwrite(jsonDialogo.text, texts);

        foreach (var t in texts.texts)
        {
            if (t != null && t.id.Equals(idText)) return t.text;
        }

        return "";
    }
}
