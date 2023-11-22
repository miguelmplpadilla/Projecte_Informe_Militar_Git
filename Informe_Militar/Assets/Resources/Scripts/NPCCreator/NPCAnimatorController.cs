using System.IO;
using System.Linq;
using Resources.Scripts;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class NPCAnimatorController : MonoBehaviour
{
    public bool manual = false;
    
    [Range(0, 6)]
    public int j = 0;

    public string currentAnimation = "Idle";

    public Character character = new Character();

    public SpriteRenderer spriteRenderer;

    public Image head;
    public Image torso;
    public Image legs;
    public Image feets;
    public Image skin;
    public Image face;

    public TMP_Dropdown dropdown;

    private int i = 0;

    private void Update()
    {
        setAllAnimations();
    }

    private void setAllAnimations(string nameAnimation = "")
    {
        if (!nameAnimation.Equals("")) currentAnimation = nameAnimation;
        
        char[] characters = spriteRenderer.sprite.name.ToCharArray();
        i = int.Parse(characters.Last().ToString());
        
        currentAnimation = dropdown.options[dropdown.value].text;
        
        setSpriteAnimation(character.idHead, "Head", head, "NPCs");
        setSpriteAnimation(character.idTorso, "Torso", torso, "NPCs");
        setSpriteAnimation(character.idLegs, "Legs", legs, "NPCs");
        setSpriteAnimation(character.idFeets, "Feet", feets, "NPCs");
        setSpriteAnimation(character.idFace, "Face", face, "NPCs");
        
        setSpriteAnimation(character.idSkin, "Skin", skin, "Skins");
    }

    private void setSpriteAnimation(string id, string nameFolder, Image imageSprite, string folder)
    {
        if (id.Equals(""))
        {
            imageSprite.transform.localScale = Vector3.zero;
            return;
        }
        
        imageSprite.transform.localScale = Vector3.one;

        string path = "Sprites/CustomNPCs/" + folder + "/" + id + "/" + currentAnimation + nameFolder + id;
        
        imageSprite.sprite = UnityEngine.Resources.LoadAll<Sprite>(path)[manual ? j : i];
    }

    public void setColorSkin(string idColor)
    {
        character.idSkin = idColor;
    }
}
