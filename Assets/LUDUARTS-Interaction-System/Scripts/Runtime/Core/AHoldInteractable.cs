using System;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class HoldInteractable : AInteractable
{
    [SerializeField] float m_HoldDuration = 2f;

    private bool m_TimerActive = false;

    private float m_Timer;
    public override InteractionTypes InteractionType => InteractionTypes.Hold;

    
    // needed to cancel hold action
    private InputAction m_InteractAction;

    public static event Action<HoldInteractable> OnHoldStarted;
    public static event Action<HoldInteractable> OnHoldCompleted;
    public static event Action<HoldInteractable> OnHoldCanceled;


    protected virtual void Awake()
    {
        m_InteractAction = InputActionProvider.instance.InteractionAction;
    }

    protected virtual void OnEnable()
    {
        InteracableDetector.OnInteractableNotDetected += OnInteractionCanceled;

        m_InteractAction.canceled += InteractActionCanceled;
    }

    protected virtual void Update()
    {
        if (!m_TimerActive)
            return;

        m_Timer += Time.deltaTime;

        if (m_Timer >= m_HoldDuration)
        {
            m_TimerActive = false;
            m_Timer = 0f;

            StopTimer();
            OnHoldCompleteCore();
            OnHoldCompleted?.Invoke(this);
        }
    }


    protected sealed override void OnInteractStartCore()
    {
        OnHoldInteractionStartCore();
        StartTimer();
        OnHoldStarted?.Invoke(this);
    }

    /// <summary>
    /// hold etkileþimi tam baþlayýnca yapýlabilecek iþlemler buraya eklenir. gerekmiyorsa boþ býrakýlsýn.
    /// </summary>
    protected abstract void OnHoldInteractionStartCore();

    /// <summary>
    /// aksiyon bir nedenden dolayý iptal olursa buradaki iþlemler uygulanýr.
    /// </summary>
    protected abstract void OnHoldInteractionCanceledCore();

    /// <summary>
    /// aksiyon süre sonuna gelince yapýlacaklar buraya yazýlýr.
    /// </summary>
    protected abstract void OnHoldCompleteCore();


    /// <summary>
    /// Timer'ý baþlatmak için.
    /// </summary>
    private void StartTimer()
    {
        m_Timer = 0f;
        m_TimerActive = true;
        Debug.Log("timer started");
    }

    private void StopTimer()
    {
        if (m_TimerActive)
        {
            m_TimerActive = false;
            m_Timer = 0f;
            Debug.Log("timer stoped");
        }
    }

    /// <summary>
    /// oyuncu hold butonunu býrakýrsa bu etkileþim iptal olur.
    /// </summary>
    /// <param name="obj"></param>
    private void InteractActionCanceled(InputAction.CallbackContext obj)
    {
        if (m_TimerActive) { 
            StopTimer();
            OnHoldCanceled?.Invoke(this);
        }
    }

    /// <summary>
    /// oyuncu baþka bir yere bakarsa da bu etkileþim iptal olur. etkileþimin burada da kalýcý olmasý için bir flag kullan.
    /// </summary>
    private void OnInteractionCanceled()
    {
        if (m_TimerActive)
        {
            StopTimer();
            OnHoldCanceled?.Invoke(this);
        }
    }
}
