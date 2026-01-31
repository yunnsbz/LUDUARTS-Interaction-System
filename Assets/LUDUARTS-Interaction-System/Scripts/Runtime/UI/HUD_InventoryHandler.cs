using System.Collections.Generic;
using UnityEngine;

public class HUD_InventoryHandler : MonoBehaviour
{
    [SerializeField] private Canvas m_Canvas;
    [SerializeField] private PlayerInputActions m_Inputs;
    [SerializeField] private GameObject m_InventoryItemPrefab;
    [SerializeField] private Transform m_InventoryContainer;

    // UI tarafýndaki aktif itemler
    private readonly List<HUD_InventoryItem> m_SpawnedItems = new();

    private void Awake()
    {
        m_Inputs = InputActionProvider.Inputs;

        
    }

    private void InventoryToggle_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (m_Canvas.enabled)
        {
            CloseInventory();
        }
        else
        {
            OpenInventory();
        }
    }

    private void OnEnable()
    {
        Inventory.Instance.OnAddItem += OnInventoryChange;
        Inventory.Instance.OnRemoveItem += OnInventoryChange;
        m_Inputs.Player.InventoryToggle.started += InventoryToggle_started;

        Refresh();
    }


    private void Start()
    {
        m_Canvas.enabled = false;
    }

    public void OpenInventory()
    {
        m_Canvas.enabled = true;

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        Refresh();
    }


    public void CloseInventory()
    {
        m_Canvas.enabled = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    private void OnInventoryChange(IItem obj)
    {
        Refresh();
    }

    // Envanteri tamamen yeniden çizer
    public void Refresh()
    {
        ClearUI();

        var items = Inventory.Instance.GetAllItems();

        foreach (var item in items)
        {
            SpawnItem(item);
        }
    }

    // Tek bir item UI ekler
    public void SpawnItem(IItem item)
    {
        GameObject go = Instantiate(m_InventoryItemPrefab, m_InventoryContainer);
        HUD_InventoryItem uiItem = go.GetComponent<HUD_InventoryItem>();

        uiItem.UpdateItem(item);
        m_SpawnedItems.Add(uiItem);
    }

    // UI temizler
    private void ClearUI()
    {
        foreach (var item in m_SpawnedItems)
        {
            if (item != null)
                DestroyImmediate(item.gameObject);
        }

        m_SpawnedItems.Clear();
    }

}
