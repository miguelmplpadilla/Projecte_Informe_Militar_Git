using UnityEngine;

public class ScrollViewDialogosController : MonoBehaviour
{
    public GameObject backgroundContent;
    public GameObject content1;
    public GameObject content2;

    private void LateUpdate()
    {
        backgroundContent.SetActive(content1.transform.childCount > 0 || content2.transform.childCount > 0);
    }
}