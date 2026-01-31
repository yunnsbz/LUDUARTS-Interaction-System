using UnityEngine;

public enum KeyTypes
{
    Blue,
    Green,
    Red
}

public class KeyItem : MonoBehaviour, IItem
{
    [SerializeField] private KeyTypes m_KeyType;
    public KeyTypes KeyType => m_KeyType;

    public string DisplayName => m_KeyType + " Key";

    void IItem.OnUse()
    {
        
    }

    GameObject IItem.GetGameObject()
    {
        return gameObject; 
    }

}
