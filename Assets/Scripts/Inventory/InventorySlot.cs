using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	public int slotIndex;

	public InventoryItem gameItem { get; set; }

	PlayerInventoryController inventoryController;

	public GameObject draggedItem;

	void Start()
	{
		
	}

	public void OnBeginDrag(PointerEventData eventData)
    {
		GameObject player = GameObject.FindWithTag("Player");
		inventoryController = player.GetComponent<PlayerInventoryController>();
		draggedItem = inventoryController.OnBeginDragInventoryItem(slotIndex);
    }

    public void OnDrag(PointerEventData eventData)
    {
		if (draggedItem != null)
		{
			draggedItem.transform.position = eventData.position;

			GameObject player = GameObject.FindWithTag("Player");
			inventoryController = player.GetComponent<PlayerInventoryController>();
			inventoryController.OnDragInventoryItem(eventData);
		}
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject player = GameObject.FindWithTag("Player");
		inventoryController = player.GetComponent<PlayerInventoryController>();
		inventoryController.OnEndDragInventoryItem(eventData);
    }
	
	public void OnDropInventoryItem(GameObject dropped)
	{	
		GameObject player = GameObject.FindWithTag("Player");
		inventoryController = player.GetComponent<PlayerInventoryController>();
		inventoryController.SwapInventorySlots(this.gameObject, dropped);
	}

}
