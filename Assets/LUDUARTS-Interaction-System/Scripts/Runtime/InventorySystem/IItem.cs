using UnityEngine;

public interface IItem
{
    string DisplayName { get; }


    GameObject GetGameObject();
}