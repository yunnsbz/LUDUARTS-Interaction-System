using UnityEngine;

/// <summary>
/// Toggle tipi etkileþimle açýlýp kapanabilen kapý.
/// Kilitliyse, oyuncunun envanterinde gerekli anahtar olup olmadýðýný kontrol eder.
/// </summary>
public class DoorInteractable : AToggleInteractable
{
    #region Fields

    [Header("Door Settings")]
    [SerializeField] private Animator m_Animator;
    [SerializeField] private bool m_IsLocked;
    [SerializeField] private KeyTypes m_RequiredKeyType;

    private bool m_IsDoorOpen;

    #endregion

    #region Properties

    /// <summary>
    /// Kapýnýn kilitli olup olmadýðýný belirtir.
    /// </summary>
    public bool IsLocked => m_IsLocked;

    /// <summary>
    /// Kapýyý açmak için gereken anahtar türü.
    /// </summary>
    public KeyTypes RequiredKeyType => m_RequiredKeyType;

    #endregion

    #region Toggle Core

    protected override void OnActivateToggleCore()
    {
        ToggleDoor();
    }

    protected override void OnRevertToggleCore()
    {
        ToggleDoor();
    }

    #endregion

    #region Door Logic

    /// <summary>
    /// Kapýnýn açýk/kapalý durumunu deðiþtirir.
    /// </summary>
    public void ToggleDoor()
    {
        if (m_IsDoorOpen)
        {
            CloseDoor();
            return;
        }

        if (m_IsLocked)
        {
            if (CheckPlayerInventoryForKey())
            {
                OpenDoor();
                return;
            }

            // Anahtar yoksa state deðiþimini iptal et
            CancelToggle();
            return;
        }

        // Kapý kilitli deðilse direkt açýlýr
        OpenDoor();
    }

    /// <summary>
    /// Kapýyý açar.
    /// </summary>
    public void OpenDoor()
    {
        m_IsDoorOpen = true;
        SetPromptText("close door");

        if (m_Animator != null)
        {
            m_Animator.SetBool("IsOpen", true);
        }

        Debug.Log("Door opened.");
    }

    /// <summary>
    /// Kapýyý kapatýr.
    /// </summary>
    public void CloseDoor()
    {
        m_IsDoorOpen = false;
        SetPromptText("open door");

        if (m_Animator != null)
        {
            m_Animator.SetBool("IsOpen", false);
        }

        Debug.Log("Door closed.");
    }

    #endregion

    #region Inventory Check

    private bool CheckPlayerInventoryForKey()
    {
        var items = Inventory.Instance.GetAllItems();

        foreach (var item in items)
        {
            if (item is not KeyItem keyItem)
            {
                continue;
            }

            if (keyItem.KeyType == m_RequiredKeyType)
            {
                return true;
            }
        }

        Debug.Log($"Key not found: {m_RequiredKeyType}");
        return false;
    }

    #endregion
}
