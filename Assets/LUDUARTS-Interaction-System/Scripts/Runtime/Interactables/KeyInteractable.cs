using UnityEngine;

public class KeyInteractable : AInteractable
{
    private void Awake()
    {
        OnInteractionStarted += KeyInteractable_OnInteractionStarted;
    }

    private void KeyInteractable_OnInteractionStarted(AInteractable interactable)
    {
        if (interactable == this)
            Debug.Log("interaction started for key (instant): " + interactable.InteractableName);
    }

    protected override void OnInteractStartCore()
    {
        // anahtarý alýp envantere yükleme
    }
}
