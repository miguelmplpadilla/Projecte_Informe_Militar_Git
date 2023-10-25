using UnityEngine;

public class ScrollViewDialogosController : MonoBehaviour
{
    public GameObject backgroundContent;
    public GameObject content;

    private void LateUpdate()
    {
        backgroundContent.SetActive(content.transform.childCount > 0);
    }
}