using System.Collections.Generic;
using UnityEngine;

public class ChestInteractable : AHoldInteractable
{
    [SerializeField] private Animator m_Animator;
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
        m_Animator.SetBool("IsOpen", true);
    }

    public void CloseChest()
    {
        m_IsChestOpen = false;
        m_Animator.SetBool("IsOpen", false);
    }
}
