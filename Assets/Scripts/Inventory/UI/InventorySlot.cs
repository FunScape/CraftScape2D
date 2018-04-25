using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	public int slotIndex;

	// public InventoryItem gameItem { get; set; }

    InventoryController inventoryController;

	public GameObject draggedItem;

    GameObject owner;

	public void OnBeginDrag(PointerEventData eventData)
    {
		// Only begin dragging the item with the left mouse button
		if (eventData.button == PointerEventData.InputButton.Left) {
			owner = GameManager.GetLocalPlayer();
			inventoryController = owner.GetComponent<InventoryController>();

			draggedItem = inventoryController.OnBeginDragInventoryItem(slotIndex);
		}
    }

    public void OnDrag(PointerEventData eventData)
    {
		if (draggedItem != null && eventData.button == PointerEventData.InputButton.Left)
		{
			draggedItem.transform.position = eventData.position;

			GameObject player = GameManager.GetLocalPlayer();
            inventoryController = player.GetComponent<InventoryController>();
			inventoryController.OnDragInventoryItem(eventData);
		}
    }

    public void OnEndDrag(PointerEventData eventData)
    {
		if (draggedItem != null && eventData.button == PointerEventData.InputButton.Left) {
			GameObject player = GameManager.GetLocalPlayer();
			inventoryController = player.GetComponent<InventoryController>();
			inventoryController.OnEndDragInventoryItem(this.slotIndex);
			draggedItem.GetComponent<CanvasGroup>().blocksRaycasts = true;
			draggedItem = null;
		}
    }
	
	public void OnDropInventoryItem(GameObject dropped)
	{	
		GameObject player = GameManager.GetLocalPlayer();
        inventoryController = player.GetComponent<InventoryController>();

		if (dropped.tag == "EquipmentSlot")
		{
			inventoryController.OnDropEquipmentItem(gameObject, dropped);
		}
		else if (dropped.tag == "InventorySlot")
		{
			inventoryController.SwapInventorySlots(gameObject, dropped);
		}
	}

}
