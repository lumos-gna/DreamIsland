using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
        GameManager.Instance.AddItem(data);
        Destroy(gameObject);
    }
}
