using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Oyuncunun envanter HUD'ýný yöneten sýnýf.
/// Envanter açma/kapama, UI yenileme ve item spawn iþlemlerini yönetir.
/// </summary>
public class HUD_InventoryHandler : MonoBehaviour
{
    #region Fields

    [Header("References")]
    [SerializeField] private Canvas m_Canvas;
    [SerializeField] private PlayerInputActions m_Inputs;
    [SerializeField] private GameObject m_InventoryItemPrefab;
    [SerializeField] private Transform m_InventoryContainer;

    /// <summary>
    /// UI tarafýndaki aktif itemler.
    /// </summary>
    private readonly List<HUD_InventoryItem> m_SpawnedItems = new();

    #endregion

    #region Unity Methods

    private void Awake()
    {
        m_Inputs = InputActionProvider.Inputs;
    }

    private void OnEnable()
    {
        SubscribeEvents();
        Refresh();
    }

    private void Start()
    {
        m_Canvas.enabled = false;
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
    }

    #endregion

    #region Event Subscriptions

    private void SubscribeEvents()
    {
        Inventory.Instance.OnAddItem += OnInventoryChange;
        Inventory.Instance.OnRemoveItem += OnInventoryChange;

        m_Inputs.Player.InventoryToggle.started += OnInventoryToggleStarted;
    }

    private void UnsubscribeEvents()
    {
        if (Inventory.Instance != null)
        {
            Inventory.Instance.OnAddItem -= OnInventoryChange;
            Inventory.Instance.OnRemoveItem -= OnInventoryChange;
        }

        m_Inputs.Player.InventoryToggle.started -= OnInventoryToggleStarted;
    }

    #endregion

    #region Input Handling

    private void OnInventoryToggleStarted(InputAction.CallbackContext context)
    {
        if (m_Canvas.enabled)
        {
            CloseInventory();
            return;
        }

        OpenInventory();
    }

    #endregion

    #region Inventory UI

    /// <summary>
    /// Envanter UI'ýný açar ve imleci serbest býrakýr.
    /// </summary>
    public void OpenInventory()
    {
        m_Canvas.enabled = true;

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        Refresh();
    }

    /// <summary>
    /// Envanter UI'ýný kapatýr ve imleci kilitler.
    /// </summary>
    public void CloseInventory()
    {
        m_Canvas.enabled = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// Envanter deðiþtiðinde tetiklenir ve UI'ý yeniden çizer.
    /// </summary>
    private void OnInventoryChange(IItem item)
    {
        Refresh();
    }

    /// <summary>
    /// Envanteri tamamen yeniden çizer.
    /// </summary>
    public void Refresh()
    {
        ClearUI();

        var items = Inventory.Instance.GetAllItems();

        foreach (var item in items)
        {
            SpawnItem(item);
        }
    }

    /// <summary>
    /// Tek bir item için UI elemaný oluþturur.
    /// </summary>
    public void SpawnItem(IItem item)
    {
        GameObject instance = Instantiate(
            m_InventoryItemPrefab,
            m_InventoryContainer);

        HUD_InventoryItem uiItem =
            instance.GetComponent<HUD_InventoryItem>();

        uiItem.UpdateItem(item);
        m_SpawnedItems.Add(uiItem);
    }

    /// <summary>
    /// Tüm UI item'larýný temizler.
    /// </summary>
    private void ClearUI()
    {
        foreach (var item in m_SpawnedItems)
        {
            if (item == null)
            {
                continue;
            }

            DestroyImmediate(item.gameObject);
        }

        m_SpawnedItems.Clear();
    }

    #endregion
}
