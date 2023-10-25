using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraFotosController : MonoBehaviour
{
    public RectTransform imagenCamara;
    public GameObject posicionFoto;

    public GameObject canvas;

    public RenderTexture renderTexture;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine("coroutineScreenshot");
        }
    }

    private IEnumerator coroutineScreenshot()
    {
        yield return new WaitForEndOfFrame();

        canvas.SetActive(false);

        Texture2D screenshotTexture =
            new Texture2D((int)imagenCamara.sizeDelta.x, (int)imagenCamara.sizeDelta.y, TextureFormat.ARGB32, false);

        Rect rect = new Rect(imagenCamara.position, imagenCamara.sizeDelta);
        
        screenshotTexture.ReadPixels(rect, 0, 0);
        screenshotTexture.Apply();

        byte[] byteArray = screenshotTexture.EncodeToPNG();
        
        System.IO.File.WriteAllBytes(Application.dataPath + "/camaraFoto.png", byteArray);
        
        canvas.SetActive(true);
    }
}