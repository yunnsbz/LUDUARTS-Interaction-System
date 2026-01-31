using UnityEngine;

public class TestInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private InteractionTypes m_InteractionType;


    public string InteractableName { get; set; } = "test interactable";

    public InteractionTypes InteractionType { get => m_InteractionType; set => m_InteractionType = value; }

    void IInteractable.InteractObject()
    {
        Debug.Log("interacting with: " + InteractableName);
    }
}
