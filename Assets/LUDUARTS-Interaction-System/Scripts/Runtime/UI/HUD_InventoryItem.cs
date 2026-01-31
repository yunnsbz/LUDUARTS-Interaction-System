using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD_InventoryItem : MonoBehaviour
{
    [SerializeField] private Button m_Button;
    [SerializeField] private TMP_Text m_ItemName;

    private IItem m_Item;

    public void UpdateItem(IItem item)
    {
        m_Item = item;
        m_ItemName.text = item.DisplayName;
        m_Button.onClick.RemoveAllListeners();
        m_Button.onClick.AddListener(() =>
        {
            Inventory.Instance.RemoveItem(m_Item);
        });
    }
}
