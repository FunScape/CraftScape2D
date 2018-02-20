using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler {

	static Sprite defaultSprite;

	GameObject backgroundImage { get { return transform.Find("Background").gameObject; } }
	GameObject slotItem { get { return transform.Find("SlotItem").gameObject; } }

	public GameObject draggedSlotItem;

	Color clearColor {
		get {
			Color color = Color.white;
			color.a = 0f;
			return color;
		}
	}

	private GameItem _item;
	public GameItem Item { get { return _item; } }

	void Start()
	{
		if (defaultSprite == null)
			defaultSprite = (Sprite)Resources.Load("Sprites/RPG_inventory_icons/f", typeof(Sprite));
	}


	public void SetItem(GameItem item)
	{
		_item = item;
		slotItem.GetComponent<SlotItem>().item = item;
		try
		{
			slotItem.GetComponent<Image>().color = clearColor;
		}catch (System.Exception){}

		Debug.Log("@SetItem(): Adding " + item.spriteName);
		Image image = slotItem.gameObject.GetComponent<Image>();
		image.sprite = (Sprite)Resources.Load(ItemDatabase.itemSpritesPath + item.spriteName, typeof(Sprite));
		image.color = Color.white;
		// slotItem.gameObject.GetComponent<Image>().sprite = item.Sprite;
	}

	void OnEnable()
	{
		if (Item != null && Item.Sprite != null)
		{
			slotItem.gameObject.GetComponent<Image>().color = clearColor;
			slotItem.gameObject.GetComponent<Image>().sprite = _item.Sprite;
		}
		else
		{
			slotItem.gameObject.GetComponent<Image>().color = Color.white;
		}
	}

	public void SetEnabledIfItemExists()
	{
		slotItem.gameObject.GetComponent<Image>().color = _item != null ? Color.white : clearColor;
	}

	public void OnBeginDrag(PointerEventData eventData)
    {
        if (_item != null)
		{
			// Slot[] allSlots = this.transform.parent.GetComponentsInChildren<Slot>();
			// foreach (Slot group in allSlots)
			// {
			// 	group.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
			// }
			GetComponent<CanvasGroup>().blocksRaycasts = false;
			draggedSlotItem = this.slotItem;
			draggedSlotItem.GetComponent<CanvasGroup>().blocksRaycasts = false;
			draggedSlotItem.transform.SetParent(this.transform.parent);
			draggedSlotItem.transform.position = eventData.position;
		}
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_item != null)
		{
			draggedSlotItem.transform.position = eventData.position;
		}
    }

    public void OnEndDrag(PointerEventData eventData)
    {
		if (draggedSlotItem != null)
		{
			Slot[] allSlots = this.transform.parent.GetComponentsInChildren<Slot>();
			foreach (Slot group in allSlots)
			{
				group.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
			}

			draggedSlotItem.transform.SetParent(this.transform);
			draggedSlotItem.transform.position = this.transform.position;
			draggedSlotItem.GetComponent<CanvasGroup>().blocksRaycasts = true;
			this.SetItem(draggedSlotItem.GetComponent<SlotItem>().item);
			draggedSlotItem = null;
		}
    }

	public void OnDrop(PointerEventData eventData)
    {
		
		// Slot droppedSlot = eventData.pointerDrag.gameObject.GetComponent<Slot>();
		// Debug.Log(droppedSlot.draggedSlotItem.GetComponent<SlotItem>().item.Title);
		// GameItem droppedItem = droppedSlot.draggedSlotItem.GetComponent<SlotItem>().item;
		// droppedSlot.draggedSlotItem.GetComponent<SlotItem>().item = this.item != null ? this.item : null;
		// droppedSlot.transform.Find("SlotItem").gameObject.GetComponent<SlotItem>().item = this.item;
		Debug.Log("@OnDrop()");
		// GetComponentInParent<Slot>().SetItem(droppedItem);

    }

}
