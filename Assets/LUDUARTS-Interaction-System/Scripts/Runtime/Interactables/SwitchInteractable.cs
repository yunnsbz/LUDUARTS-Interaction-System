using UnityEngine;
using UnityEngine.Events;

public class SwitchInteractable : AToggleInteractable
{
    [SerializeField] private UnityEvent onActivateTriggered;
    [SerializeField] private UnityEvent onRevertTriggered;

    protected override void OnActivateToggleCore() 
    { 
        onActivateTriggered?.Invoke();
    }

    protected override void OnRevertToggleCore()
    {
        onRevertTriggered?.Invoke();
    }
}
