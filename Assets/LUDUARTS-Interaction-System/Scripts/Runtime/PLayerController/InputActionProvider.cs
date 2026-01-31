using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// input kullanan sýnýflara PlayerInputActions yapýsýný ulaþtýrmak için kullanýlýr.
/// </summary>
/// <remarks>
/// awake fonksiyonu diðer objelerinkinden önce çalýþmasý gerektiði için exec order -1 yapýlmýþtýr.
/// bu sayede baþka monobehaviour'larda awake içnde çaðrýlabilir.
/// </remarks>
[DefaultExecutionOrder(-1)]
public class InputActionProvider : MonoBehaviour
{
    [SerializeField] private KeybindTarget m_Target;

    public static InputActionProvider instance;

    public static PlayerInputActions Inputs;
    public InputAction InteractionAction => m_Target.GetAction();

    private void Awake()
    {
        instance = this;
        Inputs = new PlayerInputActions();
    }
}
