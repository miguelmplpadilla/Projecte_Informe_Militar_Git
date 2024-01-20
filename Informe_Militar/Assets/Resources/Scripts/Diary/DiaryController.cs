using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Resources.Scripts;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class DiaryController : MonoBehaviour
{
    private UIInput uiInput;

    private PausaController pausaController;
    private PlayerModel _model;

    private bool showingDiary = false;
    private bool diaryShowed = false;

    public DiaryScriptableObject currentDiary;

    public int currentPage = 0;

    public TextMeshProUGUI text1;
    public TextMeshProUGUI text2;
    
    public TextMeshProUGUI date1;
    public TextMeshProUGUI date2;
    
    public TextMeshProUGUI numPage1;
    public TextMeshProUGUI numPage2;
    
    private void Awake()
    {
        uiInput = new UIInput();
    }

    private void Start()
    {
        pausaController = GameObject.Find("PanelPausa").GetComponent<PausaController>();
        _model = GameObject.Find("Player").GetComponent<PlayerModel>();

        currentDiary.diary.paginas = new List<Page>();
    }

    private void OnEnable()
    {
        uiInput.Enable();
    }

    private void OnDisable()
    {
        uiInput.Disable();

        // Tests
        currentDiary.diary = new Diary();
        AssetDatabase.Refresh();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) AddPages("Entrada1");
        if (Input.GetKeyDown(KeyCode.H)) AddPages("Entrada2-1");
        if (Input.GetKeyDown(KeyCode.J)) AddPages("Entrada2-2");
        
        if (!uiInput.UISelf.OpenInventory.WasPressedThisFrame() || showingDiary || _model.pauseShowed) return;
        
        ShowHideDiary();
    }

    public async void ShowHideDiary()
    {
        pausaController.pause();
        
        if (!diaryShowed)
            SetPages();
        
        showingDiary = true;

        await transform.DOScale(diaryShowed ? 0 : 1, 0.5f)
            .SetEase(!diaryShowed ? Ease.OutBack : Ease.InBack).SetUpdate(true).AsyncWaitForCompletion();

        if (diaryShowed) currentPage = 0;

        showingDiary = false;
        diaryShowed = !diaryShowed;
    }

    private void SetPages()
    {
        date1.text = "";
        date2.text = "";
        text1.text = "";
        text2.text = "";
        numPage1.text = "1";
        numPage2.text = "2";
        
        if (currentDiary.diary.paginas.Count <= 0) return;
            
        text1.text = currentDiary.diary.paginas[currentPage].textLeft;
        text2.text = currentDiary.diary.paginas[currentPage].textRight;
        
        date1.text = currentDiary.diary.paginas[currentPage].dateLeft;
        date2.text = currentDiary.diary.paginas[currentPage].dateRight;

        numPage1.text = (currentPage*2+1).ToString();
        numPage2.text = (currentPage*2+2).ToString();
    }

    public void AddPages(string id)
    {
        if (CheckIfPageAdded(id)) return;
        
        Story fullDiary = JSONConverter.GetDiary();

        string pages = "";

        foreach (var passage in fullDiary.passages)
        {
            pages = passage.text;
            if (passage.name.Equals(id)) break;
        }

        Diary diary = new Diary();

        pages = pages.Split("<->")[0];
        
        JsonUtility.FromJsonOverwrite(pages, diary);

        Debug.Log(diary.paginas[0].id);

        for (int i = 0; i < diary.paginas.Count; i++)
            diary.paginas[i].id = diary.id + ">" + diary.paginas[i].id;
        
        currentDiary.diary.paginas.AddRange(diary.paginas);
        AssetDatabase.Refresh();

        Debug.Log("Added: "+id);
    }

    private bool CheckIfPageAdded(string id)
    {
        foreach (var page in currentDiary.diary.paginas)
        {
            string[] splitId = page.id.Split(">");

            if (splitId[0].Equals(id)) return true;
        }

        return false;
    }

    public void ChangeIdiomDiary()
    {
        Diary diaryCurrent = new Diary();
        JsonUtility.FromJsonOverwrite(JsonUtility.ToJson(currentDiary.diary), diaryCurrent);

        currentDiary.diary = new Diary();
        currentDiary.diary.paginas = new List<Page>();
        
        for (int i = 0; i < diaryCurrent.paginas.Count; i++)
        {
            string[] splitId = diaryCurrent.paginas[i].id.Split(">");
            
            AddPages(splitId[0]);
        }
    }

    public void SumPage()
    {
        if (currentDiary.diary.paginas.Count == 0) return;
        
        currentPage++;

        if (currentPage >= currentDiary.diary.paginas.Count)
            currentPage = currentDiary.diary.paginas.Count - 1;
        
        SetPages();
    }
    
    public void RestPage()
    {
        if (currentDiary.diary.paginas.Count == 0) return;
        
        currentPage--;
        
        if (currentPage < 0)
            currentPage = 0;
        
        SetPages();
    }
}
