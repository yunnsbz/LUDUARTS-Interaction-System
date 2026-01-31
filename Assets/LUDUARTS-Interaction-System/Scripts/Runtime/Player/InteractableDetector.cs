using System;
using UnityEngine;

/// <summary>
/// Raycast yaparak oyuncunun önünde etkileþime girebileceði bir obje olup olmadýðýný tespit eder.
/// </summary>
/// <remarks>
/// "Interactable" layer mask'ý kullanarak objeleri bulur.
/// Obje çok uzaktaysa <see cref="OnNewInteractableUnreachable"/> eventini tetikler.
/// Birden fazla obje varsa en yakýndakini seçer.
/// Hiç obje yoksa <see cref="OnInteractableNotDetected"/> eventini tetikler.
/// </remarks>
public class InteractableDetector : MonoBehaviour
{
    #region Fields

    [Header("Detection")]
    [SerializeField] private float m_MaxInteractionDistance = 1f;
    [SerializeField] private LayerMask m_LayerMask;

    private Ray m_Ray;
    private readonly RaycastHit[] m_HitResults = new RaycastHit[64];

    /// <summary>
    /// Üst üste gereksiz event tetiklenmesini engellemek için kullanýlýr.
    /// </summary>
    private bool m_InteractableDetected;

    /// <summary>
    /// Bir kez unreachable olduktan sonra gereksiz event tetiklenmesini engellemek için kullanýlýr.
    /// </summary>
    private bool m_InteractableUnreachableOnce;

    private AInteractable m_CurrentInteractable;

    #endregion

    #region Events

    /// <summary>
    /// Her farklý <see cref="AInteractable"/> objesi tespit edildiðinde tetiklenir.
    /// </summary>
    public static event Action<AInteractable> OnNewInteractableDetected;

    /// <summary>
    /// Bulunan <see cref="AInteractable"/> objesi etkileþim mesafesi dýþýndaysa tetiklenir.
    /// Genellikle UI için kullanýlýr.
    /// </summary>
    public static event Action OnNewInteractableUnreachable;

    /// <summary>
    /// Oyuncunun önünde hiçbir etkileþilebilir obje yoksa tetiklenir.
    /// Genellikle UI için kullanýlýr.
    /// </summary>
    public static event Action OnInteractableNotDetected;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        m_Ray = new Ray(transform.position, transform.forward);
    }

    private void FixedUpdate()
    {
        UpdateRay();
        PerformRaycast();
    }

    #endregion

    #region Detection Logic

    private void UpdateRay()
    {
        m_Ray.origin = transform.position;
        m_Ray.direction = transform.forward;
    }

    private void PerformRaycast()
    {
        int hitCount = Physics.RaycastNonAlloc(
            m_Ray,
            m_HitResults,
            10f,
            m_LayerMask);

        if (hitCount > 0)
        {
            m_InteractableDetected = true;

            RaycastHit closestHit = FindClosestHitResult(hitCount);
            HandleClosestHit(closestHit);
        }
        else if (m_InteractableDetected)
        {
            ResetDetectionState();

            OnInteractableNotDetected?.Invoke();
            Debug.Log("Interactable not detected.");
        }
    }

    private void ResetDetectionState()
    {
        m_CurrentInteractable = null;
        m_InteractableUnreachableOnce = false;
        m_InteractableDetected = false;
    }

    #endregion

    #region Helpers

    private RaycastHit FindClosestHitResult(int hitCount)
    {
        RaycastHit closestHit = default;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < hitCount; i++)
        {
            float distance = m_HitResults[i].distance;

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestHit = m_HitResults[i];
            }
        }

        return closestHit;
    }

    private void HandleClosestHit(RaycastHit hit)
    {
        // Collision'lar genellikle child objelerde olacaðý için parent kontrol edilir.
        AInteractable interactable =
            hit.transform.GetComponentInParent<AInteractable>();

        if (interactable == null)
        {
            Debug.LogWarning(
                "An object with Interactable layer does not have an AInteractable component: " +
                hit.collider.name);
            return;
        }

        if (hit.distance <= m_MaxInteractionDistance)
        {
            HandleReachableInteractable(interactable);
        }
        else
        {
            HandleUnreachableInteractable();
        }
    }

    private void HandleReachableInteractable(AInteractable interactable)
    {
        // Daha önceden bulunmadýysa veya deðiþtiyse event invoke edilir.
        if (m_CurrentInteractable == null ||
            m_CurrentInteractable != interactable)
        {
            m_CurrentInteractable = interactable;
            m_InteractableUnreachableOnce = false;

            OnNewInteractableDetected?.Invoke(interactable);
            Debug.Log("Interactable detected.");
        }
    }

    private void HandleUnreachableInteractable()
    {
        if (m_InteractableUnreachableOnce)
        {
            return;
        }

        m_CurrentInteractable = null;
        m_InteractableUnreachableOnce = true;

        OnNewInteractableUnreachable?.Invoke();
        Debug.Log("Interactable unreachable.");
    }

    #endregion
}
