using UnityEditor;
using UnityEngine;
using System.IO;

public class PruebaCreacionScripts : MonoBehaviour
{
    [MenuItem("Assets/Create/Tools/CreateScripts")]
    public static void CreateScriptsViaMenu()
    {
        Debug.Log(AssetDatabase.GetAssetPath(Selection.activeObject));
        Debug.Log(Application.dataPath);

        string[] path = AssetDatabase.GetAssetPath(Selection.activeObject).Split("/");
        string scriptName = path[path.Length - 1];

        // Controller
        File.WriteAllText(
            Application.dataPath + 
            AssetDatabase.GetAssetPath(Selection.activeObject).Replace("Assets", "")+"/"+ scriptName + "Controller.cs",
            "using Infrastructure.Services;\r\nusing Infrastructure.Source.MVC.Objects.Controllers;\r\nusing Infrastructure.Source.MVC.States.DTO;\n" +
            "namespace " + AssetDatabase.GetAssetPath(Selection.activeObject).Replace("/", ".") + "\n" +
            "{\n" +
            "   public class "+ scriptName + "Controller : GameController<"+ scriptName + "View>\n" +
            "   {\n" +
            "   }\n" +
            "}\n");

        // Model
        File.WriteAllText(
            Application.dataPath +
            AssetDatabase.GetAssetPath(Selection.activeObject).Replace("Assets", "") + "/" + scriptName + "Model.cs",
            "using System;\r\nusing Infrastructure.Source.MVC.Objects.Models;\r\n" +
            "namespace " + AssetDatabase.GetAssetPath(Selection.activeObject).Replace("/", ".") + "\n" +
            "{\n" +
            "   [Serializable]\n" +
            "   public class " + scriptName + "View : GameModel\n" +
            "   {\n" +
            "   }\n" +
            "}\n");

        // View
        File.WriteAllText(
            Application.dataPath +
            AssetDatabase.GetAssetPath(Selection.activeObject).Replace("Assets", "") + "/" + scriptName + "View.cs",
            "using Infrastructure.Source.Attribute;\r\nusing Infrastructure.Source.MVC.Objects.Views;\r\nusing UnityEngine;\n" +
            "namespace " + AssetDatabase.GetAssetPath(Selection.activeObject).Replace("/",".") + "\n" +
            "{\n" +
            "   public class " + scriptName + "View : GameView\n" +
            "   {\n" +
            "       [ModelField, SerializeField] private "+ scriptName + "Model _model;\n" +
            "       [ControllerField] private " + scriptName + "Controller _controller;\n" +
            "   }\n" +
            "}");

        AssetDatabase.Refresh();
    }
}
