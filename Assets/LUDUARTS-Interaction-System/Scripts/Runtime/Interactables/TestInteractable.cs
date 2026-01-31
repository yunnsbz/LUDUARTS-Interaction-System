using UnityEngine;

public class TestInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private InteractionTypes m_InteractionType;
    [SerializeField] private float m_InteractionDuration;

    public string InteractableName { get; set; } = "test interactable";

    public InteractionTypes InteractionType { get => m_InteractionType; set => m_InteractionType = value; }
    public bool ToggleModeState { get; set; }
    public float HoldModeMaxDuration { get => m_InteractionDuration; set => m_InteractionDuration = value; }

    void IInteractable.StartInteractObject()
    {
        Debug.Log("started interaction with: " + InteractableName);
    }
}
