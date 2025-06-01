
using System;
using UnityEngine;

public class Alter : MonoBehaviour, IInteractable
{
    public Outline Outline { get; private set; }

    public GameObject fireEffect;
    public GameObject portalPosition;
    public GameObject portal;

    [SerializeField] private ItemData targetItemData;
    [SerializeField] private int targetItemCount;


    private Inventory _inventory;
    

    private void Awake()
    {
        Outline = GetComponent<Outline>();

        _inventory = GameManager.Instance.Inventory;
    }

    
    //테스트용, 나중에 삭제
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            fireEffect.SetActive(true);
            GameObject newPortal = Instantiate(portal, portalPosition.transform.position, portalPosition.transform.rotation);
            newPortal.transform.SetParent(portalPosition.transform);
        }
    }

    public void OnInteract()
    {
        //해당 아이템이 슬롯에 목표만큼 있는지 
        if (_inventory.FindSlot((slot) => slot.item == targetItemData && slot.quantity >= targetItemCount) != null)
        {
            fireEffect.SetActive(true);
            GameObject newPortal = Instantiate(portal, portalPosition.transform.position, portalPosition.transform.rotation);
            newPortal.transform.SetParent(portalPosition.transform);
        }
    }
}
