using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemDataSO data;

    public string GetInteractPrompt()
    {
        string info = $"{data.DisplayName}\n{data.Description}";
        return info;
    }

    public void OnInteract()
    {
        //GameManager.Instance.AddItem(data);
        Destroy(gameObject);
    }
}
