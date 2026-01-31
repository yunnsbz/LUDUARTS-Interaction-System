using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Etkileþim prompt UI'ýný yöneten sýnýf.
/// Detector ve interactable event'lerini dinleyerek ekrandaki metni günceller.
/// </summary>
public class HUD_InteractionPromptHandler : MonoBehaviour
{
    #region Fields

    [Header("References")]
    [SerializeField] private TMP_Text m_InteractionPrompt;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        SubscribeEvents();
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
    }

    #endregion

    #region Event Subscriptions

    private void SubscribeEvents()
    {
        InteractableDetector.OnNewInteractableDetected += UpdateTextWithInteractable;
        InteractableDetector.OnInteractableNotDetected += OnInteractableNotDetected;
        InteractableDetector.OnNewInteractableUnreachable += OnNewInteractableUnreachable;

        AToggleInteractable.OnToggleActivated += UpdateTextWithInteractable;
        AToggleInteractable.OnToggleReverted += UpdateTextWithInteractable;

        AHoldInteractable.OnHoldCompleted += RemoveTextForInteractable;
        AHoldInteractable.OnHoldCanceled += UpdateTextWithInteractable;
        AHoldInteractable.OnHoldStarted += RemoveTextForInteractable;
    }

    private void UnsubscribeEvents()
    {
        InteractableDetector.OnNewInteractableDetected -= UpdateTextWithInteractable;
        InteractableDetector.OnInteractableNotDetected -= OnInteractableNotDetected;
        InteractableDetector.OnNewInteractableUnreachable -= OnNewInteractableUnreachable;

        AToggleInteractable.OnToggleActivated -= UpdateTextWithInteractable;
        AToggleInteractable.OnToggleReverted -= UpdateTextWithInteractable;

        AHoldInteractable.OnHoldCompleted -= RemoveTextForInteractable;
        AHoldInteractable.OnHoldCanceled -= UpdateTextWithInteractable;
        AHoldInteractable.OnHoldStarted -= RemoveTextForInteractable;
    }

    #endregion

    #region Prompt Logic

    private void RemoveTextForInteractable(AInteractable interactable)
    {
        m_InteractionPrompt.text = string.Empty;
    }

    private void OnNewInteractableUnreachable()
    {
        m_InteractionPrompt.text = "Object is unreachable";
    }

    private void OnInteractableNotDetected()
    {
        m_InteractionPrompt.text = string.Empty;
    }

    private void UpdateTextWithInteractable(AInteractable interactable)
    {
        string bindingDisplay =
            InputActionProvider.Instance.InteractionAction.GetBindingDisplayString();

        m_InteractionPrompt.text = interactable.InteractionType switch
        {
            InteractionTypes.Instant =>
                $"Tap {bindingDisplay} to {interactable.InteractablePromptText}",

            InteractionTypes.Hold =>
                $"Hold {bindingDisplay} to {interactable.InteractablePromptText}",

            InteractionTypes.Toggle =>
                $"Toggle {bindingDisplay} to {interactable.InteractablePromptText}",

            _ =>
                "Error occurred for interaction prompt"
        };
    }

    #endregion
}
