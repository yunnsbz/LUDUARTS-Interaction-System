using UnityEngine;

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
    public static PlayerInputActions Inputs;

    private void Awake()
    {
        Inputs = new PlayerInputActions();
    }
}
