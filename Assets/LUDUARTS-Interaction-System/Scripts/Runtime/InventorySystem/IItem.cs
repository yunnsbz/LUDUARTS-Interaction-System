using UnityEngine;

public interface IItem
{
    string DisplayName { get; }

    void OnUse();

    GameObject GetGameObject();
}