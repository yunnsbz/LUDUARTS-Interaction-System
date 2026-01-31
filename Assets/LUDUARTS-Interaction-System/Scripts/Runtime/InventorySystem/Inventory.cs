using System;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class Inventory : MonoBehaviour
{
    [SerializeField] private int m_MaxSize = 20;

    private List<IItem> m_Items = new List<IItem>();

    public IReadOnlyList<IItem> Items => m_Items;

    public static Inventory Instance;

    public event Action<IItem> OnAddItem;
    public event Action<IItem> OnRemoveItem;

    private void Awake()
    {
        Instance = this;
    }

    public bool AddItem(IItem item)
    {
        if (m_Items.Count >= m_MaxSize)
        {
            Debug.Log("Inventory full!");
            return false;
        }

        m_Items.Add(item);

        item.GetGameObject().transform.position = new Vector3(0, -100, 0);

        Debug.Log($"Added: {item.DisplayName}");
        OnAddItem?.Invoke(item);
        return true;
    }

    public bool RemoveItem(IItem item)
    {
        if (m_Items.Remove(item))
        {
            item.GetGameObject().transform.position = transform.position + new Vector3( UnityEngine.Random.Range(-1f,1f),0, UnityEngine.Random.Range(-1f, 1f));

            Debug.Log($"Removed: {item.DisplayName}");
            OnRemoveItem?.Invoke(item);
            return true;
        }
        return false;
    }

    public List<IItem> GetAllItems()
    {
        return m_Items;
    }
}
