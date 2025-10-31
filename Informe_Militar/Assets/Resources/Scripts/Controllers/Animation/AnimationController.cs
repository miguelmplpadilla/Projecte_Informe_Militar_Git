using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class AnimationController : BaseControllerStory
{
    public VideoPlayer videoPlayer;
    public TextMeshProUGUI description;

    public Canvas canvas;
    public CanvasGroup canvasGroup;
    
    public VideoClip desfaultAnimation;
    
    void Start()
    {
        EventBus<SetAnimation>.Register(new EventBinding<SetAnimation>(SetAnimation, gameObject));
        EventBus<HideAllScenes>.Register(new EventBinding<HideAllScenes>(HideScene, gameObject));
        EventBus<PlayAnimation>.Register(new EventBinding<PlayAnimation>(PlayAnimation, gameObject));
    }

    private void OnDestroy()
    {
        EventBus<SetAnimation>.Deregister(new EventBinding<SetAnimation>(SetAnimation, gameObject));
        EventBus<HideAllScenes>.Deregister(new EventBinding<HideAllScenes>(HideScene, gameObject));
        EventBus<PlayAnimation>.Deregister(new EventBinding<PlayAnimation>(PlayAnimation, gameObject));
    }

    private async void SetAnimation(SetAnimation animation)
    {
        nextNode = animation.nextNode;

        canvas.GetComponent<GraphicRaycaster>().enabled = true;
        canvasGroup.alpha = 1;

        VideoClip currentAnimation = animation.video;

        if (currentAnimation == null)
        {
            description.transform.parent.gameObject.SetActive(true);
            description.text = animation.descriptionAnimation;
            
            currentAnimation = desfaultAnimation;
        }
        else
        {
            description.transform.parent.gameObject.SetActive(false);
        }
        
        videoPlayer.clip = currentAnimation;
        videoPlayer.Prepare();

        while (!videoPlayer.isPrepared) await Task.Yield();
        
        PlayAnimation();
    }

    private async void PlayAnimation()
    {
        videoPlayer.loopPointReached += OnVideoEnd;
        videoPlayer.Play();

        while (!videoPlayer.isPlaying) await Task.Yield();
        
        videoPlayer.Pause();
        
        EventBus<FadeInFadeOut>.Raise(new FadeInFadeOut
        {
            fade = false,
            callback = () =>
            {
                videoPlayer.Play();
            }
        });
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        EventBus<SetNextScene>.Raise(new SetNextScene
        {
            node = nextNode
        });
    }
    
    private void HideScene()
    {
        canvas.GetComponent<GraphicRaycaster>().enabled = false;
        canvasGroup.alpha = 0;
    }
}
