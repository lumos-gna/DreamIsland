using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public string GetInteractPrompt();
    public void OnInteract();
}

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;

    public string GetInteractPrompt()
    {
        string info = $"{data.displayName}\n{data.description}";
        return info;
    }

    public void OnInteract()
    {
        PlayerManager.Instance._Player.itemData = data;
        PlayerManager.Instance._Player.addItem?.Invoke(data);
        Destroy(gameObject);
    }
}
