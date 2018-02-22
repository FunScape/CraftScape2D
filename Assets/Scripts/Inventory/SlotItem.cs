using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotItem : MonoBehaviour, IDropHandler {

	GameItem _item = null;
	public GameItem item {
		get { return _item; }
		set { _item = value; UpdateStackLabelText(); }
	}

	public GameObject stackCountLabelPrefab;

	GameObject stackCountLabel;

	Slot parentSlot;

	// Use this for initialization
	void Start () {
		parentSlot = transform.parent.gameObject.GetComponent<Slot>();
		stackCountLabel = Text.Instantiate(stackCountLabelPrefab);
		stackCountLabel.transform.SetParent(this.transform);
		UpdateStackLabelText();
	}

	public void OnDrop(PointerEventData eventData)
    {		
		Slot droppedSlot = eventData.pointerDrag.gameObject.GetComponent<Slot>();

		if (droppedSlot.draggedSlotItem != null)
		{
			GameItem droppedItem = droppedSlot.draggedSlotItem.GetComponent<SlotItem>().item;
			droppedSlot.draggedSlotItem.GetComponent<SlotItem>().item = this.item;
			this.item = droppedItem;
			parentSlot.SetItem(this.item);
		}

    }

	public void UpdateStackLabelText()
	{
		if (stackCountLabel != null)
		{
			if (_item != null && _item.maxStackSize > 1)
				stackCountLabel.GetComponent<Text>().text = item.stackSize.ToString();
			else
				stackCountLabel.GetComponent<Text>().text = "";
		}
	}

}
