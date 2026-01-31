using UnityEngine;

public class SwitchInteractable : AToggleInteractable
{
    private void Awake()
    {
        OnToggleActivated += SwitchInteractable_OnToggleActivated;
        OnToggleReverted += SwitchInteractable_OnToggleReverted;
    }

    private void SwitchInteractable_OnToggleReverted(AToggleInteractable interactable)
    {
        if (interactable == this)
            Debug.Log("toggle reverted for Switch: " + interactable.InteractableName);
    }

    private void SwitchInteractable_OnToggleActivated(AToggleInteractable interactable)
    {
        if (interactable == this)
            Debug.Log("toggle activated for Switch: " + interactable.InteractableName);
    }

    protected override void OnActivateToggleCore()
    {
        
    }

    protected override void OnRevertToggleCore()
    {
        
    }
}
