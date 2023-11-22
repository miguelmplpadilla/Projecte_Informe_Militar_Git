using System;
using System.Linq;
using Resources.Scripts;
using UnityEditor;
using UnityEngine;

public class CustomNPCController : MonoBehaviour
{
    public string currentAnimation = "Idle";

    private Animator animator;
    public SpriteRenderer spriteRenderer;
    public Character character;
    
    public SpriteRenderer head;
    public SpriteRenderer face;
    public SpriteRenderer torso;
    public SpriteRenderer legs;
    public SpriteRenderer feets;
    public SpriteRenderer skin;

    private int i = 0;
    
    [Space(10)]
    
    [SerializeField,
     ButtonInvoke(nameof(SetCharacterSprites), null, ButtonInvoke.DisplayIn.PlayAndEditModes,
         "Set Character Sprites")]
    private bool setCharacterSpritesFunction;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        setAllAnimations();
    }
    
    private void setAllAnimations(string nameAnimation = "")
    {
        if (!nameAnimation.Equals(""))
        {
            currentAnimation = nameAnimation;
            i = 0;
        }
        else
        {
            char[] characters = spriteRenderer.sprite.name.ToCharArray();
            i = int.Parse(characters.Last().ToString());
        }

        setSpriteAnimation(character.idHead, "Head", head, "NPCs");
        setSpriteAnimation(character.idTorso, "Torso", torso, "NPCs");
        setSpriteAnimation(character.idLegs, "Legs", legs, "NPCs");
        setSpriteAnimation(character.idFeets, "Feet", feets, "NPCs");
        setSpriteAnimation(character.idFace, "Face", face, "NPCs");
        
        setSpriteAnimation(character.idSkin, "Skin", skin, "Skins");
    }

    private void setSpriteAnimation(string id, string nameFolder, SpriteRenderer imageSprite, string folder)
    {
        if (id.Equals(""))
        {
            imageSprite.sprite = null;
            return;
        }
        
        imageSprite.transform.localScale = Vector3.one;

        string path = "Sprites/CustomNPCs/" + folder + "/" + id + "/" + currentAnimation + nameFolder + id;
        
        imageSprite.sprite = UnityEngine.Resources.LoadAll<Sprite>(path)[i];
    }

    public void SetCharacterSprites()
    {
        character = JSONManager.GetCharacterFromJSON();
        
        setAllAnimations("Idle");
        
        EditorUtility.SetDirty(gameObject);
    }
}
