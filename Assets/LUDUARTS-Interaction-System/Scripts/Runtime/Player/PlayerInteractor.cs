using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// bir etkileþim butonu kullanarak oyuncunun detector tarafýndan belirlenen IInteractable objesi ile etkileþime geçmesini saðlar.
/// </summary>
public class PlayerInteractor : MonoBehaviour
{
    private InputAction m_InteractAction;
    private IInteractable m_CurrentInteractable;

    public bool timerActive = false;
    public float duration = 2f;

    private float timer;


    private void Awake()
    {
        m_InteractAction = InputActionProvider.instance.InteractionAction;
    }

    private void OnEnable()
    {
        InteracableDetector.OnNewInteractableDetected += OnNewInteractableDetected;
        InteracableDetector.OnInteractableNotDetected += OnInteractableNotDetected;

        m_InteractAction.performed += InteractStarted;
        m_InteractAction.canceled += InteractCanceled;
    }

    void Update()
    {
        if (!timerActive)
            return;

        timer += Time.deltaTime;

        if (timer >= duration)
        {
            timerActive = false;
            timer = 0f;

            if (m_CurrentInteractable != null)
            {
                StopTimer();
                m_CurrentInteractable.StartInteractObject();
            }
        }
    }

    // Timer'ý dýþarýdan baþlatmak için
    public void StartTimer(float newDuration)
    {
        duration = newDuration;
        timer = 0f;
        timerActive = true;
        Debug.Log("timer started");
    }

    public void StopTimer()
    {
        if (timerActive)
        {
            timerActive = false;
            timer = 0f;
            Debug.Log("timer stoped");
        }
    }

    private void OnInteractableNotDetected()
    {
        if (m_CurrentInteractable != null) { 
            // hold modundaysa cancel olur. diðer modlar için yapýlmasý gereken bir þey yok.
            if (m_CurrentInteractable.InteractionType == InteractionTypes.Hold)
            {
                StopTimer();
            }

            m_CurrentInteractable = null;
        }
    }

    private void OnNewInteractableDetected(IInteractable interactable)
    {
        m_CurrentInteractable = interactable;
    }

    private void InteractCanceled(InputAction.CallbackContext ctx)
    {
        if (m_CurrentInteractable != null)
        {
            // hold tipi etkileþimlerde butondan elini çektiðinde cancel olur.
            if (m_CurrentInteractable.InteractionType == InteractionTypes.Hold)
            {
                StopTimer();
            }
        }
    }

    private void InteractStarted(InputAction.CallbackContext ctx)
    {
        if (m_CurrentInteractable != null)
        {
            if(m_CurrentInteractable.InteractionType == InteractionTypes.Instant)
                m_CurrentInteractable.StartInteractObject();
            else if(m_CurrentInteractable.InteractionType == InteractionTypes.Toggle)
            {
                // toggle state'i güncellenir.
                if (m_CurrentInteractable.ToggleModeState)
                {
                    m_CurrentInteractable.ToggleModeState = false;
                }
                else
                {
                    m_CurrentInteractable.ToggleModeState = true;
                }
                m_CurrentInteractable.StartInteractObject();
            }
            else if (m_CurrentInteractable.InteractionType == InteractionTypes.Hold)
            {
                StartTimer(m_CurrentInteractable.HoldModeMaxDuration);
            }
        }
        else
        {
            Debug.Log("no interaction object found to interact with");
        }
    }
}
