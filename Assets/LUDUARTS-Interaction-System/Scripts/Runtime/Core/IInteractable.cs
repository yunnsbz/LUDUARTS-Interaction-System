using UnityEngine;

public enum InteractionTypes
{
    Instant = 0,
    Hold = 1,
    Toggle = 2,
}

public interface IInteractable
{
    public string InteractableName { get; set; }
    public bool ToggleModeState { get; set; }
    public float HoldModeMaxDuration { get; set; }

    public InteractionTypes InteractionType { get; set; }


    /// <summary>
    /// objeler ile etkileþim kurmak için kullanýlýr.
    /// </summary>
    /// <remarks>genelde input etkileþimi sonrasý çaðrýlýr.</remarks>
    public void StartInteractObject();
}
