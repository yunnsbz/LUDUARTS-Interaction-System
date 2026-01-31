using System;
using UnityEngine;

public enum InteractionTypes
{
    Instant = 0,
    Hold = 1,
    Toggle = 2, 
}

/// <summary>
/// bu haliyle instant etkileþimler için kullanýlýr. eðer hold yada toggle tipi aksiyon kullanmak istiyorsanýz o abstract sýnýflarý implemente ediniz.
/// </summary>
public abstract class AInteractable : MonoBehaviour
{
    [SerializeField] protected string m_InteractableName = "Interactable";
    public string InteractableName => m_InteractableName;

    public virtual InteractionTypes InteractionType => InteractionTypes.Instant;


    public static event Action<AInteractable> OnInteractionStarted;

    public void StartInteraction()
    {
        OnInteractStartCore();
        OnInteractionStarted?.Invoke(this);
    }

    protected abstract void OnInteractStartCore();
}
