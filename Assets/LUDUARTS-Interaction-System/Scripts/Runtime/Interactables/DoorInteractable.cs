using UnityEngine;

public class DoorInteractable : AToggleInteractable
{
    private void Awake()
    {
        OnToggleActivated += SwitchInteractable_OnToggleActivated;
        OnToggleReverted += SwitchInteractable_OnToggleReverted;
    }

    private void SwitchInteractable_OnToggleReverted(AToggleInteractable interactable)
    {
        if (interactable == this)
            Debug.Log("toggle reverted for door: " + interactable.InteractableName);
    }

    private void SwitchInteractable_OnToggleActivated(AToggleInteractable interactable)
    {
        if(interactable == this)
            Debug.Log("toggle activated for door: " + interactable.InteractableName);
    }

    protected override void OnActivateToggleCore()
    {
        
    }

    protected override void OnRevertToggleCore()
    {
        
    }
}
