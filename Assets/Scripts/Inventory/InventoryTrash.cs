using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryTrash : MonoBehaviour, IDropHandler {

	public Inventory parentInventory;

    // Use this for initialization
    void Start () {
		
	}

    public void OnDrop(PointerEventData eventData)
    {
		GameObject trashedItemObject = eventData.pointerDrag.gameObject;
        Debug.Log("Trashing item: " + trashedItemObject.GetComponent<Slot>().gameItem.title);

		if (parentInventory != null)
		{
			parentInventory.RemoveItems(trashedItemObject.GetComponent<Slot>().slotIndex);
		}

    }


}
