using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// bir etkileþim butonu kullanarak oyuncunun detector tarafýndan belirlenen IInteractable objesi ile etkileþime geçmesini saðlar.
/// </summary>
public class PlayerInteractor : MonoBehaviour
{
    private InputAction m_InteractAction;
    private AInteractable m_CurrentInteractable;

    private void Awake()
    {
        m_InteractAction = InputActionProvider.instance.InteractionAction;
    }

    private void OnEnable()
    {
        InteracableDetector.OnNewInteractableDetected += OnNewInteractableDetected;
        InteracableDetector.OnInteractableNotDetected += OnInteractableNotDetected;
        InteracableDetector.OnNewInteractableUnreachable += OnInteractableNotDetected;

        m_InteractAction.performed += InteractStarted;
    }

    private void OnInteractableNotDetected()
    {
        if (m_CurrentInteractable != null) { 
            m_CurrentInteractable = null;
        }
    }

    private void OnNewInteractableDetected(AInteractable interactable)
    {
        m_CurrentInteractable = interactable;
    }

    private void InteractStarted(InputAction.CallbackContext ctx)
    {
        if (m_CurrentInteractable != null)
        {
            m_CurrentInteractable.StartInteraction();
        }
        else
        {
            Debug.Log("no interaction object found to interact with");
        }
    }
}
