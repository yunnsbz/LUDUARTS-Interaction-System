using UnityEngine;

public class DoorInteractable : AToggleInteractable
{

    [SerializeField] private bool m_IsLocked;
    [SerializeField] private KeyTypes m_RequiredKeyType;

    private bool m_IsDoorOpen = false;

    public bool IsLocked => m_IsLocked;
    public KeyTypes RequiredKeyType => m_RequiredKeyType;



    protected override void OnActivateToggleCore()
    {
        ToggleDoor();
    }
    protected override void OnRevertToggleCore()
    {
        ToggleDoor();
    }

    public void ToggleDoor()
    {
        // kapý açýksa direkt kapa
        if (m_IsDoorOpen)
        {
            CloseDoor();
        }
        else
        {
            // kapý kilitliyse anahtar gerekli
            if (m_IsLocked)
            {
                // kapý kapalýysa anahtar varsa aç
                if (CheckPlayerInventoryForKey())
                {
                    OpenDoor();
                }
                else
                {
                    // anahtarýn yoksa state deðiþimini iptal et
                    CancelToggle();
                }
            }
            else
            {
                // kapý kilitli deðilse direkt açýlýr.
                OpenDoor();
            }
        }
    }


    public void OpenDoor()
    {
        m_IsDoorOpen = true;
        Debug.Log("door opened");
    }

    public void CloseDoor()
    {
        m_IsDoorOpen = false;
        Debug.Log("door closed");
    }

    private bool CheckPlayerInventoryForKey()
    {
        var items = Inventory.Instance.GetAllItems();

        foreach (var item in items)
        {
            if (item is KeyItem keyItem)
            {
                if (keyItem.KeyType == m_RequiredKeyType)
                {
                    return true;
                }
            }
        }
        Debug.Log("key not found: " + m_RequiredKeyType);
        return false;
    }
}
