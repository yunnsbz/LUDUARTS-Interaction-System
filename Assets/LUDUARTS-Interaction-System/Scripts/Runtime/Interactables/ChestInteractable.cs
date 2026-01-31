using UnityEngine;

public class ChestInteractable : HoldInteractable
{
    protected override void Awake()
    {
        base.Awake();

        OnHoldCanceled += ChestInteractable_OnHoldCanceled;
        OnHoldCompleted += ChestInteractable_OnHoldCompleted;
        OnHoldStarted += ChestInteractable_OnHoldStarted;
    }

    private void ChestInteractable_OnHoldStarted(HoldInteractable interactable)
    {
        Debug.Log("hold started for chest: " + interactable.InteractableName);
    }

    private void ChestInteractable_OnHoldCompleted(HoldInteractable interactable)
    {
        Debug.Log("hold completed for chest: " + interactable.InteractableName);
    }

    private void ChestInteractable_OnHoldCanceled(HoldInteractable interactable)
    {
        Debug.Log("hold canceled for chest: " + interactable.InteractableName);
    }

    protected override void OnHoldCompleteCore()
    {
        
    }

    protected override void OnHoldInteractionCanceledCore()
    {
        
    }

    protected override void OnHoldInteractionStartCore()
    {
        
    }
}
