using UnityEngine;

public class KeyInteractable : AInteractable
{
    private IItem m_KeyItem;


    private void Awake()
    {
        if(!TryGetComponent<IItem>(out m_KeyItem))
        {
            Debug.LogError("key interactable should have a key item component");
        }
    }

    protected override void OnInteractStartCore()
    {
        // anahtarý alýp envantere yükleme
        Inventory.Instance.AddItem(m_KeyItem);



    }
}
