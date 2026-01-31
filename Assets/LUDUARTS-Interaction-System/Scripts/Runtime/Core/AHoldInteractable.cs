using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Hold (basýlý tutma) tipi etkileþimler için temel soyut sýnýf.
/// Belirlenen süre boyunca buton basýlý tutulduðunda etkileþim tamamlanýr.
/// </summary>
public abstract class AHoldInteractable : AInteractable
{
    #region Fields

    [Header("Hold Settings")]
    [SerializeField] private float m_HoldDuration = 2f;

    private bool m_TimerActive;
    private float m_Timer;

    /// <summary>
    /// Hold etkileþimini iptal edebilmek için kullanýlan input action referansý.
    /// </summary>
    private InputAction m_InteractAction;

    #endregion

    #region Events

    /// <summary>
    /// Hold baþladýðýnda tetiklenir.
    /// </summary>
    public static event Action<AHoldInteractable> OnHoldStarted;

    /// <summary>
    /// Hold baþarýyla tamamlandýðýnda tetiklenir.
    /// </summary>
    public static event Action<AHoldInteractable> OnHoldCompleted;

    /// <summary>
    /// Hold herhangi bir nedenle iptal edildiðinde tetiklenir.
    /// </summary>
    public static event Action<AHoldInteractable> OnHoldCanceled;

    /// <summary>
    /// Hold ilerleme durumu (0..1 arasý) her frame güncellenir.
    /// </summary>
    public static event Action<float> OnHoldProgress;

    #endregion

    #region Properties

    /// <summary>
    /// Etkileþim tipi.
    /// </summary>
    public override InteractionTypes InteractionType => InteractionTypes.Hold;

    #endregion

    #region Unity Methods

    protected virtual void Awake()
    {
        m_InteractAction = InputActionProvider.Instance.InteractionAction;
    }

    protected virtual void OnEnable()
    {
        InteractableDetector.OnInteractableNotDetected += OnInteractionCanceled;
        m_InteractAction.canceled += OnInteractActionCanceled;
    }

    protected virtual void OnDisable()
    {
        InteractableDetector.OnInteractableNotDetected -= OnInteractionCanceled;
        m_InteractAction.canceled -= OnInteractActionCanceled;
    }

    protected virtual void Update()
    {
        if (!m_TimerActive)
        {
            return;
        }

        m_Timer += Time.deltaTime;

        if (m_Timer >= m_HoldDuration)
        {
            CompleteHold();
            return;
        }

        OnHoldProgress?.Invoke(m_Timer / m_HoldDuration);
    }

    #endregion

    #region Interaction Core

    protected sealed override void OnInteractStartCore()
    {
        OnHoldInteractionStartCore();
        StartTimer();
        OnHoldStarted?.Invoke(this);
    }

    /// <summary>
    /// Hold etkileþimi tam baþladýðýnda yapýlacak iþlemler.
    /// Gerekmiyorsa boþ býrakýlabilir.
    /// </summary>
    protected abstract void OnHoldInteractionStartCore();

    /// <summary>
    /// Hold etkileþimi iptal edildiðinde yapýlacak iþlemler.
    /// </summary>
    protected abstract void OnHoldInteractionCanceledCore();

    /// <summary>
    /// Hold etkileþimi baþarýyla tamamlandýðýnda yapýlacak iþlemler.
    /// </summary>
    protected abstract void OnHoldCompleteCore();

    #endregion

    #region Timer Logic

    private void StartTimer()
    {
        m_Timer = 0f;
        m_TimerActive = true;
        Debug.Log("Hold timer started.");
    }

    private void StopTimer()
    {
        if (!m_TimerActive)
        {
            return;
        }

        m_TimerActive = false;
        m_Timer = 0f;
        Debug.Log("Hold timer stopped.");
    }

    private void CompleteHold()
    {
        StopTimer();
        OnHoldCompleteCore();
        OnHoldCompleted?.Invoke(this);
    }

    #endregion

    #region Cancellation

    /// <summary>
    /// Oyuncu hold butonunu býrakýrsa bu etkileþim iptal edilir.
    /// </summary>
    private void OnInteractActionCanceled(InputAction.CallbackContext context)
    {
        CancelHold();
    }

    /// <summary>
    /// Oyuncu baþka bir yere baktýðýnda (detector etkileþilebilir obje kaybettiðinde)
    /// bu etkileþim iptal edilir.
    /// </summary>
    private void OnInteractionCanceled()
    {
        CancelHold();
    }

    private void CancelHold()
    {
        if (!m_TimerActive)
        {
            return;
        }

        StopTimer();
        OnHoldInteractionCanceledCore();
        OnHoldCanceled?.Invoke(this);
    }

    #endregion
}
