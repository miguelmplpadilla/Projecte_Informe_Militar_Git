using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SelectorController : MonoBehaviour
{
    [Header("Head")]
    public GameObject prefabSelectorHead;
    public GameObject headContiner;
    
    [Header("Torso")]
    public GameObject prefabSelectorTorso;
    public GameObject torsoContiner;
    
    [Header("Legs")]
    public GameObject prefabSelectorLegs;
    public GameObject legsContiner;
    
    [Header("Feet")]
    public GameObject prefabSelectorFeets;
    public GameObject feetsContiner;
    
    [Header("Face")]
    public GameObject faceContiner;

    private void Start()
    {
        createSelectors("Head", headContiner, prefabSelectorHead);
        createSelectors("Torso", torsoContiner, prefabSelectorTorso);
        createSelectors("Legs", legsContiner, prefabSelectorLegs);
        createSelectors("Feet", feetsContiner, prefabSelectorFeets);
        createSelectors("Face", faceContiner, prefabSelectorHead);
    }

    private void createSelectors(string sectionToCreate, GameObject continer, GameObject prefabSelector)
    {
        string[] dirs = Directory.GetDirectories(Application.dataPath+"/Resources/Sprites/CustomNPCs/NPCs");
        
        instanceSelector(sectionToCreate, continer, prefabSelector, "", "", true);

        foreach (var dir in dirs)
        {
            string[] dirSplit = dir.Split("/Resources/");
            Texture2D[] textures = UnityEngine.Resources.LoadAll<Texture2D>(dirSplit[1]);
            foreach (var texture in textures)
            {
                if (texture.name.Contains("Idle"+sectionToCreate))
                    instanceSelector(sectionToCreate, continer, prefabSelector, texture.name, dirSplit[1]);
            }
        }
    }

    private void instanceSelector(string sectionToCreate, GameObject continer, GameObject prefabSelector, string textureName, string dir, bool noneSelector = false)
    {
        GameObject selector = Instantiate(prefabSelector, continer.transform);
        selector.transform.GetChild(0).GetComponent<Image>().sprite = 
            UnityEngine.Resources.LoadAll<Sprite>(dir + "/" + textureName)[0];
        
        if (noneSelector) selector.transform.GetChild(0).localScale = Vector3.zero;

        SelectorButton selectorButton = selector.GetComponent<SelectorButton>();
        selectorButton.selectorType = sectionToCreate;
        selectorButton.skin = textureName.Replace("Idle"+sectionToCreate, "");
    }

    public void showPanel(GameObject panel)
    {
        panel.transform.SetAsLastSibling();
    }
}
