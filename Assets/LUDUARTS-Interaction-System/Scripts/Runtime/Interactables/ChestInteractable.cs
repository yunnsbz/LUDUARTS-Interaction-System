using System.Collections.Generic;
using UnityEngine;

public class ChestInteractable : AHoldInteractable
{
    private bool m_IsChestOpen = false;

    private List<IItem> m_ChestItems;

    protected override void OnHoldCompleteCore()
    {
        if (m_IsChestOpen)
        {
            CloseChest();
        }
        else
        {
            OpenChest();
        }
    }

    protected override void OnHoldInteractionCanceledCore()
    {
        
    }

    protected override void OnHoldInteractionStartCore()
    {
        
    }


    public void OpenChest()
    {
        m_IsChestOpen = true;
    }

    public void CloseChest()
    {
        m_IsChestOpen = false;
    }
}
