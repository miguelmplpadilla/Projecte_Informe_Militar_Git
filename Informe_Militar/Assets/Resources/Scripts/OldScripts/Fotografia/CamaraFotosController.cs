using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Cinemachine;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CamaraFotosController : MonoBehaviour
{
    [Header("Photo Taker")] [SerializeField]
    private Image photoDisplayArea;

    [Header("UI")] 
    public GameObject framePhoto;
    public GameObject photoHolder;

    private CinemachineVirtualCamera cm;
    private PlayerModel playerModel;
    private PausaController pausaController;

    private Texture2D screenCapture;

    private float lastCameraSize = 0;

    public bool cameraShowed = false;
    public bool takingPhoto = true;

    private void Start()
    {
        pausaController = GameObject.Find("PanelPausa").GetComponent<PausaController>();
        playerModel = GameObject.Find("Player").GetComponent<PlayerModel>();
        cm = GameObject.Find("CM").GetComponent<CinemachineVirtualCamera>();
        lastCameraSize = cm.m_Lens.OrthographicSize;
        screenCapture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            showCamera();

        if (Input.GetKeyDown(KeyCode.F) && cameraShowed)
            StartCoroutine("CapturePhoto");
    }

    private void showCamera()
    {
        cameraShowed = !cameraShowed;
        var transposer = cm.GetCinemachineComponent<CinemachineTransposer>();
        Vector3 followingOffset = transposer.m_FollowOffset;
        followingOffset.x = cameraShowed ? 1.5f : 0;
        transposer.m_FollowOffset = followingOffset;
        cm.m_Lens.OrthographicSize = cameraShowed ? 2.5f : lastCameraSize;

        playerModel.canInter = !cameraShowed;
        playerModel.canRun = !cameraShowed;
        
        InterAllManager.hideAllInterButtons();

        framePhoto.SetActive(cameraShowed);
    }

    IEnumerator CapturePhoto()
    {
        yield return new WaitForEndOfFrame();

        framePhoto.SetActive(false);

        Rect regionToReed = new Rect(0, 0, Screen.width, Screen.height);

        screenCapture.ReadPixels(regionToReed, 0, 0, false);
        screenCapture.Apply();
        photoHolder.SetActive(false);
        framePhoto.SetActive(true);

        ShowPhoto();
    }

    private async void ShowPhoto()
    {
        Sprite photoSprite = Sprite.Create(screenCapture, new Rect(0, 0, screenCapture.width, screenCapture.height),
            new Vector2(0.5f, 0), 100);
        photoDisplayArea.sprite = photoSprite;

        if (!takingPhoto)
        {
            pausaController.pause();
            
            GameObject imageDocument = GameObject.Find("ImageFoto");

            imageDocument.GetComponent<Image>().sprite = photoSprite;

            await imageDocument.transform.parent.parent.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).SetUpdate(true)
                .AsyncWaitForCompletion();
            imageDocument.transform.parent.DOKill();
            
            saveSprite(photoSprite,
                Application.dataPath + "/Resources/Sprites/Camera/Fotografias/" + SceneManager.GetActiveScene().name +
                "_" + DateTime.Now.ToString("MM-dd-yy (HH-mm-ss)") + ".png");

            takingPhoto = true;

            return;
        }
        
        photoHolder.SetActive(true);
        StartCoroutine("CapturePhoto");
        takingPhoto = !takingPhoto;
    }

    private void saveSprite(Sprite sp, string path)
    {
        Texture2D itemBgTex = sp.texture;
        byte[] itemBgBytes = itemBgTex.EncodeToPNG();
        File.WriteAllBytes(path, itemBgBytes);
    }
}