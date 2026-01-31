using UnityEngine;
using UnityEngine.UI;

public class HUD_HoldTimerHandler : MonoBehaviour
{
    [SerializeField] private Canvas m_Canvas;
    [SerializeField] private Image m_Image;

    private void OnEnable()
    {
        AHoldInteractable.OnHoldProgress += OnHoldProgress;
        AHoldInteractable.OnHoldStarted += ShowTimer;
        AHoldInteractable.OnHoldCompleted += HideTimer;
        AHoldInteractable.OnHoldCanceled += HideTimer;
    }

    private void ShowTimer(AHoldInteractable obj)
    {
        m_Canvas.enabled = true;
    }
    private void HideTimer(AHoldInteractable obj)
    {
        m_Canvas.enabled = false;
    }

    private void OnHoldProgress(float fill)
    {
        m_Image.fillAmount = fill;
    }

    private void Start()
    {
        m_Canvas.enabled = false;
    }

    
}
