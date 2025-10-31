using System.Collections;
using UnityEngine;

public abstract class InteractBaseController : MonoBehaviour
{
    public TypeInteract typeInteract;

    public GameObject indicator;
    public Vector3 originalScaleIndicator;
    
    public enum TypeInteract
    {
        Dialogue,
        PickUpObject,
        Inspect,
        GameEvent
    }
    
    private void Start()
    {
        originalScaleIndicator = indicator.transform.localScale;
        indicator.transform.localScale = Vector3.zero;
        
        EventBus<InteractEvent>.Register(new EventBinding<InteractEvent>(Interact, gameObject));
        EventBus<EnterInteractEvent>.Register(new EventBinding<EnterInteractEvent>(EnterInteract, gameObject));
        EventBus<ExticInteractEvent>.Register(new EventBinding<ExticInteractEvent>(ExitInteract, gameObject));
    }

    private void OnDestroy()
    {
        EventBus<InteractEvent>.Deregister(new EventBinding<InteractEvent>(Interact, gameObject));
        EventBus<EnterInteractEvent>.Deregister(new EventBinding<EnterInteractEvent>(EnterInteract, gameObject));
        EventBus<ExticInteractEvent>.Deregister(new EventBinding<ExticInteractEvent>(ExitInteract, gameObject));
    }

    protected abstract IEnumerator Inter();

    private void Interact(InteractEvent interactEvent)
    {
        if (!interactEvent.obj.Equals(gameObject)) return;

        StartCoroutine(Inter());
    }

    protected virtual void EnterInteract(EnterInteractEvent e)
    {
        if (!e.obj.Equals(gameObject)) return;
        indicator.transform.localScale = originalScaleIndicator;
    }

    protected virtual void ExitInteract(ExticInteractEvent e)
    {
        if (!e.obj.Equals(gameObject)) return;
        indicator.transform.localScale = Vector3.zero;
    }
}

public class InteractEvent : IEvent
{
    public GameObject obj;
}

public class EnterInteractEvent : IEvent
{
    public GameObject obj;
}

public class ExticInteractEvent : IEvent
{
    public GameObject obj;
}