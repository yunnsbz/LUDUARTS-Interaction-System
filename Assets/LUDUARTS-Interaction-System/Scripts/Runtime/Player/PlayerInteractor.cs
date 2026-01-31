using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// bir etkileþim butonu kullanarak oyuncunun detector tarafýndan belirlenen IInteractable objesi ile etkileþime geçmesini saðlar.
/// </summary>
public class PlayerInteractor : MonoBehaviour
{
    private PlayerInputActions m_Input;
    private IInteractable m_CurrentInteractable;


    private void Awake()
    {
        m_Input = InputActionProvider.Inputs;
    }

    private void OnEnable()
    {
        InteracableDetector.OnNewInteractableDetected += OnNewInteractableDetected;
        InteracableDetector.OnInteractableNotDetected += OnInteractableNotDetected;

        m_Input.Player.Interact.performed += InteractStarted;
        m_Input.Player.Interact.canceled += InteractCanceled;
    }

    private void OnInteractableNotDetected()
    {
        m_CurrentInteractable = null;
    }

    private void OnNewInteractableDetected(IInteractable interactable)
    {
        m_CurrentInteractable = interactable;
    }

    private void InteractCanceled(InputAction.CallbackContext ctx)
    {
        Debug.Log("canceled: " + m_CurrentInteractable?.InteractableName + " " + m_CurrentInteractable?.InteractionType);
    }

    private void InteractStarted(InputAction.CallbackContext ctx)
    {
        if (m_CurrentInteractable != null)
        {
            Debug.Log("started: " + m_CurrentInteractable.InteractableName + " " + m_CurrentInteractable.InteractionType);
        }
        else
        {
            Debug.Log("no interaction object found to interact with");
        }
    }
}
