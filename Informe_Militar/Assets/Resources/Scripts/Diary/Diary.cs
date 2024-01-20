using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Diary
{
    public string id;
    public List<Page> paginas;
}

[Serializable]
public class Page
{
    public string id;
    
    public string dateLeft;
    [TextArea(10,10)]
    public string textLeft;
        
    public string dateRight;
    [TextArea(10,10)]
    public string textRight;
}

[CreateAssetMenu(fileName = "CurrentDiary", menuName = "ScriptableObjects/Diary")]
public class DiaryScriptableObject : ScriptableObject
{
    public Diary diary;
}
