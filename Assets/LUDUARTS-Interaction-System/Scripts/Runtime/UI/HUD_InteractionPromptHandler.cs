using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class HUD_InteractionPromptHandler : MonoBehaviour
{
    [SerializeField] private TMP_Text m_InteractionPrompt;

    private void Awake()
    {
        InteracableDetector.OnNewInteractableDetected += UpdateTextWithInteractable;
        InteracableDetector.OnInteractableNotDetected += OnInteractableNotDetected;
        InteracableDetector.OnNewInteractableUnreachable += OnNewInteractableUnreachable;

        AToggleInteractable.OnToggleActivated += UpdateTextWithInteractable;
        AToggleInteractable.OnToggleReverted += UpdateTextWithInteractable;

        AHoldInteractable.OnHoldCompleted += RemoveTextForInteractable;
        AHoldInteractable.OnHoldCanceled += UpdateTextWithInteractable;
        AHoldInteractable.OnHoldStarted += RemoveTextForInteractable;
    }

    private void RemoveTextForInteractable(AInteractable interactable)
    {
        m_InteractionPrompt.text = "";
    }

    private void OnNewInteractableUnreachable()
    {
        m_InteractionPrompt.text = "Object is unreachable";
    }

    private void OnInteractableNotDetected()
    {
        m_InteractionPrompt.text = "";
    }

    private void UpdateTextWithInteractable(AInteractable interactable)
    {
        m_InteractionPrompt.text = interactable.InteractionType switch
        {
            InteractionTypes.Instant => "Tap " + InputActionProvider.instance.InteractionAction.GetBindingDisplayString() + " to " + interactable.InteractablePromptText,
            InteractionTypes.Hold => "Hold " + InputActionProvider.instance.InteractionAction.GetBindingDisplayString() + " to " + interactable.InteractablePromptText,
            InteractionTypes.Toggle => "Toggle " + InputActionProvider.instance.InteractionAction.GetBindingDisplayString() + " to " + interactable.InteractablePromptText,
            _ => "Error occured for interaction prompt"

        };
    }
}
