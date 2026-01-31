using UnityEngine;
using UnityEngine.Events;

public class SwitchInteractable : AToggleInteractable
{
    [SerializeField] private Animator m_Animator;
    [SerializeField] private UnityEvent onActivateTriggered;
    [SerializeField] private UnityEvent onRevertTriggered;

    protected override void OnActivateToggleCore() 
    { 
        onActivateTriggered?.Invoke();
        m_Animator.SetBool("IsActive", true);
    }

    protected override void OnRevertToggleCore()
    {
        onRevertTriggered?.Invoke();
        m_Animator.SetBool("IsActive", false);
    }
}
