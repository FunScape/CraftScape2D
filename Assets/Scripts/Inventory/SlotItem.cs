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
		
		SlotItem droppedSlot = eventData.pointerDrag.GetComponent<SlotItem>();

		Debug.Log("Dropped Slot Position: " + droppedSlot.transform.position.x + "x" + droppedSlot.transform.position.y);

		// if (destinationSlot)

		GameItem temp = droppedSlot.item;
		droppedSlot.item = this.item;
		this.item = temp;
		
		GetComponentInParent<Slot>().SetItem(this.item);
    }

}
