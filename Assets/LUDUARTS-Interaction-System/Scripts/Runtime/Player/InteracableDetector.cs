using System;
using UnityEngine;

/// <summary>
/// raycast yaparak oyuncunun önünde etkileþime girebileceði obje var mý yok mu nulmak için kullanýlýr.
/// </summary>
/// <remarks>
/// raycast ile "Interactable" layermask'ý kullanarak objeleri bulur. 
/// obje çok uzaktaysa OnNewInteractableUnreachable eventini tetikler.
/// obje birden fazlaysa en yakýndakini seçer.
/// obje yoksa state deðiþikliðini bildirmek için OnInteractableNotDetected tetiklenir.
/// </remarks>
public class InteracableDetector : MonoBehaviour
{
    #region Fields

    [SerializeField] private float MaxInteractionDistance = 1f;
    [SerializeField] private LayerMask m_LayerMask;
    private Ray m_Ray;
    private RaycastHit[] m_HitResults = new RaycastHit[64];

    /// <summary>
    /// üst üste gereksiz event tetiklenmesini engellemek için kullanýlýr.
    /// </summary>
    private bool m_InteractableDetected = false;

    /// <summary>
    /// bir kez unreachable olduktan sonra gereksiz bir þekilde event tetiklemesi olmamasý için bu flag ile kontrol yapýlýr.
    /// </summary>
    private bool m_InteractableUnreachableOnce = false;

    private IInteractable m_CurrentInteractable;

    #endregion

    #region Events
    /// <summary>
    /// her farklý IInteractable sýnýfýndan obje bulunduðunda bu event tetiklenir.
    /// </summary>
    public static event Action<IInteractable> OnNewInteractableDetected;

    /// <summary>
    /// bulunan IInteractable objesi çok uzaksa tetiklenir. 
    /// genelde UI için kullanýlýr.
    /// </summary>
    public static event Action OnNewInteractableUnreachable;

    /// <summary>
    /// oyuncunun önünde bir obje yoksa tetiklenir.
    /// genelde UI için kullanýlýr.
    /// </summary>
    public static event Action OnInteractableNotDetected;


    #endregion


    #region Unity Methods

    private void Awake()
    {
        m_Ray = new Ray(transform.position, transform.forward);
    }

    #endregion

    private void FixedUpdate()
    {
        m_Ray.origin = transform.position;
        m_Ray.direction = transform.forward;

        // raycast:
        int hitCount = Physics.RaycastNonAlloc(m_Ray, m_HitResults, 10, m_LayerMask);

        // obje varsa en yakýndaki bulunur. yoksa olmadýðý bildirilir.
        if (hitCount > 0)
        {
            m_InteractableDetected = true;
            RaycastHit closestHit = FindClosestHitResult(hitCount);

            // En yakýn hit burada
            HandleClosestHit(closestHit);
        }

        // önceki frame içinde zaten bir obje bulunmadýysa tekrar çaðrýlmamalý
        else if(m_InteractableDetected == true)
        {
            m_CurrentInteractable = null;
            m_InteractableUnreachableOnce = false;
            m_InteractableDetected = false;

            OnInteractableNotDetected?.Invoke();
            Debug.Log("interactable not detected");
        }
    }

    private RaycastHit FindClosestHitResult(int hitCount)
    {
        RaycastHit closestHit = default;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < hitCount; i++)
        {
            float dist = m_HitResults[i].distance;

            if (dist < closestDistance)
            {
                closestDistance = dist;
                closestHit = m_HitResults[i];
            }
        }

        return closestHit;
    }

    private void HandleClosestHit(RaycastHit hit)
    {
        var interactable = hit.transform.GetComponentInParent<IInteractable>();
        if (interactable != null)
        {
            if(hit.distance <= MaxInteractionDistance)
            {
                // daha önceden bulunmadýysa yada deðiþtiyse event invoke edilmeli.
                if(m_CurrentInteractable == null || m_CurrentInteractable != interactable)
                {
                    m_CurrentInteractable = interactable;
                    m_InteractableUnreachableOnce = false;
                    OnNewInteractableDetected?.Invoke(interactable);
                    Debug.Log("interactable detected");
                }
            }
            else if(!m_InteractableUnreachableOnce)
            {
                m_CurrentInteractable = null;
                m_InteractableUnreachableOnce = true;
                OnNewInteractableUnreachable?.Invoke();
                Debug.Log("interactable unreachable");
            }
        }
        else
        {
            Debug.LogWarning("an object with interactable layer does not have an IInteractable component: " + hit.collider.name);
        }

    }
}
