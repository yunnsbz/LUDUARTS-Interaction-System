using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Bir etkileþim butonu kullanarak, detector tarafýndan belirlenen
/// <see cref="AInteractable"/> objesi ile oyuncunun etkileþime geçmesini saðlar.
/// </summary>
public class PlayerInteractor : MonoBehaviour
{
    #region Fields

    private InputAction m_InteractAction;
    private AInteractable m_CurrentInteractable;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        m_InteractAction = InputActionProvider.Instance.InteractionAction;
    }

    private void OnEnable()
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
        InteractableDetector.OnNewInteractableDetected += OnNewInteractableDetected;
        InteractableDetector.OnInteractableNotDetected += OnInteractableNotDetected;
        InteractableDetector.OnNewInteractableUnreachable += OnInteractableNotDetected;

        m_InteractAction.performed += OnInteractPerformed;
    }

    private void UnsubscribeEvents()
    {
        InteractableDetector.OnNewInteractableDetected -= OnNewInteractableDetected;
        InteractableDetector.OnInteractableNotDetected -= OnInteractableNotDetected;
        InteractableDetector.OnNewInteractableUnreachable -= OnInteractableNotDetected;

        m_InteractAction.performed -= OnInteractPerformed;
    }

    #endregion

    #region Detector Callbacks

    private void OnNewInteractableDetected(AInteractable interactable)
    {
        m_CurrentInteractable = interactable;
    }

    private void OnInteractableNotDetected()
    {
        m_CurrentInteractable = null;
    }

    #endregion

    #region Input Handling

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        if (m_CurrentInteractable != null)
        {
            m_CurrentInteractable.StartInteraction();
            return;
        }

        Debug.Log("No interaction object found to interact with.");
    }

    #endregion
}
