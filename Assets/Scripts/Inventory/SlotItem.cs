using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotItem : MonoBehaviour, IDropHandler {

	public GameItem item = null;

	// Use this for initialization
	void Start () {
		
	}

	public void OnDrop(PointerEventData eventData)
    {
		
		// Slot droppedSlot = eventData.pointerDrag.gameObject.GetComponent<Slot>();
		// Debug.Log(droppedSlot.draggedSlotItem.GetComponent<SlotItem>().item.Title);
		// GameItem droppedItem = droppedSlot.draggedSlotItem.GetComponent<SlotItem>().item;
		// droppedSlot.draggedSlotItem.GetComponent<SlotItem>().item = this.item != null ? this.item : null;
		// // droppedSlot.transform.Find("SlotItem").gameObject.GetComponent<SlotItem>().item = this.item;
		// Debug.Log("@OnDrop()");
		// GetComponentInParent<Slot>().SetItem(droppedItem);

    }

}
