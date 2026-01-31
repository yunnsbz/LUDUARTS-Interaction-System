using System;
using UnityEngine;

public abstract class AToggleInteractable : AInteractable
{
    public override InteractionTypes InteractionType => InteractionTypes.Toggle;

    private bool m_ToggleState = false;

    public static event Action<AToggleInteractable> OnToggleActivated;
    public static event Action<AToggleInteractable> OnToggleReverted;

    protected sealed override void OnInteractStartCore()
    {
        if (m_ToggleState)
        {
            m_ToggleState = false;
            OnRevertToggleCore();
            OnToggleReverted?.Invoke(this);
        }
        else
        {
            m_ToggleState = true;
            OnActivateToggleCore();
            OnToggleActivated?.Invoke(this);
        }
    }

    protected abstract void OnActivateToggleCore();

    protected abstract void OnRevertToggleCore();

    /// <summary>
    /// o anki toggle iþlemi baþarýsýz olduysa toggle state'i eski haline almak için miras alan sýnýflarda kullanýlýr.
    /// </summary>
    protected void CancelToggle()
    {
        m_ToggleState = !m_ToggleState;
    }
}
